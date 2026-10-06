namespace FileValidate.Core;

using FileValidate.Meta;
using System.Security.Cryptography;

public class TideFile : ITideFileFormat
{

    private readonly ITideMetaData PassedMetaData;
    // <summary>
    // Constructs an object of the implementing class using the metadata
    // </summary>
    //

    //Setting up a constructor to pull the data and placing it into a new private object. 
    public TideFile(ITideMetaData metadata)
    {

        PassedMetaData = metadata;

    }

    public static ITideFileFormat? WithMetaData(ITideMetaData metadata)
    {
        TideFile tideFile = new TideFile(metadata);

        // Resolve the location of the data file described by the .tpk.
        string dataFilePath = tideFile.GetDatFilePath();
        string? directory = Path.GetDirectoryName(dataFilePath);

        // Create the destination directory when it does not already exist.
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(dataFilePath))
        {
            // Create an empty target file with the size specified by the metadata.
            using FileStream stream = File.Create(dataFilePath);
            stream.SetLength(metadata.Size);
        }
        else
        {
            FileInfo fileInfo = new FileInfo(dataFilePath);

            if (fileInfo.Length != metadata.Size)
            {
                // Recreate the file when its size does not match the metadata.
                using FileStream stream = File.Create(dataFilePath);
                stream.SetLength(metadata.Size);
            }
        }

        // Calculate the initial validation state of every chunk.
        for (int index = 0; index < metadata.Chunks.Count; index++)
        {
            tideFile.ComputeChunkHash(index);
        }

