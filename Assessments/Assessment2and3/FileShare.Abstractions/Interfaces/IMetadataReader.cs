using FileShare.Abstractions.Models;

namespace FileShare.Abstractions.Interfaces
{

    // Reads metadata from a file and returns a MetadataFile object.

    public interface IMetadataReader
    {   // Reads metadata from a file at the specified path and returns a MetadataFile object.
        MetadataFile Read(string filePath);
    }
}
