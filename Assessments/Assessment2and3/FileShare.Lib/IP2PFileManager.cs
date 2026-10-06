namespace FileShare.Lib;

public interface IP2PManager
{
    // Initializes the P2P manager with the specified directories for metadata and file storage.
    void Initialize(string metaDirectory, string fileDirectory);

    // Retrieves the metadata for a managed package.
    bool AddPackage(string filePath);

    // Removes a managed package by its identifier.
    bool RemovePackage(string identifier);


    
    byte[] GetChunkForPeer(string identifier, int chunkIndex);

    //  Writes a chunk of data received from a peer to the managed package and verifies its integrity.
    bool WriteAndVerifyChunkFromPeer(string identifier, int chunkIndex, byte[] data);

    // Retrieves a list of all managed packages, including their states and metadata.
    IEnumerable<PackageState> GetAllManagedPackages();


}
