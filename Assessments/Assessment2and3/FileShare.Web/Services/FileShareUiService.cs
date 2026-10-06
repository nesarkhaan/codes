using System.Data.Common;
using FileShare.Lib.Data;
using FileShare.Lib.Networking;
using FileShare.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace FileShare.Web.Services;

/// <summary>
/// Keeps the existing Web facing methods while reading display data from the
/// shared database and sending changes to FileShare.Service.
/// </summary>
public sealed class FileShareUiService : IFileShareUiService
{
    // The database context factory for creating instances of the FileShareContext.
    private readonly IDbContextFactory<FileShareContext> _dbFactory;

    // The IPC client for communicating with the FileShare.Service.
    private readonly IpcClient _ipcClient;

    // Initializes a new instance of the FileShareUiService class.
    public FileShareUiService(
        IDbContextFactory<FileShareContext> dbFactory,
        IpcClient ipcClient)
    {
        _dbFactory = dbFactory;
        _ipcClient = ipcClient;
    }

    //  Retrieves the list of packages from the database and returns them as a read-only list of PackageItem objects.
    public IReadOnlyList<PackageItem> GetPackages()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            var peerNames = db.Peers
                .AsNoTracking()
                .Where(peer => peer.Connected)
                .Select(peer => peer.Nickname)
                .ToArray();

