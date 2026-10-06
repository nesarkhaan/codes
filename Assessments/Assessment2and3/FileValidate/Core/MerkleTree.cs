
namespace FileValidate.Core;

using System.Security.Cryptography;
using System.Text;


public class MerkleTree : IMerkleTreeObject
{

    private ITideFileFormat? file;
    private string hash;
    private int chunkIndex;
    private MerkleTree? left;
    private MerkleTree? right;

    public MerkleTree()
    {
        file = null;
        hash = "";
        chunkIndex = -1;
        left = null;
        right = null;
    }

    // <summary>
    // Creates a merkle tree object from the file and metadata associated
    // with the file
    // </summary>
    public static IMerkleTreeObject? FromFile(ITideFileFormat file)
    {
        var metadata = file.GetMetaData();

        if (metadata == null)
        {
            return null;
        }

        return BuildTree(file, 0);
    }

    private static MerkleTree? BuildTree(ITideFileFormat file, int index)
    {
        var metadata = file.GetMetaData();

        if (metadata == null)
        {
            return null;
        }

        int firstLeafIndex = metadata.Hashes.Count;

        if (index < firstLeafIndex)
        {
            MerkleTree tree = new MerkleTree();
            tree.file = file;
            tree.hash = metadata.Hashes[index];
            tree.left = BuildTree(file, (index * 2) + 1);
            tree.right = BuildTree(file, (index * 2) + 2);
            return tree;
        }

        int chunkIndex = index - firstLeafIndex;

        if (chunkIndex >= 0 && chunkIndex < metadata.Chunks.Count)
        {
            MerkleTree tree = new MerkleTree();
            tree.file = file;
            tree.hash = metadata.Chunks[chunkIndex].Hash;
            tree.chunkIndex = chunkIndex;
            return tree;
        }

        return null;
    }


    // <summary>
    // Gets the left subtree from the root of the tree
    // If it is a leaf node, it should return null
    // </summary>
    public IMerkleTreeObject? GetLeftSubTree()
    {
        return left;
    }


    // <summary>
    // Gets the right subtree from the root of the tree
    // If it is a leaf node, it should return null
    // </summary>
    
    // Gets the right subtree from the root of the tree
    // If it is a leaf node, it should return null
   
    public IMerkleTreeObject? GetRightSubTree()
    {
        return right;
    }

    // <summary>
    // Lists all the expected hashes (not computed)
    // that are within the file.
    // </summary>
    
    // Lists all the expected hashes (not computed)
    // that are within the file.
    
    public List<string> AllExpectedHashes()
    {
        List<string> hashes = new List<string>();

        if (chunkIndex == -1)
        {
            hashes.Add(hash);
        }

        if (left != null)
        {
            hashes.AddRange(left.AllExpectedHashes());
        }

        if (right != null)
        {
            hashes.AddRange(right.AllExpectedHashes());
        }

        return hashes;
    }

    // <summary>
    // The list of hashes represented as strings that should represent
    // the completion state.
    //
    // 
    // Examples using a file with 4 chunks and 7 nodes (3 non-leaf nodes):
    //   1. If the file is complete, the root hash is return
    //   2. If the file's first two chunks are complete, the root's left child is returned
    //   3. If we have the first and last chunk complete, the first and last chunk hashes would be returned
    //   4. If the last half of the file is completed, the has of the root's right child is returned.
    // </summary>
    public List<string> MinimumSetOfHashes_RepresentingCompletion()
    {
        List<string> hashes = new List<string>();

        if (IsCompleteSubTree())
        {
            hashes.Add(hash);
            return hashes;
        }

        if (left != null)
        {
            hashes.AddRange(left.MinimumSetOfHashes_RepresentingCompletion());
        }

        if (right != null)
        {
            hashes.AddRange(right.MinimumSetOfHashes_RepresentingCompletion());
        }

        return hashes;
    }


    // <summary>
    // Retrieves the data that corresponds to the hash specified.
    //   1. If the hash corresponds to a leaf node, this will simply return the data related to that node
    //   2. If the hash corresponds to a non-leaf node,
    //          this will return the data of all leaf nodes within that subtree
    // 
    // </summary>
    public List<byte> DataFromHash(string hash)
    {

        if (this.hash == hash)
        {
            return DataInSubTree();
        }

        if (left != null)
        {
            List<byte> data = left.DataFromHash(hash);

            if (data.Count > 0)
            {
                return data;
            }
        }

        if (right != null)
        {
            List<byte> data = right.DataFromHash(hash);

            if (data.Count > 0)
            {
                return data;
            }
        }

        return new List<byte>();
    }

    // <summary>
    // Writes bytes to the subtree that corresponds to the hash specified.
    // If the hash belongs to a leaf, it writes to one chunk.
    // If the hash belongs to a non-leaf node, it writes across all chunks in that subtree.
    // </summary>
    public void WriteDataFromHash(string hash, List<byte> data)
    {
        if (this.hash == hash)
        {
            int dataIndex = 0;
            WriteDataInSubTree(data, ref dataIndex);
            return;
        }

        if (left != null)
        {
            left.WriteDataFromHash(hash, data);
        }

        if (right != null)
        {
            right.WriteDataFromHash(hash, data);
        }
    }

    private bool IsCompleteSubTree()
    {
        if (file == null)
        {
            return false;
        }

        if (chunkIndex >= 0)
        {
            return file.ComputeChunkHash(chunkIndex) == hash;
        }

        if (left == null || right == null)
        {
            return false;
        }

        bool leftComplete = left.IsCompleteSubTree();
        bool rightComplete = right.IsCompleteSubTree();

        return leftComplete &&
               rightComplete &&
               ComputeSubTreeHash() == hash;
    }

    private string ComputeSubTreeHash()
    {
        if (file == null)
        {
            return "";
        }

        if (chunkIndex >= 0)
        {
            return file.ComputeChunkHash(chunkIndex);
        }

        if (left == null || right == null)
        {
            return "";
        }

        string combinedHash = left.ComputeSubTreeHash() + right.ComputeSubTreeHash();
        byte[] bytes = Encoding.UTF8.GetBytes(combinedHash);
        byte[] hashBytes = SHA256.HashData(bytes);

        string computedHash = "";

        foreach (byte b in hashBytes)
        {
            computedHash += b.ToString("x2");
        }

        return computedHash;
    }

    private List<byte> DataInSubTree()
    {
        List<byte> data = new List<byte>();

        if (file == null)
        {
            return data;
        }

        if (chunkIndex >= 0)
        {
            return file.GetChunkData(chunkIndex);
        }

        if (left != null)
        {
            data.AddRange(left.DataInSubTree());
        }

        if (right != null)
        {
            data.AddRange(right.DataInSubTree());
        }

        return data;
    }

    private void WriteDataInSubTree(List<byte> data, ref int dataIndex)
    {
        if (file == null)
        {
            return;
        }

        if (chunkIndex >= 0)
        {
            var chunk = file.GetChunkMetaData(chunkIndex);

            if (chunk == null)
            {
                return;
            }

            List<byte> chunkData = new List<byte>();

            for (int i = 0; i < chunk.Size && dataIndex < data.Count; i++)
            {
                chunkData.Add(data[dataIndex]);
                dataIndex++;
            }

            file.WriteDataToChunk(chunkIndex, chunkData);
            return;
        }

        if (left != null)
        {
            left.WriteDataInSubTree(data, ref dataIndex);
        }

        if (right != null)
        {
            right.WriteDataInSubTree(data, ref dataIndex);
        }
    }


}