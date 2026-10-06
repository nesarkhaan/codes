using FileShare.Lib;
using FileShare.Lib.Data;
using FileShare.Lib.Networking;
using Microsoft.EntityFrameworkCore;

namespace FileShare.Service;

// This is the original Web PersistSnapshot logic moved to Service, which owns
// the live P2P state and therefore owns database writes.
public sealed class FileShareSnapshotStore
{
    // Synchronization object to ensure thread safety when accessing the database.
    private readonly object _sync = new();

    // The database context factory for creating instances of the FileShareContext.
    private readonly IDbContextFactory<FileShareContext> _dbFactory;

    // Initializes a new instance of the FileShareSnapshotStore class with the specified database context factory.
    public FileShareSnapshotStore(IDbContextFactory<FileShareContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    //  Applies any pending migrations to the database to ensure it is up to date with the current schema.
    public void ApplyMigrations()
    {
        using var db = _dbFactory.CreateDbContext();
        db.Database.Migrate();
    }

    //  Persists the current state of the file manager and network service to the database.
    public void PersistSnapshot(P2PFileManager fileManager, PeerNetworkService networkService)
    {
        try
        {
            lock (_sync)
            {
                using var db = _dbFactory.CreateDbContext();
                var now = DateTime.UtcNow;
                var packageDetails = fileManager.GetPackageDetails();
                var managedIds = packageDetails
                    .Select(package => package.Identifier)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                foreach (var stale in db.Files.ToArray().Where(file => !managedIds.Contains(file.Identifier)))
                    db.Files.Remove(stale);

                foreach (var package in packageDetails)
                {
                    var entity = db.Files.FirstOrDefault(file => file.Identifier == package.Identifier);
                    if (entity is null)
                    {
                        entity = new FileEntity
                        {
                            Identifier = package.Identifier,
                            AddedDate = now
                        };
                        db.Files.Add(entity);
                    }

                    entity.FileName = package.Filename;
                    entity.TargetPath = package.TargetPath;
                    entity.FileSize = package.Size;
                    entity.CompletedBlocks = package.CompletedBlocks;
                    entity.TotalBlocks = package.TotalBlocks;
                    entity.Completed = package.IsComplete;
                }

                var peerDetails = networkService.GetPeers();
                var knownPeerAddresses = peerDetails
                    .Select(peer => $"{peer.IpAddress}:{peer.Port}")
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                // Peers retained from an earlier Service run are history, not live connections.
                foreach (var stale in db.Peers.ToArray().Where(peer =>
                             !knownPeerAddresses.Contains($"{peer.IPAddress}:{peer.Port}")))
                {
                    stale.Connected = false;
                }

                foreach (var peer in peerDetails)
                {
                    var entity = db.Peers.FirstOrDefault(
                        existing => existing.IPAddress == peer.IpAddress && existing.Port == peer.Port);
                    if (entity is null)
                    {
                        entity = new PeerEntity
                        {
                            IPAddress = peer.IpAddress,
                            Port = peer.Port
                        };
                        db.Peers.Add(entity);
                    }

                    entity.RuntimeId = peer.Id;
                    entity.Nickname = peer.Nickname;
                    entity.Connected = peer.IsConnected;
                    entity.LastSeen = now;
                }

                db.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            // Keep the daemon running if the display snapshot cannot be updated.
            Console.Error.WriteLine($"Unable to persist FileShare state: {ex.Message}");
        }
    }

    // Retrieves the runtime peer ID corresponding to the given peer ID from the database.
    public string? GetRuntimePeerId(string peerId)
    {
        if (!int.TryParse(peerId, out var databaseId))
            return peerId;

        lock (_sync)
        {
            using var db = _dbFactory.CreateDbContext();
            return db.Peers
                .AsNoTracking()
                .Where(peer => peer.Id == databaseId)
                .Select(peer => peer.RuntimeId)
                .FirstOrDefault();
        }
    }

    // Removes a peer from the database based on its identifier.
    public bool RemovePeer(string peerId)
    {
        if (!int.TryParse(peerId, out var databaseId))
            return false;

        lock (_sync)
        {
            using var db = _dbFactory.CreateDbContext();

            var peer = db.Peers.FirstOrDefault(p => p.Id == databaseId);

            if (peer is null)
                return false;

            db.Peers.Remove(peer);
            db.SaveChanges();

            return true;
        }
    }
}
