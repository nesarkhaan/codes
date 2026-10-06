using System.Security.Cryptography;
using System.Text;

namespace MerkleTree;

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
            // Convert input string to bytes and compute hash
            byte[] bytes = CreateHash256.ComputeHash(Encoding.UTF8.GetBytes(stringdatatohash));

            // Convert byte array to hexadecimal string
            StringBuilder HashString = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                HashString.Append(bytes[i].ToString("x2"));
            }
        
            return HashString.ToString();
        }
    }

    // Adds a new hash to the data list and increments the counter
    public void AddHash(string stringdatatohash)
    {
        Data.Add(GetHash(stringdatatohash));
        Counter++;
    }

    // Returns the list of hashes
    public List<string> GetList()
    {
        return Data;
    }

    // Prints all stored hashes with numbered index
    public void ListHashes()
    {
        int counter = 1;
        foreach (var item in Data)
        {
            Console.WriteLine(counter + ". " + item);
            counter++;
        }
    }

    // Combines two hashes together and returns the new hash
    public string CombineHashes(string firsthash, string secondhash)
    {
        string combinedHash = firsthash + secondhash;
        return GetHash(combinedHash);
    }
}

public class MerkleTree
{
    public List<string> CurrentLayer;
    public int NumberOfBlocks { get; set; } // Total leaf blocks expected

    public MerkleTree(int numberofblocks)
    {
        NumberOfBlocks = numberofblocks;
    }

    // Prints all items in a given layer list
    public void PrintLayerList(List<string> data)
    {
        int count = 1;
        foreach (var item in data)
        {
            Console.WriteLine(count + ": " + item);
            count++;
        }
    }

    // Reads data lines from a text file
    public static List<string> ReadFile(string path)
    {
        List<string> Chunks = new List<string>();
        string line = String.Empty;

        using (StreamReader reader = new StreamReader(path))
        {
            while ((line = reader.ReadLine()) != null)
            {
                string[] Parts = line.Split("");
                Chunks.Add(Parts[0]);                          
            }
        }

        return Chunks;
    }

    // Combines hashes layer-by-layer until reaching a single root hash
    public List<string> BuildTree(List<string> hashlist, int startindex, int endindex)
    {
        Hashing hash = new Hashing();
        List<string> HashesCurrentLayer = new List<string>();
        
        HashesCurrentLayer = hashlist;
        int round = 0;

        Console.WriteLine(HashesCurrentLayer.Count);

        // Loop runs until only 1 root hash remains
        while (HashesCurrentLayer.Count > 1)
        {
            round++;
            Console.WriteLine($"----------------------------------------------------\n Round: " + round + "\n----------------------------------------------------");
        
            List<string> NextLayer = new List<string>();
           
            // Pair up adjacent nodes in the current layer
            for (int i = 0; i < HashesCurrentLayer.Count; i += 2)
            {
                string leftnode = HashesCurrentLayer[i];

                // If node count is odd, duplicate the last node on the right
                string rightnode = (i + 1 < HashesCurrentLayer.Count) ? HashesCurrentLayer[i + 1] : HashesCurrentLayer[i];

                Console.WriteLine(i + ": Left Node:  " + leftnode);
                Console.WriteLine(i + 1 + ": Right Node:  " + rightnode + "\n");
                
                // Combine left and right nodes into a parent hash
                string ParentHash = hash.CombineHashes(leftnode, rightnode);
                Console.WriteLine("Combined Root: " + ParentHash + "\n");

                NextLayer.Add(ParentHash);
            }

            // Move up to the next layer
            HashesCurrentLayer = NextLayer;
            PrintLayerList(NextLayer);
        }

        Console.WriteLine("\n\nFinal Root:" + HashesCurrentLayer[0]);
        return HashesCurrentLayer;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Hashing HashIt = new Hashing();
        MerkleTree tree = new MerkleTree(8);
        
        // Load data chunks from file
        string FilePath = "./datanc2.txt";
        var AllHashes = MerkleTree.ReadFile(FilePath);
      
        Console.WriteLine("Hash List");

        // Build Merkle Tree and print root
        tree.BuildTree(AllHashes, 0, 1);
    }
}