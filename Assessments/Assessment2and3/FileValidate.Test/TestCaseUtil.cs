namespace FileValidate.Test;

using FileValidate.Core;
using FileValidate.Meta;

public class TestCaseUtil
{
    public static TideFileDescription LoadMetaData(string path) {
        var description = TideFileDescription.FromFilePath(path);
        return description;
    }

    public static void TideFile_ActionCheckFromLoad(string path,
        Action<ITideFileFormat> action) {
        var description = LoadMetaData(path);
        var tfile = TideFile.WithMetaData(description);
        TideFile_ActionCheck(tfile, action);
    }

    public static void TideFile_ActionCheck(ITideFileFormat tfile,
        Action<ITideFileFormat> action) {
        action(tfile);
    }

    public static void TideFileChunk_CompareBytes(ITideFileFormat tfile,
        List<byte> expected, int index)
    {
        var chunk = tfile.GetChunkData(index);
        if(expected.Count() != chunk.Count())
        {
            Assert.Fail("The length of the lists is incorrect");
        }
        for(int i = 0; i < expected.Count(); i++) {
            var eb = expected[i];
            var ab = chunk[i];
            if(eb != ab)
            {
                Assert.Fail($"Bytes at index: {index} do not match");
            }
        }

    }

    public static void TideFileCheckFields_Identifier(ITideMetaData meta, string ident) {
        Assert.Equal(ident, meta.Identifier);
    }

    public static void TideFileCheckFields_Filename(ITideMetaData meta, string filename) {
        Assert.Equal(filename, meta.Filename);
    }

    public static void TideFileCheckFields_FileSize(ITideMetaData meta, int fsize) {
        Assert.Equal(fsize, meta.Size);
    }

    public static void TideFileCheckFields_NumberOfHashes(ITideMetaData meta, int nhashes) {
        Assert.Equal(nhashes, meta.HashesCount);
    }

    public static void TideFileCheckFields_NumberOfChunks(ITideMetaData meta, int nchunks) {
        Assert.Equal(nchunks, meta.ChunksCount);
    }

    public static void TideFileCheckFields_Hashes(ITideMetaData meta, string[] hashes) {
        Assert.Equal(hashes.Length, meta.Hashes.Count());

        for(int i = 0; i < hashes.Length; i++) {
            var hash = hashes[i];
            var actualHash = meta.Hashes[i];

            Assert.Equal(hash, actualHash);
        }
    }

    public static void TideFileCheckFields_Chunks(ITideMetaData meta, string[][] chunks) {
        Assert.Equal(chunks.Length, meta.Chunks.Count());
        for(int i = 0; i < chunks.Length; i++) {
            var chunk = ChunkTuple.From(chunks[i]);
            var actualChunk = meta.Chunks[i];

            Assert.Equal(chunk.Hash, actualChunk.Hash);
            Assert.Equal(chunk.Offset, actualChunk.Offset);
            Assert.Equal(chunk.Length, actualChunk.Size);
        }
    }

}