        return tideFile;
    }


    // <summary>
    // Returns the metadata constructed with the file
    // </summary>
    public ITideMetaData? GetMetaData()
    {
        return PassedMetaData;
    }

    // <summary>
    // Retrieves a chunk, the index corresponds to an index in the TideMetaData object 
    // </summary>
    public ITideChunkMetaData? GetChunkMetaData(int index)
    {
        //Validating the index values to ensure indexoutofrange error is not return otherwise, returns the value of Chunks based on index. [CREATE A TEST]
        if (index < 0 || index >= PassedMetaData.Chunks.Count)
        {
            return null;
        }
        return PassedMetaData.Chunks[index];
    }

    public string GetDatFilePath()
    {

        string? directory = Path.GetDirectoryName(PassedMetaData.RelativeFilePath) ?? string.Empty;
        string? path = Path.Combine(directory, PassedMetaData.Filename);

        return path;
    }

    // <summary>
    // Gets a list of bytes that correspond to the chunk
    // </summary>
    public List<byte> GetChunkData(int index)
    {
        //Get the filepath 
        string? directory = Path.GetDirectoryName(PassedMetaData.RelativeFilePath);
        //Get the filename
        string? Filename = GetDatFilePath();//Path.Combine(directory, PassedMetaData.Filename);

        List<byte> Data = new List<byte>();

        ITideChunkMetaData? ChunkData = ChunkMetaDataHelper(index);

        if (ChunkData == null)
        {
            return Data;
        }

        using (FileStream fs = File.OpenRead(Filename))
        {
            //move the cursor to the offset value for the chunk in tpk file.
            fs.Seek(ChunkData.Offset, SeekOrigin.Begin);
            //Create a new temporary Buffer Array with the size value for the chunk from the tpk file.
            byte[] TemporaryBuffer = new byte[ChunkData.Size];

            fs.ReadExactly(TemporaryBuffer, 0, ChunkData.Size);
            Data.AddRange(TemporaryBuffer);
        }
        return Data;
    }

    private ITideChunkMetaData ChunkMetaDataHelper(int index)
    {
        ITideChunkMetaData? ChunkData = null;
        foreach (var ChunkDataInCollection in PassedMetaData.Chunks)
        {

            if (ChunkDataInCollection.ChunkIndex == index)
            {
                ChunkData = ChunkDataInCollection;
                break;
            }
        }
        return ChunkData;
    }





    // <summary>
    // Writes bytes contained in data to the chunk corresponding to index
    // </summary>
    public void WriteDataToChunk(int index, List<byte> data)
    {
        ITideChunkMetaData? chunkData = GetChunkMetaData(index);

        if (chunkData == null || data == null)
        {
            return;
        }

        int bytesToWrite = Math.Min(data.Count, chunkData.Size);
        string dataFilePath = GetDatFilePath();
        using (FileStream stream = new FileStream(
            dataFilePath,
            FileMode.OpenOrCreate,
            FileAccess.Write))
        {
            // Ensure the target file has its required total size.
            if (stream.Length < PassedMetaData.Size)
            {
                stream.SetLength(PassedMetaData.Size);
            }
            stream.Seek(chunkData.Offset, SeekOrigin.Begin);
            byte[] bytes = data.Take(bytesToWrite).ToArray();
            stream.Write(bytes, 0, bytes.Length);
        }

        // Update the completion state after changing the chunk data.
        ComputeChunkHash(index);
    }




    // <summary>
    // Computes the hash of the chunk referenced by the index
    // Uses SHA256 hash algorithm
    // </summary>
    public string ComputeChunkHash(int index)
    {
        ITideChunkMetaData? chunk = GetChunkMetaData(index);

        if (chunk == null)
        {
            return string.Empty;
        }

        List<byte> chunkData = GetChunkData(index);
        string computedHash = ComputeChunkHashOnBlock(chunkData);

        // Save the calculated hash so the chunk can report its current state.
        if (chunk is TideChunkMetaData tideChunk)
        {
            tideChunk.SetComputedHash(computedHash);
        }

        return computedHash;
    }


    // <summary>
    // Computes the hash of the chunk given
    // Uses SHA256 hash algorithm
    // </summary>
    public string ComputeChunkHashOnBlock(List<byte> bytes)
    {
        List<byte> chunkData = bytes;
        byte[] hashBytes = SHA256.HashData(chunkData.ToArray());

        // Convert byte array to a hex string
        string hash = Convert.ToHexString(hashBytes).ToLowerInvariant();

        return hash;
    }


    // <summary>
    // Identifies if the file is complete
    // </summary>
    public bool IsComplete()
    {
        return IncompletedBlocks().Count == 0;
    }


    // <summary>
    // Identifies if the file is incomplete
    // </summary>
    public bool IsIncomplete()
    {
        return !IsComplete();
    }


    // <summary>
    // Returns a list of completed blocks
    // </summary>
    public List<ITideChunkMetaData> CompletedBlocks()
    {
        List<ITideChunkMetaData> CompletedList = new List<ITideChunkMetaData>();

        foreach (var item in PassedMetaData.Chunks)
        {
            string ItemHashValue = ComputeChunkHash(item.ChunkIndex); //ChunkIndex is used in ComputeChunkHash, which then pulls the corresponding Chunkdata
            if (ItemHashValue == item.Hash)
            {
                CompletedList.Add(item);
            }

        }

        return CompletedList;
    }

    // <summary>
    // Returns a list of incompleted blocks
    // </summary>
    public List<ITideChunkMetaData> IncompletedBlocks()
    {
        List<ITideChunkMetaData> CompletedList = CompletedBlocks();
        List<ITideChunkMetaData> InCompletedList = new List<ITideChunkMetaData>();

        //Add the chunk index to Hashset list. 
        HashSet<int> ListOfIndex = new HashSet<int>();

        foreach (var item in CompletedList)
        {
            ListOfIndex.Add(item.ChunkIndex);
        }

        //Compare the completed list chunk index to Passed Meta data chunk index. 
        foreach (var item in PassedMetaData.Chunks)
        {
            if (!ListOfIndex.Contains(item.ChunkIndex))
            {
                InCompletedList.Add(item);
            }
        }
        return InCompletedList;
    }
}
