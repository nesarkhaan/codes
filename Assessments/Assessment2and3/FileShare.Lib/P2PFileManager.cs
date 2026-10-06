namespace FileShare.Lib;

using FileShare.Abstractions.Interfaces;
using FileShare.Abstractions.Models;
// Manages P2P file sharing operations, including adding, removing, and retrieving packages and chunks.</summary>
public class P2PFileManager : IP2PManager
{   
    // Synchronization object for thread safety.
    private readonly object _sync = new();

    // Adapter for file validation and metadata reading.
    private readonly IFileValidationAdapter _fileValidation;
    
    // Directories for metadata and file storage.
    private string MetaDirectory = string.Empty;
    
    // Directory for storing the actual files associated with the packages.
    private string FileDirectory = string.Empty;
    
    // Dictionary to hold the currently managed packages, keyed by their identifiers.
    private readonly Dictionary<string, IManagedPackage> CurrentManagedPackages = new();
    
    // Constructor that initializes the P2PFileManager with a file validation adapter.
    public P2PFileManager(IFileValidationAdapter fileValidation)
    {
        _fileValidation = fileValidation
            ?? throw new ArgumentNullException(nameof(fileValidation));
    }
    // Initializes the P2P manager with the specified directories for metadata and file storage.
    public void Initialize(string metaDirectory, string fileDirectory)
    {
        // Guard clauses catch null or empty inputs early.
        ArgumentException.ThrowIfNullOrWhiteSpace(metaDirectory);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileDirectory);

        MetaDirectory = Path.GetFullPath(metaDirectory);
        FileDirectory = Path.GetFullPath(fileDirectory);

        // Validate and create the metadata and file directories.
        EnsureDirectory(MetaDirectory, "metafile_directory");
        EnsureDirectory(FileDirectory, "file_directory");

