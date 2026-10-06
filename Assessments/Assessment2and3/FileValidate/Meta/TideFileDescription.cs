namespace FileValidate.Meta;

using System;

// Stores the metadata and calculated state of one file chunk.
public class TideChunkMetaData : ITideChunkMetaData
{
    // Index of this chunk within the package.
    public int ChunkIndex { get; set; }

    // Expected SHA-256 hash supplied by the .tpk file.
    public string Hash { get; set; }

    // Starting byte position in the target file.
    public long Offset { get; set; }

    // Number of bytes contained in this chunk.
    public int Size { get; set; }

    // Stores the most recently calculated hash.
    private string? _computedHash;
    public TideChunkMetaData()
    {
        ChunkIndex = 0;
        Hash = string.Empty;
        Offset = 0;
        Size = 0;
    }

    // Returns true after a hash has been calculated for this chunk.
    public bool ComputedHash()
    {
        return _computedHash is not null;
    }

    // Returns true when the calculated hash matches the expected hash.
    public bool IsComplete()
    {
        return _computedHash is not null &&
               string.Equals(
                   _computedHash,
                   Hash,
                   StringComparison.OrdinalIgnoreCase);
    }

    // Records the hash calculated from the actual chunk data.
    internal void SetComputedHash(string computedHash)
    {
        _computedHash = computedHash;
    }
}


public class TideFileDescription : ITideMetaData
{
    // <summary>
    // Identifier of the file
    // </summary>
    public string Identifier { get; set; }

    // <summary>
    // Filename that is referenced, created relative to the executable
    // </summary>
    public string Filename { get; set; }

    //Filepath for the dat filename
    public string? RelativeFilePath { get; set; }

    // <summary>
    // Size of the file referenced in the metadata
    // </summary>
    public long Size { get; set; }

    // <summary>
    // Number of non-leaf hashes - For Part 3
    // </summary>
    public int HashesCount { get; set; }

    // <summary>
    // List of the hashes given - Should match the hashes count
    // </summary>
    public List<string> Hashes { get; set; }

    // <summary>
    // Number of chunks the file should have
    // </summary>
    public int ChunksCount { get; set; }

    // <summary>
    // List of chunk meta data components
    // </summary>
    public List<ITideChunkMetaData> Chunks { get; set; }


    public TideFileDescription()
    {
        Identifier = string.Empty;
        Filename = string.Empty;
        Size = 0;
        HashesCount = 0;
        ChunksCount = 0;
        Hashes = new List<string>();
        Chunks = new List<ITideChunkMetaData>();
    }


    // Static method to create a TideFileDescription object from a metadata file path.
    public static TideFileDescription FromFilePath(string filepath)
	{
        string FilePath = Path.GetFullPath(filepath);

        if (!File.Exists(FilePath))

        {   
            throw new FileNotFoundException(
				$"ERROR: The metadata file could not be found at {FilePath}");
		}
        
        TideFileDescription MetaData = new TideFileDescription();

        MetaData.RelativeFilePath = FilePath;
        
        string currentSection = string.Empty;
        
        foreach (string rawLine in File.ReadLines(FilePath))
		{
            string line = rawLine.Trim();
            
            if (string.IsNullOrWhiteSpace(line))
			{
				continue;
			}
            
            if (line.StartsWith("ident:"))
			{
				MetaData.Identifier = line.Substring("ident:".Length).Trim();

				currentSection = string.Empty;
			}
            
            else if (line.StartsWith("filename:"))
			{
				MetaData.Filename =
					line.Substring("filename:".Length).Trim();

				currentSection = string.Empty;
			}
			else if (line.StartsWith("size:"))
			{
				MetaData.Size =
					long.Parse(line.Substring("size:".Length).Trim());

				currentSection = string.Empty;
			}
			else if (line.StartsWith("nhashes:"))
			{
				MetaData.HashesCount =
					int.Parse(line.Substring("nhashes:".Length).Trim());

				currentSection = string.Empty;
			}
            else if (line == "hashes:")
			{
				currentSection = "hashes";
			}
            
            else if (line.StartsWith("nchunks:"))
			{
				MetaData.ChunksCount =
					int.Parse(line.Substring("nchunks:".Length).Trim());

				currentSection = string.Empty;
			}
            
            else if (line == "chunks:")
			{
				currentSection = "chunks";
			}
            
            else if (currentSection == "hashes")
			{
				MetaData.Hashes.Add(line);
			}

            else if (currentSection == "chunks")
			{
				string[] parts = line.Split(',');
                
                if (parts.Length != 3)
				{
					throw new FormatException(
						$"Invalid chunk metadata: {line}");
				}

				TideChunkMetaData chunk = new TideChunkMetaData
				{
					ChunkIndex = MetaData.Chunks.Count,
					Hash = parts[0].Trim(),
					Offset = long.Parse(parts[1].Trim()),
					Size = int.Parse(parts[2].Trim())
				};

				MetaData.Chunks.Add(chunk);
			}
		}

		TideMetaUtils.ValidateFormat(MetaData);

		return MetaData;
	}
}

