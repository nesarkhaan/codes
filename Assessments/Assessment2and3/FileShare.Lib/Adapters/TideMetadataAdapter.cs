using FileShare.Abstractions.Interfaces;
using FileShare.Abstractions.Models;
using FileValidate.Core;
using FileValidate.Meta;

namespace FileShare.Lib.Adapters
{
    
    // Compatibility adapter between FileShare and FileValidate.
    // FileValidate types must not escape from this class.
    
    public sealed class TideMetadataAdapter : IFileValidationAdapter
    {   // Reads a .tpk metadata file and returns a MetadataFile object.
        public MetadataFile Read(string path)
        {
            TideFileDescription tide =
                TideFileDescription.FromFilePath(path);

            string descriptorDirectory =
                Path.GetDirectoryName(Path.GetFullPath(path))
                ?? string.Empty;

            string targetPath = Path.IsPathRooted(tide.Filename)
                ? Path.GetFullPath(tide.Filename)
                : Path.GetFullPath(
                    Path.Combine(
                        descriptorDirectory,
                        tide.Filename));

            return MapMetadata(tide, targetPath);
        }
        // Opens a .tpk metadata file and returns an IManagedPackage object.
        public IManagedPackage OpenPackage(string descriptorPath, string dataDirectory)
        {
            TideFileDescription tide =
                TideFileDescription.FromFilePath(descriptorPath);

            string targetPath = Path.Combine(
                Path.GetFullPath(dataDirectory),
                Path.GetFileName(tide.Filename));

            // FileValidate resolves its data file from this metadata value.
            tide.Filename = targetPath;

            ITideFileFormat tideFile =
                TideFile.WithMetaData(tide)
                ?? throw new InvalidDataException(
                    "FileValidate could not open the package.");

            return new TideManagedPackage(
                tideFile,
                MapMetadata(tide, targetPath));
        }
        // Maps FileValidate metadata to FileShare metadata.
        private static MetadataFile MapMetadata(
            ITideMetaData tide,
            string targetPath)
        {
            return new MetadataFile
            {
                Identifier = tide.Identifier,
                Filename = Path.GetFileName(tide.Filename),
                TargetPath = targetPath,
                Size = tide.Size,
                HashesCount = tide.HashesCount,
                Hashes = tide.Hashes.ToList(),
                ChunksCount = tide.ChunksCount,
                Chunks = tide.Chunks
                    .Select(MapChunk)
                    .ToList()
            };
        }
        // Maps FileValidate chunk metadata to FileShare chunk metadata.
        private static ChunkMetadata MapChunk(
            ITideChunkMetaData chunk)
        {
            return new ChunkMetadata(chunk.ChunkIndex, chunk.Hash, chunk.Offset, chunk.Size);
        }

        
        // Wraps one FileValidate file without exposing FileValidate types.
       
        private sealed class TideManagedPackage :
            IManagedPackage
        {
            // FileValidate file format instance.
            private readonly ITideFileFormat _tideFile;
            // Constructor for the TideManagedPackage class.
            public TideManagedPackage(ITideFileFormat tideFile, MetadataFile metadata)
            {
                _tideFile = tideFile;
                Metadata = metadata;
            }
            // Metadata for the managed package.
            public MetadataFile Metadata { get; }
            // Number of completed blocks in the managed package.
            public int CompletedBlocks => _tideFile.CompletedBlocks().Count;
            // Number of incomplete blocks in the managed package.
            public IReadOnlyList<ChunkMetadata> IncompleteChunks => _tideFile.IncompletedBlocks()
                    .Select(MapChunk)
                    .ToArray();
            // Tries to get chunk metadata for a specific chunk index.
            public bool TryGetChunk( int chunkIndex, out ChunkMetadata? chunk)
            {
                ITideChunkMetaData? tideChunk = _tideFile.GetChunkMetaData(chunkIndex);
                // If the chunk metadata is not found, return false and set chunk to null.
                if (tideChunk is null)
                {
                    chunk = null;
                    return false;
                }
                // If the chunk metadata is found, map it to ChunkMetadata and return true.
                chunk = MapChunk(tideChunk);
                return true;
            }
            // Reads a chunk of data and verifies its hash against the metadata.
            public byte[] ReadVerifiedChunk(int chunkIndex)
            {
                // Try to get the chunk metadata for the specified chunk index.
                if (!TryGetChunk(chunkIndex, out ChunkMetadata? chunk)
                    || chunk is null)
                {
                    return [];
                }
                // Read the chunk data from the tide file.
                byte[] bytes =
                    _tideFile.GetChunkData(chunkIndex).ToArray();

                // Compute the hash of the read data and compare it with the expected hash from the metadata.
                string computedHash =
                    _tideFile.ComputeChunkHashOnBlock(
                        bytes.ToList());

                // If the computed hash matches the expected hash, return the data; otherwise, return an empty array.
                return string.Equals(computedHash, chunk.Hash, StringComparison.OrdinalIgnoreCase) ? bytes : [];
            }
            // Writes a chunk of data and verifies its hash against the metadata.
            public bool WriteAndVerifyChunk( int chunkIndex, byte[] data)
            {
                // Check if the data is null and throw an exception if it is.
                ArgumentNullException.ThrowIfNull(data);

                // Try to get the chunk metadata for the specified chunk index.
                if (!TryGetChunk(chunkIndex, out ChunkMetadata? chunk)
                    || chunk is null
                    || data.Length != chunk.Size)
                {
                    return false;
                }
                // Compute the hash of the provided data and compare it with the expected hash from the metadata.
                string computedHash =_tideFile.ComputeChunkHashOnBlock(data.ToList());

                // If the computed hash does not match the expected hash, return false.
                if (!string.Equals(computedHash, chunk.Hash, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                // Write the data to the tide file and verify that the written data's hash matches the expected hash.
                _tideFile.WriteDataToChunk(chunkIndex, data.ToList());

                // Return true if the written data's hash matches the expected hash; otherwise, return false.
                return string.Equals(_tideFile.ComputeChunkHash(chunkIndex), chunk.Hash, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
