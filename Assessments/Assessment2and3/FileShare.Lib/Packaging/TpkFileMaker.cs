namespace FileShare.Lib.Packaging;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

public class TpkFileMaker
{
    public string Ident { get; set; } = string.Empty;
    public string DatFilename { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public int NHashes { get; set; }
    public List<string> Hashes { get; set; } = [];
    public int NChunks { get; set; }
    public List<ChunkEntry> Chunks { get; set; } = [];
    public string FilePath { get; set; }
    public string GeneratedTpkPath { get; set; }

    public TpkFileMaker(string sourceFilePath, string outputTpkPath = "")
    {
        FilePath = sourceFilePath;
        GeneratedTpkPath = string.IsNullOrWhiteSpace(outputTpkPath)
            ? Path.ChangeExtension(sourceFilePath, ".tpk")
            : outputTpkPath;
    }

    public void Build()
    {
        if (!File.Exists(FilePath))
        {
            throw new FileNotFoundException("Source file does not exist.", FilePath);
        }

        var reader = new ReadFile(FilePath);
        FileSize = reader.GetFileSize();
        DatFilename = Path.GetFileName(FilePath);

        // 1. Slice file and generate chunk entries + leaf hashes
        var (chunkList, leafHashes) = reader.GenerateChunksAndHashes();
        Chunks = chunkList;
        NChunks = chunkList.Count;

        // 2. Compute non-leaf hashes and nhashes via MerkleTreeBuilder
        var (nHashes, internalHashes, _) = MerkleTreeBuilder.BuildTree(leafHashes);
        NHashes = nHashes;
        Hashes = internalHashes;

        // 3. Generate deterministic identifier
        Ident = GenerateIdent(FilePath, FileSize);

        // 4. Output .tpk metadata file
        WriteTpkFile();
    }

    private static string GenerateIdent(string path, long size)
    {
        using var stream = File.OpenRead(path);
        byte[] buffer = new byte[Math.Min(20, stream.Length)];
        stream.ReadExactly(buffer, 0, buffer.Length);

        string seed = $"{Convert.ToHexString(buffer).ToLowerInvariant()}_{size}";
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(seed));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private void WriteTpkFile()
    {
        using var writer = new StreamWriter(GeneratedTpkPath, false, Encoding.UTF8);
        writer.WriteLine($"ident:{Ident}");
        writer.WriteLine($"filename:{DatFilename}");
        writer.WriteLine($"size:{FileSize}");
        writer.WriteLine($"nhashes:{NHashes}");
        writer.WriteLine("hashes:");
        foreach (var hash in Hashes)
        {
            writer.WriteLine($"        {hash}");
        }
        writer.WriteLine($"nchunks:{NChunks}");
        writer.WriteLine("chunks:");
        foreach (var chunk in Chunks)
        {
            writer.WriteLine($"        {chunk.Hash},{chunk.Offset},{chunk.Size}");
        }
    }
}