namespace FileShare.Abstractions.Models
{
    // Metadata for a file, including its chunks and hashes.
    public class MetadataFile
    {
        // Unique identifier for the file.
        public string Identifier { get; set; } = "";
        // Name of the file.
        public string Filename { get; set; } = "";
        // Target path where the file should be stored or retrieved.
        public string TargetPath { get; set; } = "";
        // Size of the file in bytes.
        public long Size { get; set; }
        // Number of hashes associated with the file.
        public int HashesCount { get; set; }
        // List of hashes for the file, used for integrity verification.
        public List<string> Hashes { get; set; } = new();
        // Number of chunks the file is divided into.
        public int ChunksCount { get; set; }
        //
        public List<ChunkMetadata> Chunks { get; set; } = new();
    }
}
