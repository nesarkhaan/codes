using FileShare.Abstractions.Models;

namespace FileShare.Abstractions.Interfaces;


// Represents a managed package that can read and write chunks of data based on metadata.

public interface IManagedPackage
{
    // The metadata associated with the package, including information about its chunks and hashes.
    MetadataFile Metadata { get; }
    // The number of chunks that have been successfully completed and verified.
    int CompletedBlocks { get; }
    // The total number of chunks in the package.
    IReadOnlyList<ChunkMetadata> IncompleteChunks { get; }
    // Tries to retrieve the metadata for a specific chunk index. Returns true if the chunk exists, false otherwise.
    bool TryGetChunk(int chunkIndex, out ChunkMetadata? chunk);
    // Reads a chunk of data from the package and verifies its integrity based on the associated metadata.
    byte[] ReadVerifiedChunk(int chunkIndex);
    // Writes a chunk of data to the package and verifies its integrity against the associated metadata. Returns true if the write and verification are successful, false otherwise.
    bool WriteAndVerifyChunk(int chunkIndex, byte[] data);
}
