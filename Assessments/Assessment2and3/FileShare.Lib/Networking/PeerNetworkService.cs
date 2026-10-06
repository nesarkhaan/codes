namespace FileShare.Lib.Networking;

using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

public sealed record PeerConnectionInfo(string Id, string Nickname, string IpAddress, int Port, bool IsConnected);

// Accepts and creates simple-share peer connections and services protocol messages.</summary>
public sealed class PeerNetworkService : IAsyncDisposable
{
    
    private const int MaxResponseData = 3054;
    private readonly P2PFileManager _files;
    private readonly ConcurrentDictionary<string, PeerSession> _peers = new();
    private TcpListener? _listener;
    private CancellationTokenSource? _listenerCts;
    private Task? _acceptTask;
    private string _nickname = "peer-iso";
    private int _maxPeers = 128;
    private int _unknownCounter;

    public PeerNetworkService(P2PFileManager files) => _files = files;

    public IReadOnlyList<PeerConnectionInfo> GetPeers() => _peers.Values
        .OrderBy(p => p.IpAddress).ThenBy(p => p.Port)
        .Select(p => p.Info).ToArray();

    public async Task StartListeningAsync(int port, string nickname, int maxPeers, CancellationToken cancellationToken = default)
    {
        await StopListeningAsync();
        _nickname = nickname;
        _maxPeers = maxPeers;
        var listener = new TcpListener(IPAddress.Any, port);
        try { listener.Start(); }
        catch (SocketException ex) { throw new IOException("Port specified is either in use or invlaid", ex); }
        _listener = listener;
        _listenerCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _acceptTask = AcceptLoopAsync(listener, _listenerCts.Token);
    }

    public async Task StopListeningAsync()
    {
        if (_listenerCts is null) return;
        _listenerCts.Cancel();
        _listener?.Stop();
        if (_acceptTask is not null)
        {
            try { await _acceptTask; } catch (OperationCanceledException) { } catch (ObjectDisposedException) { }
        }
        _listenerCts.Dispose();
        _listenerCts = null;
        _listener = null;
        _acceptTask = null;
    }

    public async Task<(bool Success, string Message, string? PeerId)> ConnectAsync(string ipAddress, int port, CancellationToken cancellationToken = default)
    {
        var key = Key(ipAddress, port);
        if (_peers.TryGetValue(key, out var existing) && existing.IsConnected)
            return (false, "Already connected to peer", existing.Id);
        if (_peers.Values.Count(p => p.IsConnected) >= _maxPeers)
            return (false, "Maximum peer limit reached", null);

        var client = new TcpClient();
        try
        {
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeout.CancelAfter(TimeSpan.FromSeconds(5));
            await client.ConnectAsync(ipAddress, port, timeout.Token);
            var stream = client.GetStream();
            await SimpleShareProtocol.WriteAsync(stream, new(SimpleShareCode.Acp, []), timeout.Token);
            var response = await SimpleShareProtocol.ReadAsync(stream, timeout.Token);
            if (response?.Code != SimpleShareCode.Acp)
                throw new InvalidDataException("Peer did not acknowledge connection.");
            await SimpleShareProtocol.WriteAsync(stream, new(SimpleShareCode.Ack, []), timeout.Token);

            var nickname = await RequestNicknameAsync(stream, timeout.Token);
            var session = new PeerSession(client, ipAddress, port, nickname ?? $"Unknown#{Interlocked.Increment(ref _unknownCounter)}");
            _peers[key] = session;
            session.RunTask = HandlePeerAsync(session, CancellationToken.None);
            return (true, "Connected to peer.", session.Id);
        }
        catch (Exception ex) when (ex is SocketException or IOException or OperationCanceledException or InvalidDataException)
        {
            client.Dispose();
            return (false, "Unable to connect to peer", null);
        }
    }

    public async Task<(bool Success, string Message)> DisconnectAsync(string peerId)
    {
        var session = _peers.Values.FirstOrDefault(p => p.Id == peerId);
        if (session is null || !session.IsConnected) return (false, "Unknown peer, not connected");
        try { await SimpleShareProtocol.WriteAsync(session.Client.GetStream(), new(SimpleShareCode.Dsn, [])); } catch { }
        session.Close();
        return (true, "Disconnected from peer.");
    }

