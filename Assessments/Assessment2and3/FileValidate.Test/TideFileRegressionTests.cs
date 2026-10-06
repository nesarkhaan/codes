using FileValidate.Core;
using FileValidate.Meta;
using System.Security.Cryptography;

namespace FileValidate.Test;

public class TideFileRegressionTests
{
    private static string HashBytes(IEnumerable<byte> bytes)
    {
        byte[] hashBytes = SHA256.HashData(bytes.ToArray());
        string hash = string.Empty;

        foreach (byte b in hashBytes)
        {
            hash += b.ToString("x2");
        }

        return hash;
    }

    [Fact]
    public void WithMetaData_CreatesMissingFile_AtExpectedSize()
    {
        string path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.dat");

        try
        {
            TideFileDescription metadata = new()
            {
                Filename = path,
                RelativeFilePath = path,
                Size = 12
            };

            ITideFileFormat? file = TideFile.WithMetaData(metadata);

            Assert.NotNull(file);
            Assert.True(File.Exists(path));
            Assert.Equal(12, new FileInfo(path).Length);
        }
        finally
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    [Fact]
    public void WithMetaData_ResizesExistingFile_ToMatchMetadata()
    {
        string path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.dat");

        try
        {
            File.WriteAllBytes(path, new byte[] { 1, 2, 3 });

            TideFileDescription metadata = new()
            {
                Filename = path,
                RelativeFilePath = path,
                Size = 8
            };

            ITideFileFormat? file = TideFile.WithMetaData(metadata);

            Assert.NotNull(file);
            Assert.Equal(8, new FileInfo(path).Length);
        }
        finally
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    [Fact]
    public void ChunkLifecycle_TracksCompletion_AndUpdatesAfterWrite()
    {
        string path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.dat");

        try
        {
            byte[] chunk0 = new byte[] { 1, 2, 3, 4 };
            byte[] chunk1 = new byte[] { 5, 6, 7, 8 };

            File.WriteAllBytes(path, chunk0.Concat(new byte[] { 9, 9, 9, 9 }).ToArray());

            TideFileDescription metadata = new()
            {
                Filename = path,
                RelativeFilePath = path,
                Size = 8
            };

            metadata.Chunks.Add(new TideChunkMetaData
            {
                ChunkIndex = 0,
                Hash = HashBytes(chunk0),
                Offset = 0,
                Size = 4
            });

            metadata.Chunks.Add(new TideChunkMetaData
            {
                ChunkIndex = 1,
                Hash = HashBytes(chunk1),
                Offset = 4,
                Size = 4
            });

            ITideFileFormat file = TideFile.WithMetaData(metadata)!;

            Assert.True(file.IsIncomplete());
            Assert.False(file.IsComplete());
            Assert.Single(file.CompletedBlocks());
            Assert.Single(file.IncompletedBlocks());

            file.WriteDataToChunk(1, chunk1.ToList());

            Assert.True(file.IsComplete());
            Assert.False(file.IsIncomplete());
            Assert.Empty(file.IncompletedBlocks());
            Assert.Equal(2, file.CompletedBlocks().Count);
            Assert.Equal(chunk0.Concat(chunk1).ToArray(), File.ReadAllBytes(path));
        }
        finally
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
