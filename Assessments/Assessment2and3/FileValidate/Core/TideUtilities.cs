using System.Security.Cryptography;
using System.Text;

namespace FileValidate.Core;

public class HashHelper
{
    public static string ComputeHashSHA256(string hash)
    {
        StringBuilder HashString = new StringBuilder();

        using (SHA256 CreateHash256 = SHA256.Create())

        {
            byte[] bytes = CreateHash256.ComputeHash(Encoding.UTF8.GetBytes(hash));
            for (int i = 0; i < bytes.Length; i++)
            {
                HashString.Append(bytes[i].ToString("X2"));
            }
        }
        return HashString.ToString();

    }

    public static string GetCombinedHash(string left, string right)
    {
        return ComputeHashSHA256(left + right);
    }

}


