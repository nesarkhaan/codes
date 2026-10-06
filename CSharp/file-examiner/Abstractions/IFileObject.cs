namespace FileExaminer.Abstractions;

using System;

public interface IFileObject
{
    // Core Identity
    string Name { get; set; }
    string FilePath { get; set; }
    string Extension { get; set; }

    // Metadata & Content Identity
    long Size { get; set; }
    string MagicByte { get; set; }
    string Hash { get; set; }

    // System Timestamps
    DateTime CreatedAt { get; set; }
    DateTime ModifiedAt { get; set; }
    DateTime LastAccessedAt { get; set; }

    // Runtime State
    bool IsReadOnly { get; set; }
    bool Exists { get; } 
}
