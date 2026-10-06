namespace FileValidate.Test;

using FileValidate.Core;
using FileValidate.Meta;

public class ChunkTestData {
    public string Hash { get; set; }
    public List<byte> Data { get; set; }

    public ChunkTestData(string hash, List<byte> data)
    {
        Hash = hash;
        Data = data;
    } 
}

public class TideChunkBytesAndHashValidation
{
    private static ChunkTestData ConstructChunkTestData(string Hash,
        byte[] bytes)
    {
        return new ChunkTestData(
            Hash, bytes.ToList()
        );
    }

        private void CheckChunkHash(ChunkTestData expected, ITideFileFormat actual,
        int chunkIndex)
    {
        Assert.Equal(expected.Hash, actual.ComputeChunkHash(chunkIndex));
    }

    private void CheckHashIsInvalid(ChunkTestData expected, ITideFileFormat actual,
        int chunkIndex)
    {
        Assert.NotEqual(expected.Hash, actual.ComputeChunkHash(chunkIndex));
    }

    [Fact]
    public void Test_Construct_CheckdHashOfIncomplete_Index_1()
    {
        ChunkTestData testData = ConstructChunkTestData(
            ChunkInComplete1TestData.ChunkHash_0,
            ChunkInComplete1TestData.ChunkData_0
        );
        var description = TideFileDescription.FromFilePath(MetaDataTestData.IncompleteOne_Path);
        var file = TideFile.WithMetaData(description);
        int chunkIndex = 0;

        CheckChunkHash(testData, file, chunkIndex);
    }

    [Fact]
    public void Test_Construct_CheckHashOfIncomplete_Index_2()
    {
        ChunkTestData testData = ConstructChunkTestData(
            ChunkInComplete1TestData.ChunkHash_1,
            ChunkInComplete1TestData.ChunkData_1
        );
        var description = TideFileDescription.FromFilePath(MetaDataTestData.IncompleteOne_Path);
        var file = TideFile.WithMetaData(description);
        int chunkIndex = 1;

        CheckChunkHash(testData, file, chunkIndex);
    }

    [Fact]
    public void Test_Construct_CheckInvalidHashOfIncomplete_Index_3()
    {
        ChunkTestData testData = ConstructChunkTestData(
            ChunkInComplete1TestData.ChunkHash_Expected_2,
            ChunkInComplete1TestData.ChunkData_2
        );
        var description = TideFileDescription.FromFilePath(MetaDataTestData.IncompleteOne_Path);
        var file = TideFile.WithMetaData(description);
        int chunkIndex = 2;

        CheckHashIsInvalid(testData, file, chunkIndex);
    }

    [Fact]
    public void Test_Construct_CheckInvalidHashOfIncomplete_Index_4()
    {
        ChunkTestData testData = ConstructChunkTestData(
            ChunkInComplete1TestData.ChunkHash_Expected_3,
            ChunkInComplete1TestData.ChunkData_3
        );
        var description = TideFileDescription.FromFilePath(MetaDataTestData.IncompleteOne_Path);
        var file = TideFile.WithMetaData(description);
        int chunkIndex = 3;

        CheckHashIsInvalid(testData, file, chunkIndex);
    }

    [Fact]
    public void Test_Construct_CheckHashOfIncomplete_Index_5()
    {
        ChunkTestData testData = ConstructChunkTestData(
            ChunkInComplete1TestData.ChunkHash_4,
            ChunkInComplete1TestData.ChunkData_4
        );
        var description = TideFileDescription.FromFilePath(MetaDataTestData.IncompleteOne_Path);
        var file = TideFile.WithMetaData(description);
        int chunkIndex = 4;

        CheckChunkHash(testData, file, chunkIndex);
    }


    [Fact]
    public void Test_Construct_CheckHash_Index_1()
    {
        ChunkTestData testData = ConstructChunkTestData(
            ChunkComplete1TestData.ChunkHash_0,
            ChunkComplete1TestData.ChunkData_0
        );
        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteOne_Path);
        var file = TideFile.WithMetaData(description);
        int chunkIndex = 0;

