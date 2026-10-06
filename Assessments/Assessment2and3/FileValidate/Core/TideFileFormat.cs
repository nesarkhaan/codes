namespace FileValidate.Core;

using FileValidate.Meta;

// <summary>
// TideFileFormat - used to represent kinds of file
// formats that can be used for reading and writing
// </summary>
public interface ITideFileFormat
{

    // <summary>
    // Returns the metadata constructed with the file
    // </summary>
    ITideMetaData? GetMetaData();

    // <summary>
    // Retrieves a chunk, the index corresponds to an index in the TideMetaData object 
    // </summary>
    ITideChunkMetaData? GetChunkMetaData(int index);

    // <summary>
    // Gets a list of bytes that correspond to the chunk
    // </summary>
    List<byte> GetChunkData(int index);

    // <summary>
    // Writes bytes contained in data to the chunk corresponding to index
    // </summary>
    void WriteDataToChunk(int index, List<byte> data);

    // <summary>
    // Computes the hash of the chunk referenced by the index
    // Uses SHA256 hash algorithm
    // </summary>
    string ComputeChunkHash(int index);


    // <summary>
    // Computes the hash of the chunk given
    // Uses SHA256 hash algorithm
    // </summary>
    string ComputeChunkHashOnBlock(List<byte> bytes);


    // <summary>
    // Identifies if the file is complete
    // </summary>
    bool IsComplete();


    // <summary>
    // Identifies if the file is incomplete
    // </summary>
    bool IsIncomplete();


    // <summary>
    // Returns a list of completed blocks
    // </summary>
    List<ITideChunkMetaData> CompletedBlocks();

    // <summary>
    // Returns a list of incompleted blocks
    // </summary>
    List<ITideChunkMetaData> IncompletedBlocks();
}