            return db.Files
                .AsNoTracking()
                .OrderBy(file => file.FileName)
                .ToArray()
                .Select(file => new PackageItem(file.Identifier, file.FileName, file.TargetPath, file.CompletedBlocks, file.TotalBlocks,
                file.Completed ? PackageState.Complete : PackageState.Incomplete, peerNames))
                .ToArray();
        }
        catch (Exception ex) when (ex is DbException or InvalidOperationException or IOException)
        {
            // Service creates the database. Before Service starts, show no rows.
            return [];
        }
    }

    // Adds a package to the service by sending the package file to the FileShare.Service via IPC.
    public async Task<OperationResult> AddPackageAsync(
        IFormFile? packageFile,
        CancellationToken cancellationToken)
    {
        if (packageFile is null || packageFile.Length == 0)
            return OperationResult.Failure("Missing file argument.");
        if (!string.Equals(Path.GetExtension(packageFile.FileName), ".tpk", StringComparison.OrdinalIgnoreCase))
            return OperationResult.Failure("Unable to parse tpk file.");

        var tempPath = Path.Combine(Path.GetTempPath(), $"fileshare-{Guid.NewGuid():N}.tpk");
        try
        {
            await using (var output = new FileStream(
                             tempPath,
                             FileMode.CreateNew,
                             FileAccess.Write,
                             System.IO.FileShare.None))
            {
                await packageFile.CopyToAsync(output, cancellationToken);
            }

            var response = await _ipcClient.SendAsync(
                $"ADD_PACKAGE|{tempPath}",
                cancellationToken);
            return ToOperationResult(response);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return OperationResult.Failure("Cannot open file");
        }
        catch (TimeoutException)
        {
            return ServiceUnavailable();
        }
        finally
        {
            try
            {
                if (File.Exists(tempPath))
                    File.Delete(tempPath);
            }
            catch
            {
                
            }
        }
    }

    // Removes a package from the service by sending the package identifier to the FileShare.Service via IPC.
    public OperationResult RemovePackage(string? identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier) || identifier.Length < 20)
        {
            return OperationResult.Failure(
                "Missing identifier argument, please specify whole 1024 character or at least 20characters.");
        }

        return SendCommand($"REMOVE_PACKAGE|{identifier}");
    }

    // Retrieves the list of peers from the database and returns them as a read-only list of PeerItem objects.
    public IReadOnlyList<PeerItem> GetPeers()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return db.Peers
                .AsNoTracking()
                .OrderBy(peer => peer.Nickname)
                .ToArray()
                .Select(peer => new PeerItem(
                    peer.Id.ToString(),
                    peer.Nickname,
                    peer.IPAddress,
                    peer.Port,
                    peer.Connected))
                .ToArray();
        }
        catch (Exception ex) when (ex is DbException or InvalidOperationException or IOException)
        {
            return [];
        }
    }

    // Connects to a peer by sending the IP address and port to the FileShare.Service via IPC.
    public OperationResult ConnectPeer(string? ipAddress, int? port)
    {
        if (string.IsNullOrWhiteSpace(ipAddress)
            || port is null or < 1 or > 65535
            || !System.Net.IPAddress.TryParse(ipAddress, out _))
        {
            return OperationResult.Failure("Missing address and port argument.");
        }

        return SendCommand($"CONNECT|{ipAddress}|{port}");
    }

    // Disconnects from a peer by sending the peer identifier to the FileShare.Service via IPC.
    public OperationResult DisconnectPeer(string? peerId)
    {
        if (string.IsNullOrWhiteSpace(peerId))
            return OperationResult.Failure("Missing address and port argument.");

        return SendCommand($"DISCONNECT|{peerId}");
    }

    // Reconnects to a peer by sending the peer identifier to the FileShare.Service via IPC.
    public OperationResult ReconnectPeer(string? peerId)
    {
        if (string.IsNullOrWhiteSpace(peerId))
            return OperationResult.Failure("Missing address and port argument.");

        return SendCommand($"RECONNECT|{peerId}");
    }

    // Sends a request to remove a peer based on its identifier.
    public OperationResult RemovePeer(string? peerId)
    {
        if (string.IsNullOrWhiteSpace(peerId))
            return OperationResult.Failure("Missing peer identifier.");

        return SendCommand($"REMOVE_PEER|{peerId}");
    }

    //  Retrieves the current configuration from the FileShare.Service via IPC and returns it as a FileShareConfiguration object.
    public FileShareConfiguration GetConfiguration()
    {
        try
        {
            var response = _ipcClient.SendAsync("GET_CONFIG").GetAwaiter().GetResult();
            var parts = response?.Split('|');
            if (parts is not { Length: 6 }
                || parts[0] != "SUCCESS"
                || !int.TryParse(parts[4], out var maxPeers)
                || !int.TryParse(parts[5], out var port))
            {
                return new FileShareConfiguration();
            }

            return new FileShareConfiguration
            {
                Nickname = parts[1],
                MetafileDirectory = parts[2],
                FileDirectory = parts[3],
                MaxPeers = maxPeers,
                Port = port
            };
        }
        catch (Exception ex) when (ex is TimeoutException or IOException)
        {
            return new FileShareConfiguration();
        }
    }

    // Saves the configuration to the FileShare.Service via IPC after validating the input.
    public OperationResult SaveConfiguration(FileShareConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(configuration.Nickname)
            || string.IsNullOrWhiteSpace(configuration.MetafileDirectory)
            || string.IsNullOrWhiteSpace(configuration.FileDirectory))
        {
            return OperationResult.Failure("Configuration rejected: all fields are required.");
        }
        if (configuration.MaxPeers is < 1 or > 2048)
            return OperationResult.Failure("max_peers within the configuration file is set to an invalid value");
        if (configuration.Port is < 1 or > 65535)
            return OperationResult.Failure("Port specified is either in use or invlaid");

        return SendCommand(
            $"SAVE_CONFIG|{configuration.Nickname}|{configuration.MetafileDirectory}|" +
            $"{configuration.FileDirectory}|{configuration.MaxPeers}|{configuration.Port}");
    }

    // Sends a command to the FileShare.Service via IPC and returns the result as an OperationResult.
    private OperationResult SendCommand(string command)
    {
        try
        {
            return ToOperationResult(_ipcClient.SendAsync(command).GetAwaiter().GetResult());
        }
        catch (Exception ex) when (ex is TimeoutException or IOException)
        {
            return ServiceUnavailable();
        }
    }

    //  Converts the response from the FileShare.Service into an OperationResult.
    private static OperationResult ToOperationResult(string? response)
    {
        if (string.IsNullOrWhiteSpace(response))
            return OperationResult.Failure("FileShare.Service returned no response.");

        var parts = response.Split('|', 2);
        var message = parts.Length == 2 ? parts[1] : parts[0];
        return parts[0] == "SUCCESS"
            ? OperationResult.Success(message)
            : OperationResult.Failure(message);
    }

    // Returns an OperationResult indicating that the FileShare.Service is not running.
    private static OperationResult ServiceUnavailable() =>
        OperationResult.Failure("FileShare.Service is not running.");
}
