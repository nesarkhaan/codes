using FileShare.Lib;
using FileShare.Lib.Configuration;
using FileShare.Lib.Networking;
using Microsoft.Extensions.Hosting;

namespace FileShare.Service;

public class DaemonWorker : BackgroundService
{
    // These are the same objects registered in Program.cs, injected by the host.
    private readonly FileShareConfig _config;

    // The File Manager is the core of the P2P file sharing logic.
    private readonly P2PFileManager _fileManager;

    // The Peer Network Service handles the TCP network connections and peer management.
    private readonly PeerNetworkService _networkService;

    // The IPC server allows the Web UI to communicate with the Service.
    private readonly IpcServer _ipcServer;

    // The Snapshot Store handles persisting the current state of files and peers to the database.
    private readonly FileShareSnapshotStore _snapshotStore;

    // Dependency Injection hands us the same objects registered in Program.cs.
    public DaemonWorker(FileShareConfig config, P2PFileManager fileManager, PeerNetworkService networkService, IpcServer ipcServer, FileShareSnapshotStore snapshotStore)
    {
        _config = config;
        _fileManager = fileManager;
        _networkService = networkService;
        _ipcServer = ipcServer;
        _snapshotStore = snapshotStore;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            Console.WriteLine("Daemon: Initializing File Manager...");

            // Initialise the existing File Manager with the configured folders.
            _fileManager.Initialize(_config.MetaFileDirectory, _config.FileDirectory);
            Console.WriteLine("Daemon: File Manager initialized successfully.");

            // Service owns the database writes because it owns the live state.
            _snapshotStore.ApplyMigrations();
            _snapshotStore.PersistSnapshot(_fileManager, _networkService);

            // Start the existing TCP network listener.
            Console.WriteLine($"Daemon: Starting TCP Network on port {_config.Port}...");
            await _networkService.StartListeningAsync(
                _config.Port,
                _config.Nickname,
                _config.MaxPeers,
                stoppingToken);
            Console.WriteLine($"Daemon: Network is online. Listening as '{_config.Nickname}'.");

            // Keep the teammate's named-pipe server and the existing download loop
            // running together inside Service instead of inside Web.
            Console.WriteLine("Daemon: Starting IPC Pipe Server...");
            await Task.WhenAll(
                _ipcServer.StartListeningAsync(stoppingToken),
                DownloadLoopAsync(stoppingToken));
        }
        // Handle graceful shutdown on cancellation.
        catch (OperationCanceledException)
        {
            Console.WriteLine("\nDaemon: Shutting down safely...");
        }
        // Handle any unexpected exceptions and log them before exiting.
        catch (Exception ex)
        {
            Console.WriteLine($"\nDaemon FATAL ERROR: {ex.Message}");
            Environment.Exit(1);
        }
    }

    // This is the main loop that continuously checks for incomplete file chunks and attempts to download them from connected peers.
    private async Task DownloadLoopAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {

            try
            {
                var peers = _networkService.GetPeers()
                    .Where(peer => peer.IsConnected)
                    .ToArray();

                if (peers.Length > 0)
                {
                    foreach (var chunk in _fileManager.GetIncompleteChunks())
                    {
                        foreach (var peer in peers)
                        {
                            if (await _networkService.DownloadChunkAsync(peer.Id, chunk.Identifier, chunk.ChunkIndex, cancellationToken))
                            {
                                break;
                            }
                        }
                    }
                }

                _snapshotStore.PersistSnapshot(_fileManager, _networkService);
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Daemon download error: {ex.Message}");
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            }
        }
    }
}
