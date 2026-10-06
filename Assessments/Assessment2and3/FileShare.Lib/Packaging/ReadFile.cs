namespace FileShare.Lib.Packaging;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;

public class ReadFile(string filePath)
{
    public string FilePath { get; } = filePath;

    public long GetFileSize() => File.Exists(FilePath) ? new FileInfo(FilePath).Length : 0;

    public byte[] Stream(int chunkSize, long offset)
    {
        if (!File.Exists(FilePath)) return [];

        using var fs = File.OpenRead(FilePath);
        if (offset >= fs.Length) return [];

        int actualChunkSize = (int)Math.Min(chunkSize, fs.Length - offset);
        fs.Seek(offset, SeekOrigin.Begin);

        byte[] buffer = new byte[actualChunkSize];
        fs.ReadExactly(buffer, 0, actualChunkSize);
        return buffer;
    }

    public static int CalculateChunkSize(long fileSize)
    {
        if (fileSize < 1024 * 1024) return 64 * 1024;

        double fileLog = Math.Log(fileSize / (1024.0 * 1024.0), 2);
        int targetChunkCount = (int)(32 + (fileLog * 20));
        long rawTarget = fileSize / Math.Max(1, targetChunkCount);

        int chunkSize = 1;
        while (chunkSize < rawTarget)
        {
            chunkSize <<= 1;
        }

        if (chunkSize > 16 * 1024 * 1024) return 16 * 1024 * 1024;

        return chunkSize;
    }

    public (List<ChunkEntry> Chunks, List<string> LeafHashes) GenerateChunksAndHashes()
    {
        long fileSize = GetFileSize();
        if (fileSize == 0) return ([], []);

        int chunkSize = CalculateChunkSize(fileSize);
        long numberOfChunks = (fileSize + chunkSize - 1) / chunkSize;

        var chunkEntries = new List<ChunkEntry>();
        var leafHashes = new List<string>();

        for (long i = 0; i < numberOfChunks; i++)
        {
            long startOffset = i * chunkSize;
            byte[] chunkBytes = Stream(chunkSize, startOffset);

            string hash = HashHelper.ComputeChunkHashOnBlock(chunkBytes);
            chunkEntries.Add(new ChunkEntry(hash, startOffset, chunkBytes.Length));
            leafHashes.Add(hash);
        }

        return (chunkEntries, leafHashes);
    }
}

public static class HashHelper
{
    public static string ComputeChunkHashOnBlock(byte[] bytes)
    {
        byte[] hashBytes = SHA256.HashData(bytes);
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }
}