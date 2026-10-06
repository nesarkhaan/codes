using System.Net;
using System.Net.Sockets;
using FileShare.Abstractions.Interfaces;
using FileShare.Abstractions.Models;
using FileShare.Lib;
using FileShare.Lib.Networking;

namespace FileShare.Test;

public class PackageAndPeerTests
{
    [Fact]
    public void AddPackage_RegistersPackageInManager()
    {
        string rootDirectory = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid():N}");

        string sourceDirectory = Path.Combine(rootDirectory, "source");
        string metadataDirectory = Path.Combine(rootDirectory, "meta");
        string fileDirectory = Path.Combine(rootDirectory, "files");
        string descriptorPath = Path.Combine(sourceDirectory, "sample.tpk");

        Directory.CreateDirectory(sourceDirectory);
        Directory.CreateDirectory(metadataDirectory);
        Directory.CreateDirectory(fileDirectory);
        File.WriteAllText(descriptorPath, "sample descriptor");

        var manager = new P2PFileManager(new FakeFileValidationAdapter());

        try
        {
            manager.Initialize(metadataDirectory, fileDirectory);

            bool added = manager.AddPackage(descriptorPath);

            Assert.True(added);

            var packages = manager.GetPackageDetails();
            Assert.Single(packages);
            Assert.Equal(FakeFileValidationAdapter.Identifier, packages[0].Identifier);
            Assert.Equal("sample.dat", packages[0].Filename);
        }
        finally
        {
            if (Directory.Exists(rootDirectory))
            {
                Directory.Delete(rootDirectory, recursive: true);
            }
        }
    }

    [Fact]
    public async Task StartListeningAndConnectAsync_CreatesPeerSession()
    {
        int port = GetFreeTcpPort();
        var manager = new P2PFileManager(new FakeFileValidationAdapter());
        var peerService = new PeerNetworkService(manager);

        try
        {
            await peerService.StartListeningAsync(port, "test-peer", 10);

            var connection = await peerService.ConnectAsync("127.0.0.1", port);

            Assert.True(connection.Success, connection.Message);
            Assert.False(string.IsNullOrWhiteSpace(connection.PeerId));

            var peers = peerService.GetPeers();
            Assert.NotEmpty(peers);
            Assert.Contains(peers, p => p.Nickname == "test-peer");
        }
        finally
        {
            await peerService.DisposeAsync();
        }
    }

    private static int GetFreeTcpPort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        return ((IPEndPoint)listener.LocalEndpoint).Port;
    }

    private sealed class FakeFileValidationAdapter : IFileValidationAdapter
    {
        public const string Identifier = "peer-package-1234567890";

        public MetadataFile Read(string filePath)
        {
            return new MetadataFile
            {
                Identifier = Identifier,
                Filename = "sample.dat",
                TargetPath = "sample.dat",
                Size = 8,
                ChunksCount = 2,
                Chunks =
                [
                    new ChunkMetadata(0, "hash-0", 0, 4),
                    new ChunkMetadata(1, "hash-1", 4, 4)
                ]
            };
        }

        public IManagedPackage OpenPackage(string descriptorPath, string dataDirectory)
        {
            return new FakeManagedPackage(Read(descriptorPath));
        }
    }

    private sealed class FakeManagedPackage : IManagedPackage
    {
        public FakeManagedPackage(MetadataFile metadata)
        {
            Metadata = metadata;
        }

        public MetadataFile Metadata { get; }

        public int CompletedBlocks => Metadata.ChunksCount;

        public IReadOnlyList<ChunkMetadata> IncompleteChunks => [];

        public bool TryGetChunk(int chunkIndex, out ChunkMetadata? chunk)
        {
            chunk = Metadata.Chunks.FirstOrDefault(c => c.Index == chunkIndex);
            return chunk is not null;
        }

        public byte[] ReadVerifiedChunk(int chunkIndex)
        {
            return chunkIndex switch
            {
                0 => new byte[] { 1, 2, 3, 4 },
                1 => new byte[] { 5, 6, 7, 8 },
                _ => []
            };
        }

        public bool WriteAndVerifyChunk(int chunkIndex, byte[] data)
        {
            return data.Length > 0;
        }
    }
}