    // Removes a peer from the active peer collection.
    public async Task<(bool Success, string Message)> RemoveAsync(string peerId)
    {
        var session = _peers.Values.FirstOrDefault(p => p.Id == peerId);

        if (session is null)
            return (false, "Unknown peer");

        if (session.IsConnected)
        {
            try
            {
                await SimpleShareProtocol.WriteAsync(
                    session.Client.GetStream(),
                    new(SimpleShareCode.Dsn, []));
            }
            catch { }
        }

        session.Close();

        var key = Key(session.IpAddress, session.Port);
        _peers.TryRemove(key, out _);

        return (true, "Peer removed.");
    }

    
    public async Task<(bool Success, string Message)> ReconnectAsync(string peerId, CancellationToken cancellationToken = default)
    {
        var session = _peers.Values.FirstOrDefault(p => p.Id == peerId);
        if (session is null) return (false, "Unknown peer, not connected");
        if (session.IsConnected) return (false, "Already connected to peer");
        var result = await ConnectAsync(session.IpAddress, session.Port, cancellationToken);
        return (result.Success, result.Message);
    }

    public async Task<bool> DownloadChunkAsync(string peerId, string identifier, int chunkIndex, CancellationToken cancellationToken = default)
    {
        var peer = _peers.Values.FirstOrDefault(p => p.Id == peerId && p.IsConnected);
        if (peer is null || !_files.TryGetChunkInfo(identifier, chunkIndex, out var chunkSize) || chunkSize <= 0)
            return false;

        var fullChunk = new byte[chunkSize];
        var position = 0;
        while (position < chunkSize)
        {
            // REQ length is encoded as uint16 in this implementation, so request a chunk in ranges.
            var length = Math.Min(ushort.MaxValue, chunkSize - position);
            var key = $"{identifier}:{chunkIndex}:{position}";
            var pending = new PendingRequest(position, length);
            if (!peer.Pending.TryAdd(key, pending)) return false;

            try
            {
                var payload = BuildRequest(identifier, (uint)chunkIndex, (uint)position, (ushort)length);
                await peer.SendLock.WaitAsync(cancellationToken);
                try
                {
                    await SimpleShareProtocol.WriteAsync(peer.Client.GetStream(), new(SimpleShareCode.Req, payload), cancellationToken);
                }
                finally { peer.SendLock.Release(); }

                using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeout.CancelAfter(TimeSpan.FromSeconds(10));
                var data = await pending.Completion.Task.WaitAsync(timeout.Token);
                if (data.Length != length) return false;
                Buffer.BlockCopy(data, 0, fullChunk, position, data.Length);
                position += data.Length;
            }
            catch (Exception ex) when (ex is OperationCanceledException or IOException or SocketException)
            {
                return false;
            }
            finally
            {
                peer.Pending.TryRemove(key, out _);
            }
        }

        return _files.WriteAndVerifyChunkFromPeer(identifier, chunkIndex, fullChunk);
    }

    private static byte[] BuildRequest(string identifier, uint chunkIndex, uint offset, ushort length)
    {
        var payload = new byte[1034];
        SimpleShareProtocol.EncodeIdentifier(identifier).CopyTo(payload, 0);
        BinaryPrimitives.WriteUInt32LittleEndian(payload.AsSpan(1024, 4), chunkIndex);
        BinaryPrimitives.WriteUInt32LittleEndian(payload.AsSpan(1028, 4), offset);
        BinaryPrimitives.WriteUInt16LittleEndian(payload.AsSpan(1032, 2), length);
        return payload;
    }