        // If all goes well, load the .tpk files into CurrentManagedPackages.
        lock (_sync)
        {
            CurrentManagedPackages.Clear();
            foreach (var path in Directory.EnumerateFiles(MetaDirectory, "*.tpk", SearchOption.TopDirectoryOnly))
            {
                try { LoadPackage(path); }
                catch (Exception ex) when (ex is InvalidDataException or FormatException or IOException or UnauthorizedAccessException)
                {
                    Console.Error.WriteLine($"Unable to parse tpk file: {Path.GetFileName(path)}");
                }
            }
        }
    }
    // Adds a package to the manager by reading its metadata from the specified file path.
    public bool AddPackage(string filePath)
    {   
        if (string.IsNullOrWhiteSpace(filePath)) return false;        
        if (!File.Exists(filePath)) return false;        
        MetadataFile description;
        try { description = _fileValidation.Read(filePath); }
        catch (Exception ex) when (ex is InvalidDataException or FormatException or IOException or UnauthorizedAccessException) { return false; }

        lock (_sync)
        {
            if (CurrentManagedPackages.ContainsKey(description.Identifier)) return false;
            var destination = Path.Combine(MetaDirectory, Path.GetFileName(filePath));
            if (!Path.GetFullPath(filePath).Equals(Path.GetFullPath(destination), StringComparison.OrdinalIgnoreCase))
            {
                destination = MakeUniqueDescriptorPath(destination);
                File.Copy(filePath, destination, overwrite: false);
            }
            LoadPackage(destination);
            return true;
        }
    }
    // Removes a package from the manager based on its identifier, deleting associated metadata files.
    public bool RemovePackage(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier) || identifier.Length < 20) return false;
        lock (_sync)
        {
            var matches = CurrentManagedPackages.Keys.Where(key => key.StartsWith(identifier, StringComparison.OrdinalIgnoreCase)).ToArray();
            if (matches.Length != 1) return false;
            var fullIdentifier = matches[0];
            CurrentManagedPackages.Remove(fullIdentifier);

            foreach (var descriptor in Directory.EnumerateFiles(MetaDirectory, "*.tpk"))
            {
                try
                {
                    if (_fileValidation.Read(descriptor).Identifier.Equals(fullIdentifier, StringComparison.OrdinalIgnoreCase))
                    {
                        File.Delete(descriptor);
                    }
                }
                catch { /* Keep malformed unrelated descriptors untouched. */ }
            }
            return true;
        }
    }
    // Retrieves a specific chunk of data for a peer based on the package identifier and chunk index.
    public byte[] GetChunkForPeer(string identifier, int chunkIndex)
    {
        lock (_sync)
        {
            if (!TryGetPackage(identifier, out var package)) return [];
            return package.ReadVerifiedChunk(chunkIndex);
        }
    }

    // Writes a chunk of data received from a peer to the package, verifying its integrity.
    public bool WriteAndVerifyChunkFromPeer(string identifier, int chunkIndex, byte[] data)
    {
        ArgumentNullException.ThrowIfNull(data);
        lock (_sync)
        {
            if (!TryGetPackage(identifier, out var package)) return false;
            return package.WriteAndVerifyChunk(chunkIndex, data);
        }
    }
    // Returns a list of all managed packages with their current state, including completion percentage.
    public IEnumerable<PackageState> GetAllManagedPackages()
    {
        lock (_sync)
        {
            return CurrentManagedPackages.Values.Select(package =>
            {
                var meta = package.Metadata;
                var completed = package.CompletedBlocks;
                var total = meta.ChunksCount;
                return new PackageState(meta.Identifier, meta.Filename, completed == total,
                    total == 0 ? 0 : completed * 100d / total);
            }).ToArray();
        }
    }

    //Returns detailed package information used by networking and UI adapters.</summary>
    public IReadOnlyList<ManagedPackageInfo> GetPackageDetails()
    {
        lock (_sync)
        {
            return CurrentManagedPackages.Values.Select(package =>
            {
                var meta = package.Metadata;
                var completed = package.CompletedBlocks;
                return new ManagedPackageInfo(meta.Identifier, meta.Filename, meta.TargetPath,
                    meta.Size, completed, meta.ChunksCount, completed == meta.ChunksCount);
            }).ToArray();
        }
    }
    // Returns a list of incomplete chunks across all managed packages, including their identifiers and sizes.
    public IReadOnlyList<IncompleteChunkInfo> GetIncompleteChunks()
    {
        lock (_sync)
        {
            var result = new List<IncompleteChunkInfo>();
            foreach (var package in CurrentManagedPackages.Values)
            {
                foreach (var chunk in package.IncompleteChunks)
                    result.Add(new IncompleteChunkInfo(package.Metadata.Identifier, chunk.Index, chunk.Size));
            }
            return result;
        }
    }
    // Tries to get the size of a specific chunk for a given package identifier and chunk index.
    public bool TryGetChunkInfo(string identifier, int chunkIndex, out int chunkSize)
    {
        lock (_sync)
        {
            chunkSize = 0;
            if (!TryGetPackage(identifier, out var package)) return false;
            if (!package.TryGetChunk(chunkIndex, out var chunk) || chunk is null) return false;
            chunkSize = chunk.Size;
            return true;
        }
    }
    // Helper method to load a package from a descriptor file and add it to the managed packages if not already present.
    private void LoadPackage(string descriptorPath)
    {
        var package = _fileValidation.OpenPackage(descriptorPath, FileDirectory);
        if (CurrentManagedPackages.ContainsKey(package.Metadata.Identifier)) return;
        
        CurrentManagedPackages.Add(package.Metadata.Identifier, package);
    }
    // Helper method to retrieve a package by its identifier, allowing for partial matches if unique.
    private bool TryGetPackage(string identifier, out IManagedPackage package)
    {
        package = null!;
        if (string.IsNullOrWhiteSpace(identifier)) return false;
        if (CurrentManagedPackages.TryGetValue(identifier, out package!)) return true;
        var matches = CurrentManagedPackages.Where(pair => pair.Key.StartsWith(identifier, StringComparison.OrdinalIgnoreCase)).ToArray();
        if (matches.Length != 1) return false;
        package = matches[0].Value;
        return true;
    }

    // Helper method for Initialize. Directory check, creation and validation.
    private static void EnsureDirectory(string path, string label)
    {
        try
        {
            if (File.Exists(path)) throw new IOException();
            Directory.CreateDirectory(path);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException or NotSupportedException)
        {
            throw new IOException($"Unable to create {label}", ex);
        }
    }
    // Generates a unique file path for a descriptor by appending a numeric suffix if the file already exists.
    private static string MakeUniqueDescriptorPath(string destination)
    {
        if (!File.Exists(destination)) return destination;
        var directory = Path.GetDirectoryName(destination)!;
        var name = Path.GetFileNameWithoutExtension(destination);
        for (var i = 1; ; i++)
        {
            var candidate = Path.Combine(directory, $"{name}-{i}.tpk");
            if (!File.Exists(candidate)) return candidate;
        }
    }

}
// Represents the state of a package, including its identifier, filename, completion status, and completion percentage.
public sealed record ManagedPackageInfo(string Identifier, string Filename, string TargetPath, long Size,
    int CompletedBlocks, int TotalBlocks, bool IsComplete);

// Represents the state of a package, including its identifier, filename, completion status, and completion percentage.
public sealed record IncompleteChunkInfo(string Identifier, int ChunkIndex, int Size);



