namespace FileShare.Lib.Data;

// An absolute shared path prevents Service and Web creating separate databases
// simply because they start from different project directories.
public static class FileShareDatabase
{   // The directory where the database file is stored, created if it doesn't exist.
    public static string DataDirectory { get; } = CreateDataDirectory();
    // The full path to the SQLite database file, combining the data directory and the database file name.
    public static string DatabasePath => Path.Combine(DataDirectory, "FileShare.db");
    // The connection string used to connect to the SQLite database, specifying the data source as the database path.
    public static string ConnectionString => $"Data Source={DatabasePath}";
    // Creates the data directory if it doesn't exist and returns its path.
    private static string CreateDataDirectory()
    {
        // Use LocalApplicationData to ensure the directory is user-specific and accessible by both Service and Web.
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FileShare");
        Directory.CreateDirectory(directory);
        return directory;
    }
}
