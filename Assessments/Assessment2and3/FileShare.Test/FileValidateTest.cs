namespace FileShare.Test;

using FileShare.Lib;
using FileShare.Lib.Adapters;
using System;
using System.IO;
using Xunit;

public class GeneratePaths
{
    // Generates temporary paths for testing purposes.
    private string TempBasePath = String.Empty;
    // Temporary path for metadata files.
    string TempMetaPath = String.Empty;
    // Temporary path for data files.
    string TempFilePath = String.Empty;
    // Initializes the GeneratePaths class and sets up temporary paths.
    public GeneratePaths()
    {
        TempBasePath = Path.GetTempPath();
        TempMetaPath = Path.Combine(TempBasePath, Guid.NewGuid().ToString());
        TempFilePath = Path.Combine(TempBasePath, Guid.NewGuid().ToString());
    }
    // Returns the temporary file path.
    public string TemporaryFilePath()
    {
        return TempFilePath;

    }
    // Returns the temporary metadata path.
    public string TemporaryMetaPath()
    {

        return TempMetaPath;

    }

}


//  
public class FileValidationTests
{


    // Test to ensure that the P2PFileManager initializes correctly with valid paths.
    [Fact]

    public void Initialise_test_null_PASS()
    {
        var path = new GeneratePaths();
        var manager = new P2PFileManager(
            new TideMetadataAdapter());


        var exception = Record.Exception(() =>

                {
                    manager.Initialize(path.TemporaryMetaPath(), path.TemporaryFilePath());
                });
        Assert.Null(exception);

    }
    // Test to ensure that the P2PFileManager throws an ArgumentNullException when initialized with a null metadata path.
    [Fact]
    public void Initialise_test_ThrowNullError()
    {

        var path = new GeneratePaths();
        var manager = new P2PFileManager(new TideMetadataAdapter());

        Assert.Throws<ArgumentNullException>(() =>
        {
            manager.Initialize(null, path.TemporaryFilePath());

        });


    }

}
