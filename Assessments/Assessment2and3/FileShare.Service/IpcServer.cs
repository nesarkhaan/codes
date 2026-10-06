using System.IO.Pipes;
using FileShare.Lib;
using FileShare.Lib.Configuration;
using FileShare.Lib.Networking;
using FileShare.Lib.Packaging;

namespace FileShare.Service;
// The IpcServer class handles inter process communication between the daemon and the Web application.
public class IpcServer
{
    // Dependencies for managing files, networking, configuration, and snapshot storage.
    private readonly P2PFileManager _fileManager;

    // The PeerNetworkService manages peer connections and network communication.
    private readonly PeerNetworkService _networkService;

    // The FileShareConfig holds configuration settings for the file sharing application.
    private readonly FileShareConfig _config;

    // The FileShareSnapshotStore is responsible for persisting the current state of files and peers to the database.
    private readonly FileShareSnapshotStore _snapshotStore;

    // Constructor that initializes the IpcServer with the required dependencies.
    public IpcServer(P2PFileManager fileManager, PeerNetworkService networkService, FileShareConfig config, FileShareSnapshotStore snapshotStore)
    {
        _fileManager = fileManager;
        _networkService = networkService;
        _config = config;
        _snapshotStore = snapshotStore;
    }

    // Starts the IPC server to listen for incoming commands from the Web application.
    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Daemon: IPC Pipe Server is starting...");

        while (!cancellationToken.IsCancellationRequested)
        {
            // Create a named pipe server for IPC communication with the Web application.
            using var pipeServer = new NamedPipeServerStream(
                IpcClient.PipeName,
                PipeDirection.InOut,
                1,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous);

            try
            {
                await pipeServer.WaitForConnectionAsync(cancellationToken);

                using var reader = new StreamReader(pipeServer);
                using var writer = new StreamWriter(pipeServer) { AutoFlush = true };
                var commandLine = await reader.ReadLineAsync(cancellationToken);

                if (!string.IsNullOrWhiteSpace(commandLine))
                    await ProcessCommandAsync(commandLine, writer, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Daemon IPC Error: {ex.Message}");
            }
        }
    }

