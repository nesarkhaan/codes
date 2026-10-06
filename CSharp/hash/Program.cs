namespace hash;


using System.Text;
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {


        string source = "Hello World!";
        byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(source));

        // Convert byte array to a hex string
        string hash = Convert.ToHexString(hashBytes);
        Console.WriteLine(hash);
    }
}
