using FileValidate.Core;
using FileValidate.Meta;
using System.Security.Cryptography;
using System.Text;

namespace FileValidate.Test;

public class AdditionalTests
{
    private static string HashBytes(List<byte> bytes)
    {
        byte[] hashBytes = SHA256.HashData(bytes.ToArray());
        string hash = "";

        foreach (byte b in hashBytes)
        {
            hash += b.ToString("x2");
        }

        return hash;
    }

    private static string HashString(string text)
    {
        byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(text));
        string hash = "";

        foreach (byte b in hashBytes)
        {
            hash += b.ToString("x2");
        }

        return hash;
    }

    [Fact]
    public void Additional_MetadataCountMismatch_ThrowsFormatException()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".tpk");

        File.WriteAllText(path,
            @"ident: test
            filename: test.dat
            size: 4
            nhashes: 2
            hashes:
            abc
            nchunks: 1
            chunks:
            def,0,4");

        Assert.Throws<FormatException>(() => TideFileDescription.FromFilePath(path));
    }

    [Fact]
    public void Additional_WithMetaData_CreatesMissingFile()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".dat");

        TideFileDescription metadata = new TideFileDescription();
        metadata.Filename = path;
        metadata.Size = 8;

        TideFile.WithMetaData(metadata);

        Assert.True(File.Exists(path));
        Assert.Equal(8, new FileInfo(path).Length);
    }

    [Fact]
    public void Additional_WithMetaData_ResizesIncorrectFile()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".dat");

        File.WriteAllBytes(path, new byte[] { 1, 2, 3 });

        TideFileDescription metadata = new TideFileDescription();
        metadata.Filename = path;
        metadata.Size = 10;

        TideFile.WithMetaData(metadata);

        Assert.Equal(10, new FileInfo(path).Length);
    }

    [Fact]
    public void Additional_WriteDataToChunk_WritesAtCorrectOffset()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".dat");

        File.WriteAllBytes(path, new byte[8]);

        TideFileDescription metadata = new TideFileDescription();
        metadata.Filename = path;
        metadata.Size = 8;

        metadata.Chunks.Add(
            new TideChunkMetaData
            {
                ChunkIndex = 0,
                Hash = "",
                Offset = 0,
                Size = 4
            });

        metadata.Chunks.Add(
            new TideChunkMetaData
            {
                ChunkIndex = 1,
                Hash = "",
                Offset = 4,
                Size = 4
            });

        ITideFileFormat file = TideFile.WithMetaData(metadata)!;

        file.WriteDataToChunk(
            1,
            new List<byte> { 1, 2, 3, 4 });

        byte[] result = File.ReadAllBytes(path);

        Assert.Equal(
            new byte[] { 0, 0, 0, 0, 1, 2, 3, 4 },
            result);
    }

    [Fact]
    public void Additional_CompleteAndIncompleteBlocks_AreReported()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".dat");

        List<byte> chunk0 = new List<byte> { 1, 2, 3, 4 };
        List<byte> chunk1 = new List<byte> { 5, 6, 7, 8 };

        File.WriteAllBytes(
            path,
            chunk0.Concat(chunk1).ToArray());

        TideFileDescription metadata = new TideFileDescription();
        metadata.Filename = path;
        metadata.Size = 8;

        metadata.Chunks.Add(
            new TideChunkMetaData
            {
                ChunkIndex = 0,
                Hash = HashBytes(chunk0),
                Offset = 0,
                Size = 4
            });

        metadata.Chunks.Add(
            new TideChunkMetaData
            {
                ChunkIndex = 1,
                Hash = HashBytes(
                    new List<byte> { 9, 9, 9, 9 }),
                Offset = 4,
                Size = 4
            });

        ITideFileFormat file = TideFile.WithMetaData(metadata)!;

        Assert.True(file.IsIncomplete());
        Assert.False(file.IsComplete());
        Assert.Single(file.CompletedBlocks());
        Assert.Single(file.IncompletedBlocks());
    }

    [Fact]
    public void Additional_WriteDataFromHash_WritesSubtreeData()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".dat");

        File.WriteAllBytes(path, new byte[8]);

        List<byte> chunk0 = new List<byte> { 1, 2, 3, 4 };
        List<byte> chunk1 = new List<byte> { 5, 6, 7, 8 };

        string chunkHash0 = HashBytes(chunk0);
        string chunkHash1 = HashBytes(chunk1);
        string rootHash = HashString(chunkHash0 + chunkHash1);

        TideFileDescription metadata = new TideFileDescription();

        metadata.Filename = path;
        metadata.Size = 8;

        metadata.HashesCount = 1;
        metadata.Hashes.Add(rootHash);

        metadata.ChunksCount = 2;

        metadata.Chunks.Add(
            new TideChunkMetaData
            {
                ChunkIndex = 0,
                Hash = chunkHash0,
                Offset = 0,
                Size = 4
            });

        metadata.Chunks.Add(
            new TideChunkMetaData
            {
                ChunkIndex = 1,
                Hash = chunkHash1,
                Offset = 4,
                Size = 4
            });

        ITideFileFormat file = TideFile.WithMetaData(metadata)!;

        MerkleTree tree =
            (MerkleTree)MerkleTree.FromFile(file)!;

        tree.WriteDataFromHash(
            rootHash,
            chunk0.Concat(chunk1).ToList());

        byte[] result = File.ReadAllBytes(path);

        Assert.Equal(
            chunk0.Concat(chunk1).ToArray(),
            result);
    }

    [Fact]
    public void Additional_DataFromHash_ReturnsSubtreeData()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".dat");

        List<byte> chunk0 = new List<byte> { 1, 2, 3, 4 };
        List<byte> chunk1 = new List<byte> { 5, 6, 7, 8 };

        File.WriteAllBytes(path, chunk0.Concat(chunk1).ToArray());

        string chunkHash0 = HashBytes(chunk0);
        string chunkHash1 = HashBytes(chunk1);
        string rootHash = HashString(chunkHash0 + chunkHash1);

        TideFileDescription metadata = new TideFileDescription();
        metadata.Filename = path;
        metadata.Size = 8;
        metadata.HashesCount = 1;
        metadata.Hashes.Add(rootHash);
        metadata.ChunksCount = 2;
        metadata.Chunks.Add(new TideChunkMetaData { ChunkIndex = 0, Hash = chunkHash0, Offset = 0, Size = 4 });
        metadata.Chunks.Add(new TideChunkMetaData { ChunkIndex = 1, Hash = chunkHash1, Offset = 4, Size = 4 });

        ITideFileFormat file = TideFile.WithMetaData(metadata)!;
        IMerkleTreeObject tree = MerkleTree.FromFile(file)!;

        List<byte> data = tree.DataFromHash(rootHash);

        Assert.Equal(chunk0.Concat(chunk1).ToList(), data);
    }

    [Fact]
    public void Additional_WriteDataFromHash_WritesLeafData()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".dat");

        File.WriteAllBytes(path, new byte[8]);

        List<byte> chunk0 = new List<byte> { 1, 2, 3, 4 };
        List<byte> chunk1 = new List<byte> { 5, 6, 7, 8 };

        string chunkHash0 = HashBytes(chunk0);
        string chunkHash1 = HashBytes(chunk1);
        string rootHash = HashString(chunkHash0 + chunkHash1);

        TideFileDescription metadata = new TideFileDescription();
        metadata.Filename = path;
        metadata.Size = 8;
        metadata.HashesCount = 1;
        metadata.Hashes.Add(rootHash);
        metadata.ChunksCount = 2;
        metadata.Chunks.Add(new TideChunkMetaData { ChunkIndex = 0, Hash = chunkHash0, Offset = 0, Size = 4 });
        metadata.Chunks.Add(new TideChunkMetaData { ChunkIndex = 1, Hash = chunkHash1, Offset = 4, Size = 4 });

        ITideFileFormat file = TideFile.WithMetaData(metadata)!;
        MerkleTree tree = (MerkleTree)MerkleTree.FromFile(file)!;
        tree.WriteDataFromHash(chunkHash1, chunk1);

        byte[] result = File.ReadAllBytes(path);

        Assert.Equal(new byte[] { 0, 0, 0, 0, 5, 6, 7, 8 }, result);
    }

    [Fact]
    public void Additional_ChunkMetadata_ReportsComplete_WhenHashMatches()
    {
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid():N}.dat");

        try
        {
            List<byte> expectedData = new()
        {
            1, 2, 3, 4
        };

            File.WriteAllBytes(filePath, expectedData.ToArray());

            TideChunkMetaData chunk = new()
            {
                ChunkIndex = 0,
                Hash = HashBytes(expectedData),
                Offset = 0,
                Size = expectedData.Count
            };

            TideFileDescription metadata = new()
            {
                Filename = filePath,
                Size = expectedData.Count,
                HashesCount = 0,
                ChunksCount = 1
            };

            metadata.Chunks.Add(chunk);

            ITideFileFormat file = TideFile.WithMetaData(metadata)!;
                        
            Assert.True(chunk.ComputedHash());
                        
            Assert.True(chunk.IsComplete());
                        
            Assert.Single(file.CompletedBlocks());
            Assert.Empty(file.IncompletedBlocks());
            Assert.True(file.IsComplete());
            Assert.False(file.IsIncomplete());
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Fact]
    public void Additional_ChunkMetadata_UpdatesStatus_AfterCorrectDataIsWritten()
    {
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid():N}.dat");

        try
        {
            List<byte> incorrectData = new()
        {
            0, 0, 0, 0
        };

            List<byte> correctData = new()
        {
            5, 6, 7, 8
        };

            File.WriteAllBytes(filePath, incorrectData.ToArray());

            TideChunkMetaData chunk = new()
            {
                ChunkIndex = 0,
                Hash = HashBytes(correctData),
                Offset = 0,
                Size = correctData.Count
            };

            TideFileDescription metadata = new()
            {
                Filename = filePath,
                Size = correctData.Count,
                HashesCount = 0,
                ChunksCount = 1
            };

            metadata.Chunks.Add(chunk);

            ITideFileFormat file = TideFile.WithMetaData(metadata)!;

            
            Assert.True(chunk.ComputedHash());
            Assert.False(chunk.IsComplete());
            Assert.False(file.IsComplete());
            Assert.True(file.IsIncomplete());

            
            file.WriteDataToChunk(0, correctData);

            
            Assert.True(chunk.ComputedHash());
            Assert.True(chunk.IsComplete());
            Assert.True(file.IsComplete());
            Assert.False(file.IsIncomplete());

            Assert.Equal(correctData, file.GetChunkData(0));
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}