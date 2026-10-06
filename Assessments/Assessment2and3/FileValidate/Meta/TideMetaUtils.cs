namespace FileValidate.Meta;


public static class TideMetaUtils
{

    public static void PackageHashesData(StreamReader reader, TideFileDescription metadata)
    {
        TideFileDescription MetaData = metadata;

        for (int i = 0; i < MetaData.HashesCount; i++)
        {
            string? ReadNextLine = reader.ReadLine();

            if (ReadNextLine != null)
            {
                MetaData.Hashes.Add(ReadNextLine.Trim());
            }
        }
    }


    public static void PackageChunksData(StreamReader reader, TideFileDescription metadata)

    {
        StreamReader FileStreamReader = reader;
        TideFileDescription MetaData = metadata;

        for (int i = 0; i < MetaData.ChunksCount; i++)
        {
            //Creating Object for Chunks Meta Data such as Hash, Offset and Size.
            // Loop iteration number (i) is used as ChunkIndex.
            TideChunkMetaData newChunk = new TideChunkMetaData();
            string? ReadNextLine = FileStreamReader.ReadLine();

            if (string.IsNullOrWhiteSpace(ReadNextLine))
            {
                break;
            }

            if (ReadNextLine != null)
            {
                string[] chunkParts = ReadNextLine.Split(",");
                newChunk.ChunkIndex = i;
                newChunk.Hash = chunkParts[0].Trim();
                newChunk.Offset = long.Parse(chunkParts[1].Trim());
                newChunk.Size = int.Parse(chunkParts[2].Trim());

                //Add each chunk to the MetaData List.
                MetaData.Chunks.Add(newChunk);
            }
        }
    }




    public static void PackageTideFileDescription(StreamReader reader, string? readfileline, TideFileDescription metadata, string filepath)
    {
        var FileStreamReader = reader;
        TideFileDescription MetaData = metadata;
        string?[] Parts = new string[3];
        string? ReadFileLine = readfileline;

        //Without this, Below code returns indexoutofrange as some lines were empty.
        if (string.IsNullOrWhiteSpace(ReadFileLine) || !ReadFileLine.Contains(":"))
        {
            return;
        }


        Parts = ReadFileLine.Split(":", 2);
        if (Parts.Length < 2)
        {
            return;
        }

        string LabelFromFile = Parts[0].Trim();
        string value = Parts[1].Trim();

        switch (LabelFromFile) //Switch cases to get data of ident, filename, size, nhashes and nchunks
        {
            //Place Identifier into MetaData 
            case "ident":
                MetaData.Identifier = value;
                break;

            //Place Filename into MetaData
            case "filename":
                MetaData.Filename = value;
                MetaData.RelativeFilePath = filepath;
                break;

            //Place Size into MetaData
            case "size":
                MetaData.Size = int.Parse(value.Trim());
                break;

            // Hash Numbers within the file
            case "nhashes":
                MetaData.HashesCount = int.Parse(value.Trim());
                break;

            // Chunk Numbers within the file.
            case "nchunks":
                MetaData.ChunksCount = int.Parse(value);
                break;

            case "hashes":
                PackageHashesData(FileStreamReader, MetaData);
                break;

            case "chunks":
                PackageChunksData(FileStreamReader, MetaData);
                break;

            default:
                break;
        }

    }


    public static void ValidateFormat(TideFileDescription metadata)
    {
        //Check Ident first
        if (string.IsNullOrWhiteSpace(metadata.Identifier))
        {
            throw new InvalidDataException("ERROR: ident field is missing");
        }

        if (string.IsNullOrWhiteSpace(metadata.Filename))
        {

            throw new InvalidDataException("ERROR: filename field is missing");
        }

        if (metadata.Size <= 0)
        {

            throw new InvalidDataException("ERROR: size field is missing");
        }

        if (metadata.HashesCount <= 0)
        {

            throw new InvalidDataException("ERROR: nhashes field is missing");
        }

        if (metadata.Hashes.Count <= 0)
        {

            throw new InvalidDataException("ERROR: hashes field is missing");
        }


        if (metadata.ChunksCount <= 0)
        {

            throw new InvalidDataException("ERROR: nchunks field is missing");
        }

        if (metadata.Chunks.Count <= 0)
        {

            throw new InvalidDataException("ERROR: chunks field is missing");
        }

        // Ensure the number of hashes matches the nhashes value.
        if (metadata.HashesCount != metadata.Hashes.Count)
        {
            throw new FormatException(
                "The hashes count does not match the number of hashes listed.");
        }

        // Ensure the number of chunks matches the nchunks value.
        if (metadata.ChunksCount != metadata.Chunks.Count)
        {
            throw new FormatException(
                "The chunks count does not match the number of chunks listed.");
        }


    }




}








