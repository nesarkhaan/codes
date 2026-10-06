namespace FileExaminer.Models;

using FileExaminer.Abstractions;

using System.IO;

public class LocalFileObject : IFileObject
{
    public string Name { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public long Size { get; set; }
    public string MagicByte { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public DateTime LastAccessedAt { get; set; }
    public bool IsReadOnly { get; set; }
    
    // Concrete implementation of the interface's read-only runtime check
    public bool Exists => File.Exists(FilePath); 
}
