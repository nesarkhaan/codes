namespace FileShare.Lib.Packaging;

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public static class MerkleTreeBuilder
{
    public static string ComputeSha256(string input)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    public static string CombineHashes(string left, string right)
    {
        return ComputeSha256(left + right);
    }

    public static (int NHashes, List<string> InternalHashes, string RootHash) BuildTree(List<string> chunkHashes)
    {
        if (chunkHashes == null || chunkHashes.Count == 0)
        {
            return (0, [], string.Empty);
        }

        if (chunkHashes.Count == 1)
        {
            return (0, [], chunkHashes[0]);
        }

        List<string> internalHashes = [];
        List<string> currentLayer = new List<string>(chunkHashes);

        while (currentLayer.Count > 1)
        {
            List<string> nextLayer = [];

            for (int i = 0; i < currentLayer.Count; i += 2)
            {
                string leftNode = currentLayer[i];
                string rightNode = (i + 1 < currentLayer.Count) ? currentLayer[i + 1] : currentLayer[i];

                string parentHash = CombineHashes(leftNode, rightNode);
                nextLayer.Add(parentHash);
                internalHashes.Add(parentHash);
            }

            currentLayer = nextLayer;
        }

        return (internalHashes.Count, internalHashes, currentLayer[0]);
    }
}