    // Processes commands received from the Web application via IPC.
    private async Task ProcessCommandAsync(
        string commandLine,
        StreamWriter writer,
        CancellationToken cancellationToken)
    {
        var parts = commandLine.Split('|');
        var action = parts[0].ToUpperInvariant();

        try
        {
            switch (action)
            {
                // this command generates the apk file.
               case "CREATE_TPK":
                {
                    if (parts.Length != 2)
                        throw new ArgumentException("Requires source file path.");

                    string sourcePath = parts[1].Trim();
                    if (!File.Exists(sourcePath))
                        throw new FileNotFoundException("Source file does not exist.", sourcePath);
                    //if directory doesnt exist then create it based on values fromc onfig
                    Directory.CreateDirectory(_config.FileDirectory);
                    Directory.CreateDirectory(_config.MetaFileDirectory);

                    string fileName = Path.GetFileName(sourcePath);
                    string targetDatPath = Path.Combine(_config.FileDirectory, fileName);
                    string targetTpkPath = Path.Combine(
                        _config.MetaFileDirectory,
                        Path.ChangeExtension(fileName, ".tpk"));

                    if (!string.Equals(Path.GetFullPath(sourcePath), Path.GetFullPath(targetDatPath), StringComparison.OrdinalIgnoreCase))
                    {
                        File.Copy(sourcePath, targetDatPath, overwrite: true);
                    }

                    var maker = new TpkFileMaker(targetDatPath, targetTpkPath);
                    maker.Build();

                    // Unique variable name to avoid CS0136 collision with ADD_PACKAGE
                    bool packageRegistered = _fileManager.AddPackage(targetTpkPath);
                    _snapshotStore.PersistSnapshot(_fileManager, _networkService);

                    await writer.WriteLineAsync(
                        packageRegistered
                            ? $"SUCCESS|Created and tracking package: {maker.Ident}"
                            : "ERROR|Package generated but failed to register in manager.");
                    break;
                }

                case "GET_PEERS":
                    _snapshotStore.PersistSnapshot(_fileManager, _networkService);
                    var peers = _networkService.GetPeers();
                    if (peers.Count == 0)
                    {
                        await writer.WriteLineAsync("SUCCESS|No connected peers.");
                    }
                    else
                    {
                        var response = string.Join(
                            ";",
                            peers.Select(peer =>
                                $"{peer.Id},{peer.Nickname},{peer.IpAddress},{peer.Port},{peer.IsConnected}"));
                        await writer.WriteLineAsync($"SUCCESS|{response}");
                    }
                    break;

                case "CONNECT":
                    if (parts.Length != 3)
                        throw new ArgumentException("Requires IP and Port.");

                    var connectResult = await _networkService.ConnectAsync(
                        parts[1],
                        int.Parse(parts[2]),
                        cancellationToken);
                    _snapshotStore.PersistSnapshot(_fileManager, _networkService);
                    await writer.WriteLineAsync(
                        connectResult.Success
                            ? $"SUCCESS|{connectResult.Message}"
                            : $"ERROR|{connectResult.Message}");
                    break;

                case "ADD_PACKAGE":
                    if (parts.Length != 2)
                        throw new ArgumentException("Requires File Path.");

                    var added = _fileManager.AddPackage(parts[1]);
                    _snapshotStore.PersistSnapshot(_fileManager, _networkService);
                    await writer.WriteLineAsync(
                        added
                            ? "SUCCESS|Package added and tracking started."
                            : "ERROR|Failed to add package. Check path or duplicates.");
                    break;

                case "GET_PACKAGES":
                    _snapshotStore.PersistSnapshot(_fileManager, _networkService);
                    var packages = _fileManager.GetPackageDetails();
                    if (packages.Count == 0)
                    {
                        await writer.WriteLineAsync("SUCCESS|No packages managed.");
                    }
                    else
                    {
                        var packageResponse = string.Join(
                            ";",
                            packages.Select(package =>
                                $"{package.Filename}, " +
                                $"{(package.CompletedBlocks / (double)Math.Max(1, package.TotalBlocks) * 100):0.0}%"));
                        await writer.WriteLineAsync($"SUCCESS|{packageResponse}");
                    }
                    break;

                case "REMOVE_PACKAGE":
                    if (parts.Length != 2)
                        throw new ArgumentException("Requires package identifier.");

                    var removed = _fileManager.RemovePackage(parts[1]);
                    _snapshotStore.PersistSnapshot(_fileManager, _networkService);
                    await writer.WriteLineAsync(
                        removed
                            ? "SUCCESS|Package has been removed"
                            : "ERROR|Identifier provided does not match managed packages");
                    break;

                case "DISCONNECT":
                    if (parts.Length != 2)
                        throw new ArgumentException("Requires peer identifier.");

                    var disconnectId = _snapshotStore.GetRuntimePeerId(parts[1]);
                    var disconnectResult = string.IsNullOrWhiteSpace(disconnectId)
                        ? (false, "Unknown peer, not connected")
                        : await _networkService.DisconnectAsync(disconnectId);
                    _snapshotStore.PersistSnapshot(_fileManager, _networkService);
                    await writer.WriteLineAsync(
                        disconnectResult.Item1
                            ? $"SUCCESS|{disconnectResult.Item2}"
                            : $"ERROR|{disconnectResult.Item2}");
                    break;

                case "RECONNECT":
                    if (parts.Length != 2)
                        throw new ArgumentException("Requires peer identifier.");

                    var reconnectId = _snapshotStore.GetRuntimePeerId(parts[1]);
                    var reconnectResult = string.IsNullOrWhiteSpace(reconnectId)
                        ? (false, "Unknown peer, not connected")
                        : await _networkService.ReconnectAsync(reconnectId, cancellationToken);
                    _snapshotStore.PersistSnapshot(_fileManager, _networkService);
                    await writer.WriteLineAsync(
                        reconnectResult.Item1
                            ? $"SUCCESS|{reconnectResult.Item2}"
                            : $"ERROR|{reconnectResult.Item2}");
                    break;

                case "REMOVE_PEER":
                    if (parts.Length != 2)
                        throw new ArgumentException("Requires peer identifier.");

                    var removeId = _snapshotStore.GetRuntimePeerId(parts[1]);

                    if (!string.IsNullOrWhiteSpace(removeId))
                        await _networkService.RemoveAsync(removeId);

                    var peerRemoved = _snapshotStore.RemovePeer(parts[1]);

                    await writer.WriteLineAsync(
                        peerRemoved
                            ? "SUCCESS|Peer removed."
                            : "ERROR|Peer could not be removed.");

                    break;

                case "GET_CONFIG":
                    await writer.WriteLineAsync(
                        $"SUCCESS|{_config.Nickname}|{_config.MetaFileDirectory}|" +
                        $"{_config.FileDirectory}|{_config.MaxPeers}|{_config.Port}");
                    break;

                case "SAVE_CONFIG":
                    if (parts.Length != 6)
                        throw new ArgumentException("All configuration values are required.");

                    await SaveConfigurationAsync(parts, cancellationToken);
                    await writer.WriteLineAsync("SUCCESS|Configuration saved.");
                    break;

                default:
                    await writer.WriteLineAsync("ERROR|Unknown Command sent to Daemon.");
                    break;
            }
        }
        catch (Exception ex)
        {
            await writer.WriteLineAsync($"ERROR|{ex.Message}");
        }
    }


