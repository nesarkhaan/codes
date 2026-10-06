namespace FileShare.Test;

using FileShare.Abstractions.Interfaces;
using FileShare.Abstractions.Models;
using FileShare.Lib;

/// <summary>
/// Confirms that the manager can use a replacement validation implementation.
/// No FileValidate types are used by this test.
/// </summary>
public class FileValidationAbstractionTests
{
    [Fact]
    // Test to ensure that the P2PFileManager uses the provided validation abstraction correctly.
    public void Manager_UsesValidationAbstraction()
    {
        string rootDirectory = Path.Combine(
            Path.GetTempPath(),
            Guid.NewGuid().ToString());

        string sourceDirectory =
            Path.Combine(rootDirectory, "source");
        string metadataDirectory =
            Path.Combine(rootDirectory, "metadata");
        string fileDirectory =
            Path.Combine(rootDirectory, "files");
        string descriptorPath =
            Path.Combine(sourceDirectory, "test.tpk");

        Directory.CreateDirectory(sourceDirectory);
        File.WriteAllText(descriptorPath, "test descriptor");

        var adapter = new FakeFileValidationAdapter();
        var manager = new P2PFileManager(adapter);

        try
        {
            manager.Initialize(
                metadataDirectory,
                fileDirectory);

            Assert.True(manager.AddPackage(descriptorPath));
            Assert.True(adapter.ReadWasCalled);
            Assert.True(adapter.OpenWasCalled);
            Assert.Equal(
                new byte[] { 1, 2, 3 },
                manager.GetChunkForPeer(
                    FakeFileValidationAdapter.Identifier,
                    0));
        }
        finally
        {
            if (Directory.Exists(rootDirectory))
            {
                Directory.Delete(rootDirectory, recursive: true);
            }
        }
    }
    
    private sealed class FakeFileValidationAdapter :
        IFileValidationAdapter
    {
        // Unique identifier for the fake package used in testing.
        public const string Identifier =
            "12345678901234567890-test-package";
        // Flags to track whether the Read and OpenPackage methods were called.
        public bool ReadWasCalled { get; private set; }
        // Flag to track whether the OpenPackage method was called.
        public bool OpenWasCalled { get; private set; }
        // Reads the metadata file from the specified file path and returns a MetadataFile object.
        public MetadataFile Read(string filePath)
        {
            ReadWasCalled = true;
            return CreateMetadata();
        }
        // Opens the package using the provided descriptor path and data directory, returning an IManagedPackage object.
        public IManagedPackage OpenPackage( string descriptorPath, string dataDirectory)
        {
            OpenWasCalled = true;
            return new FakeManagedPackage(CreateMetadata());
        }
        // Creates a MetadataFile object with predefined values for testing purposes.
        private static MetadataFile CreateMetadata()
        {
            return new MetadataFile
            {
                Identifier = Identifier,
                Filename = "test.dat",
                TargetPath = "test.dat",
                Size = 3,
                ChunksCount = 1,
                Chunks =
                [
                    new ChunkMetadata(
                        0,
                        "test-hash",
                        0,
                        3)
                ]
            };
        }
    }

    private sealed class FakeManagedPackage :
        IManagedPackage
    {
        public FakeManagedPackage(MetadataFile metadata)
        {
            Metadata = metadata;
        }

        public MetadataFile Metadata { get; }

        public int CompletedBlocks => 1;

        public IReadOnlyList<ChunkMetadata>
            IncompleteChunks => [];

        public bool TryGetChunk(int chunkIndex, out ChunkMetadata? chunk)
        {
            chunk = chunkIndex == 0
                ? Metadata.Chunks[0]
                : null;

            return chunk is not null;
        }

        public byte[] ReadVerifiedChunk(int chunkIndex)
        {
            return chunkIndex == 0
                ? [1, 2, 3]
                : [];
        }

        public bool WriteAndVerifyChunk(int chunkIndex, byte[] data)
        {
            return chunkIndex == 0
                && data.SequenceEqual(
                    new byte[] { 1, 2, 3 });
        }
    }
}