        CheckChunkHash(testData, file, chunkIndex);
    }

    [Fact]
    public void Test_Construct_CheckHash_Index_2()
    {

        ChunkTestData testData = ConstructChunkTestData(
            ChunkComplete1TestData.ChunkHash_1,
            ChunkComplete1TestData.ChunkData_1
        );
        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteOne_Path);
        var file = TideFile.WithMetaData(description);
        int chunkIndex = 1;

        CheckChunkHash(testData, file, chunkIndex);
    }

    [Fact]
    public void Test_Construct_CheckHash_Index_3()
    {

        ChunkTestData testData = ConstructChunkTestData(
            ChunkComplete1TestData.ChunkHash_2,
            ChunkComplete1TestData.ChunkData_2
        );
        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteOne_Path);
        var file = TideFile.WithMetaData(description);
        int chunkIndex = 2;

        CheckChunkHash(testData, file, chunkIndex);
    }

    [Fact]
    public void Test_Construct_CheckHash_Index_4()
    {
        ChunkTestData testData = ConstructChunkTestData(
            ChunkComplete1TestData.ChunkHash_3,
            ChunkComplete1TestData.ChunkData_3
        );
        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteOne_Path);
        var file = TideFile.WithMetaData(description);
        int chunkIndex = 3;

        CheckChunkHash(testData, file, chunkIndex);

    }

    [Fact]
    public void Test_Construct_CheckHash_Index_Last()
    {
        ChunkTestData testData = ConstructChunkTestData(
            ChunkComplete1TestData.ChunkHash_4,
            ChunkComplete1TestData.ChunkData_4
        );
        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteOne_Path);
        var file = TideFile.WithMetaData(description);
        int chunkIndex = 15;

        CheckChunkHash(testData, file, chunkIndex);

    }


    [Fact]
    public void Test_Construct_CheckBytes_1()
    {
        
        ChunkTestData testData = ConstructChunkTestData(
            ChunkComplete1TestData.ChunkHash_0,
            ChunkComplete1TestData.ChunkData_0
        );
        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteOne_Path);
        var file = TideFile.WithMetaData(description);
        int chunkIndex = 0;

        TestCaseUtil.TideFileChunk_CompareBytes(
            file,
            testData.Data,
            chunkIndex);
        
    }

    [Fact]
    public void Test_Construct_Check_Valid_2()
    {
        ChunkTestData testData = ConstructChunkTestData(
            ChunkComplete1TestData.ChunkHash_1,
            ChunkComplete1TestData.ChunkData_1
        );
        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteOne_Path);
        var file = TideFile.WithMetaData(description);
        int chunkIndex = 1;

        TestCaseUtil.TideFileChunk_CompareBytes(
            file,
            testData.Data,
            chunkIndex);

    }

    [Fact]
    public void Test_Construct_Check_Valid_3()
    {
        ChunkTestData testData = ConstructChunkTestData(
            ChunkComplete1TestData.ChunkHash_2,
            ChunkComplete1TestData.ChunkData_2
        );
        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteOne_Path);
        var file = TideFile.WithMetaData(description);
        int chunkIndex = 2;

        TestCaseUtil.TideFileChunk_CompareBytes(
            file,
            testData.Data,
            chunkIndex);

    }

    [Fact]
    public void Test_Construct_Check_Valid_4()
    {
        ChunkTestData testData = ConstructChunkTestData(
            ChunkComplete1TestData.ChunkHash_3,
            ChunkComplete1TestData.ChunkData_3
        );
        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteOne_Path);
        var file = TideFile.WithMetaData(description);
        int chunkIndex = 3;

        TestCaseUtil.TideFileChunk_CompareBytes(
            file,
            testData.Data,
            chunkIndex);

    }

    [Fact]
    public void Test_Construct_Check_Valid_Last()
    {
        ChunkTestData testData = ConstructChunkTestData(
            ChunkComplete1TestData.ChunkHash_4,
            ChunkComplete1TestData.ChunkData_4
        );
        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteOne_Path);
        var file = TideFile.WithMetaData(description);
        int chunkIndex = 15;

        TestCaseUtil.TideFileChunk_CompareBytes(
            file,
            testData.Data,
            chunkIndex);

    }
}
