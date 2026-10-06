namespace FileShare.Lib.Data;

// Moved from FileShare.Web so Service can write the same database that Web reads.
public class FileEntity
{
    // Unique identifier for the file entity in the database.
    public int Id { get; set; }
    // Unique identifier for the file, used for tracking and retrieval.
    public string Identifier { get; set; } = "";
    // Name of the file.
    public string FileName { get; set; } = "";
    // Target path where the file should be stored or retrieved.
    public string TargetPath { get; set; } = "";
    // Size of the file in bytes.
    public long FileSize { get; set; }
    // Number of blocks that have been completed for the file.
    public int CompletedBlocks { get; set; }
    // Total number of blocks that the file is divided into.
    public int TotalBlocks { get; set; }
    // Indicates whether the file transfer or processing is completed.
    public bool Completed { get; set; }
    // Date and time when the file entity was added to the database.
    public DateTime AddedDate { get; set; }
}