    // Saves the configuration settings received from the Web application and initializes the network and file manager accordingly.
    private async Task SaveConfigurationAsync(string[] parts, CancellationToken cancellationToken)
    {
        var nickname = parts[1].Trim();
        var metadataDirectory = parts[2].Trim();
        var fileDirectory = parts[3].Trim();
        var maxPeers = int.Parse(parts[4]);
        var port = int.Parse(parts[5]);

        if (string.IsNullOrWhiteSpace(nickname)
            || string.IsNullOrWhiteSpace(metadataDirectory)
            || string.IsNullOrWhiteSpace(fileDirectory))
        {
            throw new ArgumentException("Configuration rejected: all fields are required.");
        }
        if (maxPeers is < 1 or > 2048)
            throw new ArgumentException("max_peers within the configuration file is set to an invalid value");
        if (port is < 1 or > 65535)
            throw new ArgumentException("Port specified is either in use or invlaid");

        if (File.Exists(metadataDirectory))
            throw new IOException("Unable to create metafile_directory");
        if (File.Exists(fileDirectory))
            throw new IOException("Unable to create file_directory");

        Directory.CreateDirectory(metadataDirectory);
        Directory.CreateDirectory(fileDirectory);

        await _networkService.StartListeningAsync(port, nickname, maxPeers, cancellationToken);
        _fileManager.Initialize(metadataDirectory, fileDirectory);

        _config.Nickname = nickname;
        _config.MetaFileDirectory = metadataDirectory;
        _config.FileDirectory = fileDirectory;
        _config.MaxPeers = maxPeers;
        _config.Port = port;

        File.WriteAllLines(
            Path.Combine(AppContext.BaseDirectory, "config.conf"),
            [
                $"nickname={_config.Nickname}",
                $"metafile_directory={_config.MetaFileDirectory}",
                $"file_directory={_config.FileDirectory}",
                $"max_peers={_config.MaxPeers}",
                $"port={_config.Port}"
            ]);

        _snapshotStore.PersistSnapshot(_fileManager, _networkService);
    }
}
