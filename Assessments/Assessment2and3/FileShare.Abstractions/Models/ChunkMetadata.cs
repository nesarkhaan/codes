namespace FileShare.Abstractions.Models;


// Metadata for a single chunk of a file.

public sealed record ChunkMetadata(int Index, string Hash, long Offset, int Size);
