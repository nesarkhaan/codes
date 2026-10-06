namespace FileShare.Web.Models;
// Represents the state of a package, either incomplete or complete.
public enum PackageState
{
    Incomplete,
    Complete
}
// Represents a package item with its metadata and state.
public sealed record PackageItem(string Identifier, string FileName, string TargetPath, int CompletedBlocks, int TotalBlocks, PackageState State,
    IReadOnlyList<string> Peers)
{
    //  Calculates the progress percentage of the package based on completed and total blocks.
    public int ProgressPercentage => TotalBlocks <= 0
        ? 0
        : (int)Math.Round(CompletedBlocks * 100d / TotalBlocks);
    // Returns a shortened version of the identifier if it exceeds 32 characters, appending "..." to indicate truncation.
    public string ShortIdentifier => Identifier.Length <= 32
        ? Identifier
        : $"{Identifier[..32]}...";
}
