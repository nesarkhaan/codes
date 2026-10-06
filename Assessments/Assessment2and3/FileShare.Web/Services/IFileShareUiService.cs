using FileShare.Web.Models;

namespace FileShare.Web.Services;

// Represents a service for managing the UI of the FileShare application.
public interface IFileShareUiService
{
    // Retrieves a read-only list of packages currently managed by the application.
    IReadOnlyList<PackageItem> GetPackages();

    // Adds a new package to the application asynchronously.
    Task<OperationResult> AddPackageAsync(IFormFile? packageFile, CancellationToken cancellationToken);

    // Removes a package from the application based on its identifier.
    OperationResult RemovePackage(string? identifier);

    // Updates the details of an existing package in the application.
    IReadOnlyList<PeerItem> GetPeers();

    // Connects to a peer using the specified IP address and port.
    OperationResult ConnectPeer(string? ipAddress, int? port);

    // Disconnects from a peer based on its identifier.
    OperationResult DisconnectPeer(string? peerId);

    // Reconnects to a peer based on its identifier.
    OperationResult ReconnectPeer(string? peerId);

    // Removes a peer based on its identifier.
    OperationResult RemovePeer(string? peerId);


    // Retrieves the current configuration settings for the file sharing application.
    FileShareConfiguration GetConfiguration();

    // Saves the provided configuration settings for the file sharing application.
    OperationResult SaveConfiguration(FileShareConfiguration configuration);
}
