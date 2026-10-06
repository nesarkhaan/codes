namespace FileValidate.Test;

using FileValidate.Core;
using FileValidate.Meta;

public class MerkleTreeValidationTests
{
    private void CheckHashEntries(string[] expected, List<string> actual) {
        if(expected.Length != actual.Count()) {
            Assert.Fail("The number of elements given does not match what is expected");
        }
               
        expected.Sort();
        actual.Sort();
        
        for(int i = 0; i < actual.Count(); i++) {
            string exp = expected[i];
            string act = actual[i];
            Assert.Equal(exp, act);
        }
    }

    private IMerkleTreeObject GetMerkleTree(string path) {
        
        var description = TideFileDescription.FromFilePath(path);
        var file = TideFile.WithMetaData(description);
        var merkleTree = MerkleTree.FromFile(file);

        return merkleTree;
    }

    [Fact]
    public void Test_AllHashes_Complete1()
    {
        var merkleTree = GetMerkleTree(MerkleTreeTestData.Complete1_Path);
        var allHashes = merkleTree.AllExpectedHashes();
        CheckHashEntries(MerkleTreeTestData.Complete1_ComputedHashes, allHashes);
        
        
    }

    [Fact]
    public void Test_AllHashes_Incomplete1()
    {
            
        var merkleTree = GetMerkleTree(MerkleTreeTestData.InComplete1_Path);
        var allHashes = merkleTree.AllExpectedHashes();
        CheckHashEntries(MerkleTreeTestData.InComplete1_ExpectedHashes, allHashes);
    }

    [Fact]
    public void Test_MinHashSet_Complete1()
    {
            
        var merkleTree = GetMerkleTree(MerkleTreeTestData.Complete1_Path);
        var hashes = merkleTree.MinimumSetOfHashes_RepresentingCompletion();
        CheckHashEntries(MerkleTreeTestData.Complete1_MinHashSet, hashes);
    }

    [Fact]
    public void Test_MinHashSet_Complete2()
    {
            
        var merkleTree = GetMerkleTree(MerkleTreeTestData.Complete2_Path);
        var hashes = merkleTree.MinimumSetOfHashes_RepresentingCompletion();
        CheckHashEntries(MerkleTreeTestData.Complete2_MinHashSet, hashes);
    }

    [Fact]
    public void Test_MinHashSet_Complete3()
    {
            
        var merkleTree = GetMerkleTree(MerkleTreeTestData.Complete3_Path);
        var hashes = merkleTree.MinimumSetOfHashes_RepresentingCompletion();
        CheckHashEntries(MerkleTreeTestData.Complete3_MinHashSet, hashes);
    }

    [Fact]
    public void Test_MinHashSet_InComplete1()
    {
            
        var merkleTree = GetMerkleTree(MerkleTreeTestData.InComplete1_Path);
        var hashes = merkleTree.MinimumSetOfHashes_RepresentingCompletion();
        CheckHashEntries(MerkleTreeTestData.InComplete1_MinHashSet, hashes);
    }

    [Fact]
    public void Test_MinHashSet_InComplete2()
    {
            
        var merkleTree = GetMerkleTree(MerkleTreeTestData.InComplete2_Path);
        var hashes = merkleTree.MinimumSetOfHashes_RepresentingCompletion();
        CheckHashEntries(MerkleTreeTestData.InComplete2_MinHashSet, hashes);
    }

    [Fact]
    public void Test_MinHashSet_InComplete3()
    {
            
        var merkleTree = GetMerkleTree(MerkleTreeTestData.InComplete3_Path);
        var hashes = merkleTree.MinimumSetOfHashes_RepresentingCompletion();
        CheckHashEntries(MerkleTreeTestData.InComplete3_MinHashSet, hashes);
    }

    [Fact]
    public void Test_AllHashes_Complete_LeftSubTree()
    {

        var merkleTree = GetMerkleTree(MerkleTreeTestData.Complete1_Path);
        var allHashes = merkleTree.GetLeftSubTree().AllExpectedHashes();
        CheckHashEntries(MerkleTreeTestData.Complete1_LeftHashes, allHashes);
    }

    [Fact]
    public void Test_AllHashes_Complete_LeftLeftSubTree1()
    {


        var merkleTree = GetMerkleTree(MerkleTreeTestData.Complete1_Path);
        var allHashes = merkleTree
            .GetLeftSubTree()
            .GetLeftSubTree().AllExpectedHashes();
        CheckHashEntries(MerkleTreeTestData.Complete1_LeftLeftHashes, allHashes);
    }

    [Fact]
    public void Test_AllHashes_Complete_RightSubTree1()
    {

        var merkleTree = GetMerkleTree(MerkleTreeTestData.Complete1_Path);
        var allHashes = merkleTree
            .GetRightSubTree().AllExpectedHashes();
        CheckHashEntries(MerkleTreeTestData.Complete1_RightHashes, allHashes);
    }
    
  
}
