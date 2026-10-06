namespace SnapshotKV;
//This file contains the logic code for commit, checkout, load and save.

//OpQueryDB is a partial class.
public partial class OpQueryDB
{
    private SnapshotKVDB DB;
    private string? DBSavePath;

    //Loading the database
    public OpQueryDB(SnapshotKVDB database)
    {
        DB = database ?? new SnapshotKVDB();

        if (DB == null)
        {
            Console.WriteLine("Database could not be initialised");
        }
    }

    public SnapshotDBOpResult Save(string path)
    {

        this.DBSavePath = path;
        //Pulling the Keys, KeyCount and Database and placing it into the instance of Operations NewSave
        Operations NewSave = new Operations(DB.GetKeysArray(), DB.GetDatabaseArray(), DB.GetKeyCount());

        //Get relative full path and combine
        System.IO.StreamWriter writer;
        writer = new System.IO.StreamWriter(DBSavePath);
        //Through a double loop, saving the keys and database into a file.
        for (int i = 0; i < NewSave.GetSavedKeys()?.Length; i++)
        {
            writer.Write($"{NewSave.GetSavedKeys()[i].ToString()}");
            for (int j = 0; j < NewSave.GetSavedDatabase()[i].Length; j++)
            {
                writer.Write($",{NewSave.GetSavedDatabase()[i][j].ToString()}");
            }
            writer.Write("\n");
        }
        //Closing the StreamWriter so the data can be moved from the buffer into the file. 
        writer.Close();

        return new OpResults("Save", true, DB);
    }

    //Helper Method to pull line numbers for the Load.Line number in files are used to update the keys and database arrays. 
    public int LineCounter(string path)
    {
        int count = 0;
        System.IO.StreamReader counter;
        counter = new System.IO.StreamReader(path);
        while ((counter.ReadLine()) != null)
        {
            count++;
        }

        counter.Close();
        return count;
    }


    //Helper method for load. Converting string to int.  
    static int[] ConvertToInt(string[] parts)
    {
        int[] Parts = new int[parts.Length];

        for (int i = 0; i < parts.Length; i++)
        {
            Parts[i] = Convert.ToInt32(parts[i]);
        }

        return Parts;

    }

    //Load method takes a file path and name which is then loaded into the keys, database, counts. 
    public SnapshotDBOpResult Load(string path)
    {

        string? line = null;
        //Checking if file exists.
        if (!System.IO.File.Exists(path))
        {

            Console.WriteLine("File does not exist");
            return new OpResults("Load", false, DB);

        }
        //Temporary Arrays for LoadKeys and Database. Function created to get the number of lines in the file.
        int LinesInFile = LineCounter(path);
        int lineCounter = 0;
        int[]? LoadKeys = new int[LinesInFile];
        int[][]? LoadDatabase = new int[LinesInFile][];

        DBSavePath = path;

        System.IO.StreamReader reader;
        reader = new System.IO.StreamReader(path);

        while ((line = reader.ReadLine()) != null)
        {

            string[] Parts = line.Split(",");
            int[] PartsConverted2Int = ConvertToInt(Parts);
            LoadKeys[lineCounter] = PartsConverted2Int[0];
            LoadDatabase[lineCounter] = new int[Parts.Length - 1];

            for (int i = 1; i < Parts.Length; i++)
            {
                LoadDatabase[lineCounter][i - 1] = PartsConverted2Int[i];

            }
            lineCounter++;

        }
        reader.Close();
        DB.SetKey(LoadKeys);
        DB.SetDatabase(LoadDatabase);
        DB.SetCount(LinesInFile);
        //Helper methods which resets the WatcherKeys, Watcher Starts and Watcher Ends. 
        DB.ResetInternalState();

        return new OpResults("Load", true, DB);

    }
    //Commit saves a snapshot of the database and keys at this current moment. Multiple commits can be saved. 
    public SnapshotDBOpResult Commit()
    {
        Operations newCommit = new Operations(DB.GetKeysArray(), DB.GetDatabaseArray(), DB.GetKeyCount());

        int ID = DB.GetCommitCount();

        DB.SetCommit(ID, newCommit);

        DB.IncrementCommitCount();

        return new OpResults("Commit", true, DB);
    }


    public SnapshotDBOpResult Checkout(int id)
    {
        int currentTotal = DB.GetCommitCount();

        // ID Validation
        if (id < 0 || id >= currentTotal)
        {
            return new OpResults("Checkout", false, DB);
        }
        // Attempting to pull the "Box"
        Operations savedBox = DB.GetCommit(id);

        // Guard Clause 2: Null Check
        if (savedBox == null)
        {
            return new OpResults("Checkout", false, DB);
        }

        // Verification of the data inside the box
        int[] keysToRestore = savedBox.GetSavedKeys();

        //Updating the live database
        DB.SetKey(keysToRestore);
        DB.SetDatabase(savedBox.GetSavedDatabase());
        DB.SetCount(savedBox.GetSavedCount());

        return new OpResults("Checkout", true, DB);

    }

}

//Operations class
public class Operations
{
    private int[] _savedKeys;
    private int[][] _savedDatabases;
    private int _savedCount;


    //Operations constructor, it loads the Keys, Database and Count into an instance.
    public Operations(int[] liveKeys, int[][] liveDatabases, int liveCount)
    {

        _savedCount = liveCount;
        _savedKeys = new int[liveKeys.Length];
        for (int i = 0; i < liveKeys.Length; i++)
        {
            _savedKeys[i] = liveKeys[i];

        }

        _savedDatabases = new int[liveDatabases.Length][];

        for (int i = 0; i < liveDatabases.Length; i++)
        {
            _savedDatabases[i] = new int[liveDatabases[i].Length];
            for (int j = 0; j < liveDatabases[i].Length; j++)
            {
                _savedDatabases[i][j] = liveDatabases[i][j];
            }
        }

    }
    //Gets the _savedCount 
    public int GetSavedCount()
    {
        return _savedCount;
    }

    //Gets the savedkeys which have been loaded by the constructor
    public int[] GetSavedKeys()
    {

        int[] keyCopy = new int[_savedKeys.Length];
        for (int i = 0; i < _savedKeys.Length; i++)
        {
            keyCopy[i] = _savedKeys[i];
        }
        return keyCopy;

    }
    //Gets the saveddatabase which has been loaded by the constructor
    public int[][] GetSavedDatabase()
    {

        int[][] dbCopy = new int[_savedDatabases.Length][];



        for (int i = 0; i < _savedDatabases.Length; i++)
        {
            dbCopy[i] = new int[_savedDatabases[i].Length];
            for (int j = 0; j < _savedDatabases[i].Length; j++)
            {
                dbCopy[i][j] = _savedDatabases[i][j];
            }
        }
        return dbCopy;

    }
}

