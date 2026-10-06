namespace FileValidate.Meta;


// <summary>
// The ITideMetaData interface is used to store the following information
// regarding a tpk file.
// * Identifier - (ident) within the file
// * Filename - (filename) within the file, if it does not exist, it should be created
// * Size - (size) number of bytes that represents the file
// * HashesCount - (nhashes) number of hashes that are listed within the tpk file
// * Hashes  - (hashes) list of hashes, these represent non-leaf nodes within the merkle tree
// * ChunksCount - (nchunks) number of chunks that the file is broken up into
// * Chunks  - (chunks) Comma separated: Hash representing the chunk, starting position, end byte (not including).
// </summary>
public interface ITideMetaData
{

    // <summary>
    // Identifier of the file
    // </summary>
    string Identifier { get; set; }

    // <summary>
    // Filename that is referenced, created relative to the executable
    // </summary>
    string Filename { get; set; }

    string RelativeFilePath { get; set; } //Ref: I added this

    // <summary>
    // Size of the file referenced in the metadata
    // </summary>
    long Size { get; set; }

    // <summary>
    // Number of non-leaf hashes - For Part 3
    // </summary>
    int HashesCount { get; set; }

    // <summary>
    // List of the hashes given - Should match the hashes count
    // </summary>
    List<string> Hashes { get; set; }

    // <summary>
    // Number of chunks the file should have
    // </summary>
    int ChunksCount { get; set; }

    // <summary>
    // List of chunk meta data components
    // </summary>
    List<ITideChunkMetaData> Chunks { get; set; }

}



// <summary>
// The ITideChunkData interface is used to store the following information
// * Hash - Expected hash of the chunk
// * Offset - The starting byte of the chunk
// * Size - The size of the chunk
// </summary>
public interface ITideChunkMetaData
{

    // <summary>
    // Index that corresponds the list this object is contained in
    // </summary>
    int ChunkIndex { get; set; }

    // <summary>
    // Expected hash of the chunk
    // </summary>
    string Hash { get; set; }

    // <summary>
    // Offset within the file referenced
    // </summary>
    long Offset { get; set; }

    // <summary>
    // Size of the chunk
    // </summary>
    int Size { get; set; }


    // <summary>
    // True if the chunk is complete (chunk computed matches hash)
    // </summary>
    bool IsComplete();

    // <summary>
    // Returns the computed hash for the chunk
    // </summary>
    bool ComputedHash();

}
