using System.Security.Cryptography;
using System.Text;

namespace MerkleTreeNode;

// Binary tree node class storing a hash and references to left/right children
public class Node
{
    public string Hash { get; set; }
    public Node Left { get; set; }
    public Node Right { get; set; }

    public Node(string hash, Node left = null, Node right = null)
    {
        Hash = hash;
        Left = left;
        Right = right;
    } 
}

// Utility class for hashing operations
public class Hashing
{
    public string HashString { get; set; }
    public List<string> Data { get; set; }
    public int Counter;

    public Hashing()
    {
        Data = new List<string>();
        Counter = 0;    
    }

    // Generates a SHA-256 hash string from input text
    public string GetHash(string stringdatatohash)
    {
        using (SHA256 CreateHash256 = SHA256.Create())
        {
            // Convert input text to byte array and compute hash
            byte[] bytes = CreateHash256.ComputeHash(Encoding.UTF8.GetBytes(stringdatatohash));

            // Format byte array into hexadecimal string
            StringBuilder HashString = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                HashString.Append(bytes[i].ToString("x2"));
            }
        
            return HashString.ToString();
        }
    }

    // Hashes string data, saves it to the list, and increments counter
    public void AddHash(string stringdatatohash)
    {
        Data.Add(GetHash(stringdatatohash));
        Counter++;
    }

    // Returns the internal list of stored hashes
    public List<string> GetList()
    {
        return Data;
    }

    // Prints all stored hashes with a 1-based index
    public void ListHashes()
    {
        int counter = 1;
        foreach (var item in Data)
        {
            Console.WriteLine(counter + ". " + item);
            counter++;
        }
    }

    // Concatenates two hashes and hashes the combined result
    public string CombineHashes(string firsthash, string secondhash)
    {
        string combinedHash = firsthash + secondhash;
        return GetHash(combinedHash);
    }
}

public class MerkleTree
{
    public Node Root { get; private set; } // Reference to the root node of the tree
    public int NumberOfBlocks { get; set; }

    public MerkleTree(int numberofblocks)
    {
        NumberOfBlocks = numberofblocks;
    }

    // Prints all node hashes in a given tree layer
    public void PrintLayerList(List<Node> data)
    {
        int count = 1;
        foreach (var item in data)
        {
            Console.WriteLine(count + ": " + item.Hash);
            count++;
        }
    }

    // Reads data line by line from a text file into a list of strings
    public static List<string> ReadFile(string path)
    {
        List<string> Chunks = new List<string>();
        string line = String.Empty;

        using (StreamReader reader = new StreamReader(path))
        {
            while ((line = reader.ReadLine()) != null)
            {
                // Split line content by space and take the first item
                string[] Parts = line.Split(' ');
                Chunks.Add(Parts[0]);                          
            }
        }

        return Chunks;
    }

    // Builds the tree upwards using Node objects until reaching the root
    public Node BuildTree(List<string> hashlist)
    {
        Hashing hashUtils = new Hashing();
        
        List<Node> HashesCurrentLayer = new List<Node>();
        
        // Wrap raw hash strings into leaf Node objects
        foreach (string h in hashlist)
        {
            HashesCurrentLayer.Add(new Node(h));
        }

        int round = 0;
        Console.WriteLine(HashesCurrentLayer.Count);

        // Keep combining layers until only 1 root node remains
        while (HashesCurrentLayer.Count > 1)
        {
            round++;
            Console.WriteLine($"----------------------------------------------------\n Round: " + round + "\n----------------------------------------------------");
        
            List<Node> NextLayer = new List<Node>();
           
            // Pair up adjacent nodes in the current layer
            for (int i = 0; i < HashesCurrentLayer.Count; i += 2)
            {
                Node leftnode = HashesCurrentLayer[i];

                // If node count is odd, duplicate the left node as the right node
                Node rightnode = (i + 1 < HashesCurrentLayer.Count) ? HashesCurrentLayer[i + 1] : HashesCurrentLayer[i];

                // DEBUG: Accessing .Hash property for printing
                Console.WriteLine(i + ": Left Node:  " + leftnode.Hash);
                Console.WriteLine(i + 1 + ": Right Node:  " + rightnode.Hash + "\n");
                
                // Combine child hashes to get parent hash
                string ParentHash = hashUtils.CombineHashes(leftnode.Hash, rightnode.Hash);
                Console.WriteLine("Combined Root: " + ParentHash + "\n");

                // Create a parent node pointing to left and right child nodes
                Node parentNode = new Node(ParentHash, leftnode, rightnode);
                NextLayer.Add(parentNode);
            }

            // Move up to process the parent layer
            HashesCurrentLayer = NextLayer;
            PrintLayerList(NextLayer);
        }

        // Assign and return the root node
        Root = HashesCurrentLayer[0];
        
        // FIXED: Accessing .Hash property for printing root
        Console.WriteLine("\n\nFinal Root:" + Root.Hash);
        return Root;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Hashing HashIt = new Hashing();
        MerkleTree tree = new MerkleTree(8);
        
        // Read raw data from file
        string FilePath = "./datanc2.txt";
        var AllHashes = MerkleTree.ReadFile(FilePath);
      
        Console.WriteLine("Hash List");

        // Build tree and obtain root Node
        Node rootNode = tree.BuildTree(AllHashes);
    }
}