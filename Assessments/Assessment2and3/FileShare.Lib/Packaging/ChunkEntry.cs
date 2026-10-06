namespace FileShare.Lib.Packaging;

public record ChunkEntry(string Hash, long Offset, int Size);