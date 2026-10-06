using Xunit;
using MerkleTree;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.IO.Pipelines;
using System.Reflection; // (Just in case you need it for File operations later too)

namespace MerkleTree.Tests;

public class ReadTextFile
{
    public List<string> TextList;
    public string FileName;

    public ReadTextFile(string filename) {
        TextList = new List<string>();
        FileName = filename;
        ReadFile();
}
    public List<string> ReadFile()
    {
        using (StreamReader sr = File.OpenText(FileName))
        {
            string line = string.Empty;

            while ((line = sr.ReadLine()) != null)
            {
                string clearString = line;
                TextList.Add(clearString);
            }
        }
        return TextList;
    }
}


public class CreateHashFromString
{
    ReadTextFile StringList;
    public List<string> HashList;
    public List<string> Data;
    public CreateHashFromString(string filename)
    {
        StringList = new ReadTextFile(filename);

        HashList = new List<string>();
        Data = StringList.TextList;
        HashList = CreateHash();    

    }

    public List<string> CreateHash()
    {
        using (SHA256 sha256create = SHA256.Create())
        {
            foreach(var item in Data)
            {
                byte[] bytes = sha256create.ComputeHash(Encoding.UTF8.GetBytes(item));
                StringBuilder builder = new StringBuilder();
                    foreach (byte b in bytes)
                    {
                    builder.Append(b.ToString("x2"));
                    }
                HashList.Add(builder.ToString());
            }

        }
        return HashList;
    }

    public string CreateHash(string textstring)
    {
        string result = string.Empty;
        using (SHA256 sha256create = SHA256.Create())
        { 
                byte[] bytes = sha256create.ComputeHash(Encoding.UTF8.GetBytes(textstring));
                StringBuilder builder = new StringBuilder();
                    foreach (byte b in bytes)
                    {
                    builder.Append(b.ToString("x2"));
                    }
                    result = builder.ToString();
        }
        
        return result;
    }
}





public class HashingTests
{
    string filename = "./String.txt";
    //Opens a Text File and writes it to a List 
    
    CreateHashFromString TestData;
     Random random = new Random();
     int RandomString;
     string ExpectedHash;
     string ActualHash;

     string NotExpectedHash;
    
    
    public HashingTests(){
    TestData = new CreateHashFromString(filename);
    Hashing hasher = new Hashing();
    this.RandomString = random.Next(0, TestData.Data.Count);
    this.ExpectedHash = TestData.HashList[RandomString];
    this.NotExpectedHash = TestData.CreateHash(TestData.HashList[RandomString]+"changestring");
    this.ActualHash = hasher.GetHash(TestData.Data[RandomString]);

    }
  



    [Fact]
    public void Test_hash_Length()
    {
        Assert.Equal(64, ActualHash.Length);
        Console.WriteLine("64" + ActualHash.Length);
    }
    [Fact]
    public void GetHash_1_string()
    {
        Assert.Equal(ExpectedHash, ActualHash);
    }

    [Fact]
    public void GetHash_wrong_hash()
    {
        Assert.NotEqual(NotExpectedHash, ActualHash);
    }



}
