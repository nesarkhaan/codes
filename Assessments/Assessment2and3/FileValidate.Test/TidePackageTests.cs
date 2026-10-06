namespace FileValidate.Test;

using FileValidate.Core;
using FileValidate.Meta;

public class ChunkTuple {
    
    public string Hash { get; set; }
    public int Offset { get; set; }
    public int Length { get; set; }

    public ChunkTuple(string hash, int offset, int size) {
        Hash = hash;
        Offset = offset;
        Length = size;
        
    }

    public static ChunkTuple From(string[] tuple) {
        var inst = new ChunkTuple(tuple[0], int.Parse(tuple[1]), int.Parse(tuple[2]));
        return inst;
    }
}

public class TidePackageMetaDataTests
{
    
    [Fact]
    public void Test_TidePackage_LoadTPK_File_1()
    {
        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteOne_Path);        
        Assert.NotNull(description);
        
    }

    
    [Fact]
    public void Test_TidePackage_LoadTPK_File_2()
    {
        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteTwo_Path);
        Assert.NotNull(description);

    }

    
    [Fact]
    public void Test_TidePackage_LoadTPK_File_GetIdent_1()
    {

        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteOne_Path);
        TestCaseUtil.TideFileCheckFields_Identifier(description, MetaDataTestData.CompleteOne_Identifier);
    }
        
    [Fact]
    public void Test_TidePackage_LoadTPK_File_GetIdent_2()
    {
        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteTwo_Path);
        TestCaseUtil.TideFileCheckFields_Identifier(description, MetaDataTestData.CompleteTwo_Identifier);

    }
        
    [Fact]
    public void Test_TidePackage_LoadTPK_File_GetFileName_1()
    {
        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteOne_Path);
        TestCaseUtil.TideFileCheckFields_Filename(description, MetaDataTestData.CompleteOne_FileName);

    }

    
    [Fact]
    public void Test_TidePackage_LoadTPK_File_GetFileName_2()
    {
        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteTwo_Path);
        TestCaseUtil.TideFileCheckFields_Filename(description, MetaDataTestData.CompleteTwo_FileName);

    }
    
    
    [Fact]
    public void Test_TidePackage_LoadTPK_File_GetSize_1()
    {

        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteOne_Path);
        TestCaseUtil.TideFileCheckFields_FileSize(description, MetaDataTestData.CompleteOne_Size);
    }
    
    [Fact]
    public void Test_TidePackage_LoadTPK_File_GetSize_2()
    {
        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteTwo_Path);
        TestCaseUtil.TideFileCheckFields_FileSize(description, MetaDataTestData.CompleteTwo_Size);

    }
    
    [Fact]
    public void Test_TidePackage_LoadTPK_File_NonLeafHashes_1()
    {

        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteOne_Path);
        TestCaseUtil.TideFileCheckFields_NumberOfHashes(description, MetaDataTestData.CompleteOne_NHashes);
        TestCaseUtil.TideFileCheckFields_Hashes(description, MetaDataTestData.CompleteOne_Hashes);
    }
    
    [Fact]
    public void Test_TidePackage_LoadTPK_File_NonLeafHashes_2()
    {

        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteTwo_Path);
        TestCaseUtil.TideFileCheckFields_NumberOfHashes(description, MetaDataTestData.CompleteTwo_NHashes);
        TestCaseUtil.TideFileCheckFields_Hashes(description, MetaDataTestData.CompleteTwo_Hashes);

    }
    
    [Fact]
    public void Test_TidePackage_LoadTPK_File_ChunkData_1()
    {
        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteOne_Path);
        TestCaseUtil.TideFileCheckFields_NumberOfChunks(description, MetaDataTestData.CompleteOne_NChunks);
        TestCaseUtil.TideFileCheckFields_Chunks(description, MetaDataTestData.CompleteOne_Chunks);

    }
    
    [Fact]
    public void Test_TidePackage_LoadTPK_File_ChunkData_2()
    {
        var description = TideFileDescription.FromFilePath(MetaDataTestData.CompleteTwo_Path);
        TestCaseUtil.TideFileCheckFields_NumberOfChunks(description, MetaDataTestData.CompleteTwo_NChunks);
        TestCaseUtil.TideFileCheckFields_Chunks(description, MetaDataTestData.CompleteTwo_Chunks);

    }
    
}