    private async Task AcceptLoopAsync(TcpListener listener, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            TcpClient client;
            try { client = await listener.AcceptTcpClientAsync(cancellationToken); }
            catch (OperationCanceledException) { break; }
            catch (ObjectDisposedException) { break; }

            if (_peers.Values.Count(p => p.IsConnected) >= _maxPeers) { client.Dispose(); continue; }
            var remote = (IPEndPoint)client.Client.RemoteEndPoint!;
            _ = Task.Run(async () =>
            {
                try
                {
                    var stream = client.GetStream();
                    using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                    var first = await SimpleShareProtocol.ReadAsync(stream, timeout.Token);
                    if (first?.Code != SimpleShareCode.Acp) { client.Dispose(); return; }
                    await SimpleShareProtocol.WriteAsync(stream, new(SimpleShareCode.Acp, []), timeout.Token);
                    var ack = await SimpleShareProtocol.ReadAsync(stream, timeout.Token);
                    if (ack?.Code != SimpleShareCode.Ack) { client.Dispose(); return; }
                    var session = new PeerSession(client, remote.Address.ToString(), remote.Port,
                        $"Unknown#{Interlocked.Increment(ref _unknownCounter)}");
                    _peers[Key(session.IpAddress, session.Port)] = session;
                    session.RunTask = HandlePeerAsync(session, cancellationToken);
                }
                catch { client.Dispose(); }
            }, cancellationToken);
        }
    }

    private async Task<string?> RequestNicknameAsync(NetworkStream stream, CancellationToken cancellationToken)
    {
        await SimpleShareProtocol.WriteAsync(stream, new(SimpleShareCode.Ida, []), cancellationToken);
        var message = await SimpleShareProtocol.ReadAsync(stream, cancellationToken);
        if (message?.Code != SimpleShareCode.Idr) return null;
        var nickname = Encoding.UTF8.GetString(message.Payload);
        return nickname.Length <= 512 ? nickname : nickname[..512];
    }

    private async Task HandlePeerAsync(PeerSession peer, CancellationToken cancellationToken)
    {
        try
        {
            var stream = peer.Client.GetStream();
            while (peer.IsConnected && !cancellationToken.IsCancellationRequested)
            {
                var message = await SimpleShareProtocol.ReadAsync(stream, cancellationToken);
                if (message is null) break;
                switch (message.Code)
                {
                    case SimpleShareCode.Acp:
                        await SimpleShareProtocol.WriteAsync(stream, new(SimpleShareCode.Ack, []), cancellationToken);
                        break;
                    case SimpleShareCode.Ida:
                        await SimpleShareProtocol.WriteAsync(stream,
                            new(SimpleShareCode.Idr, Encoding.UTF8.GetBytes(_nickname)), cancellationToken);
                        break;
                    case SimpleShareCode.Png:
                        await SimpleShareProtocol.WriteAsync(stream, new(SimpleShareCode.Pog, []), cancellationToken);
                        break;
                    case SimpleShareCode.Dsn:
                        peer.Close();
                        return;
                    case SimpleShareCode.Req:
                        await HandleRequestAsync(stream, message.Payload, cancellationToken);
                        break;
                    case SimpleShareCode.Res:
                        HandleResponse(peer, message.Payload);
                        break;
                    case SimpleShareCode.Pog:
                    case SimpleShareCode.Ack:
                    case SimpleShareCode.Idr:
                        break;
                    default:
                        await SimpleShareProtocol.WriteAsync(stream, new(SimpleShareCode.Err, []), cancellationToken);
                        break;
                }
            }
        }
        catch (Exception ex) when (ex is IOException or SocketException or OperationCanceledException or InvalidDataException) { }
        finally { peer.Close(); }
    }

    private static void HandleResponse(PeerSession peer, byte[] payload)
    {
        if (payload.Length < 1035) return;
        var identifier = SimpleShareProtocol.DecodeIdentifier(payload.AsSpan(0, 1024));
        var chunkIndex = BinaryPrimitives.ReadUInt32LittleEndian(payload.AsSpan(1024, 4));
        var offset = BinaryPrimitives.ReadUInt32LittleEndian(payload.AsSpan(1028, 4));
        var dataLength = BinaryPrimitives.ReadUInt16LittleEndian(payload.AsSpan(1032, 2));
        var error = payload[1034];
        if (payload.Length != 1035 + dataLength) return;

        var offsetInt = checked((int)offset);
        var match = peer.Pending.FirstOrDefault(pair =>
            pair.Key.StartsWith($"{identifier}:{chunkIndex}:", StringComparison.OrdinalIgnoreCase) &&
            offsetInt >= pair.Value.Start && offsetInt < pair.Value.Start + pair.Value.ExpectedLength);
        if (string.IsNullOrEmpty(match.Key)) return;

        var pending = match.Value;
        if (error != 0)
        {
            pending.Completion.TrySetResult([]);
            return;
        }

        var relative = offsetInt - pending.Start;
        if (relative < 0 || relative + dataLength > pending.Buffer.Length) return;
        payload.AsSpan(1035, dataLength).CopyTo(pending.Buffer.AsSpan(relative));
        pending.Received += dataLength;
        if (pending.Received >= pending.ExpectedLength)
            pending.Completion.TrySetResult(pending.Buffer);
    }

    private async Task HandleRequestAsync(NetworkStream stream, byte[] payload, CancellationToken cancellationToken)
    {
        // REQ payload: 1024-byte identifier, uint32 chunk index, uint32 offset, uint16 request length.
        if (payload.Length != 1034) return;
        var identifier = SimpleShareProtocol.DecodeIdentifier(payload.AsSpan(0, 1024));
        var chunkIndex = BinaryPrimitives.ReadUInt32LittleEndian(payload.AsSpan(1024, 4));
        var offset = BinaryPrimitives.ReadUInt32LittleEndian(payload.AsSpan(1028, 4));
        var requested = BinaryPrimitives.ReadUInt16LittleEndian(payload.AsSpan(1032, 2));
        if (requested == 0 || !_files.TryGetChunkInfo(identifier, checked((int)chunkIndex), out var chunkSize) || offset >= (uint)chunkSize)
        {
            await SendErrorResponseAsync(stream, identifier, chunkIndex, offset, cancellationToken);
            return;
        }

        var chunk = _files.GetChunkForPeer(identifier, checked((int)chunkIndex));
        if (chunk.Length == 0)
        {
            await SendErrorResponseAsync(stream, identifier, chunkIndex, offset, cancellationToken);
            return;
        }

        var remaining = Math.Min(requested, chunk.Length - checked((int)offset));
        var sent = 0;
        while (sent < remaining)
        {
            var take = Math.Min(MaxResponseData, remaining - sent);
            var data = chunk.AsSpan(checked((int)offset) + sent, take).ToArray();
            var response = BuildResponse(identifier, chunkIndex, offset + (uint)sent, data, 0);
            await SimpleShareProtocol.WriteAsync(stream, new(SimpleShareCode.Res, response), cancellationToken);
            sent += take;
        }
    }

    private static async Task SendErrorResponseAsync(NetworkStream stream, string identifier, uint chunkIndex, uint offset, CancellationToken ct)
        => await SimpleShareProtocol.WriteAsync(stream, new(SimpleShareCode.Res, BuildResponse(identifier, chunkIndex, offset, [], 1)), ct);

    private static byte[] BuildResponse(string identifier, uint chunkIndex, uint offset, byte[] data, byte error)
    {
        // RES payload: identifier, chunk index, offset, data length, error byte, data.
        var payload = new byte[1024 + 4 + 4 + 2 + 1 + data.Length];
        SimpleShareProtocol.EncodeIdentifier(identifier).CopyTo(payload, 0);
        BinaryPrimitives.WriteUInt32LittleEndian(payload.AsSpan(1024, 4), chunkIndex);
        BinaryPrimitives.WriteUInt32LittleEndian(payload.AsSpan(1028, 4), offset);
        BinaryPrimitives.WriteUInt16LittleEndian(payload.AsSpan(1032, 2), checked((ushort)data.Length));
        payload[1034] = error;
        data.CopyTo(payload, 1035);
        return payload;
    }

    private static string Key(string ip, int port) => $"{ip}:{port}";

    public async ValueTask DisposeAsync()
    {
        await StopListeningAsync();
        foreach (var peer in _peers.Values) peer.Close();
    }

    private sealed class PeerSession(TcpClient client, string ipAddress, int port, string nickname)
    {
        public string Id { get; } = Guid.NewGuid().ToString("N");
        public TcpClient Client { get; } = client;
        public string IpAddress { get; } = ipAddress;
        public int Port { get; } = port;
        public string Nickname { get; } = nickname;
        public bool IsConnected => Client.Connected;
        public Task? RunTask { get; set; }
        public SemaphoreSlim SendLock { get; } = new(1, 1);
        public ConcurrentDictionary<string, PendingRequest> Pending { get; } = new();
        public PeerConnectionInfo Info => new(Id, Nickname, IpAddress, Port, IsConnected);
        public void Close()
        {
            foreach (var pending in Pending.Values) pending.Completion.TrySetResult([]);
            try { Client.Close(); } catch { }
        }
    }

    private sealed class PendingRequest(int start, int expectedLength)
    {
        public int Start { get; } = start;
        public int ExpectedLength { get; } = expectedLength;
        public byte[] Buffer { get; } = new byte[expectedLength];
        public int Received { get; set; }
        public TaskCompletionSource<byte[]> Completion { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
    }
}
