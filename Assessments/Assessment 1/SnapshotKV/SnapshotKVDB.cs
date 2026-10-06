namespace SnapshotKV;

///<summary>
/// Type that represents the KV-database
/// You are free to implement
///</summary>
public class SnapshotKVDB
{
    // You can add fields, properties and methods to this type
    private int[] Keys;
    private int[][] Database;
    private int _count;
    //Makewatcher and VeiwWatcher
    private int[] WatcherKeys;
    private int[] WatcherStarts;
    private int[] WatcherEnds;
    private int _watcherCount;

    //Commit
    private Operations[] commits;
    private int _commitCount;


    private OpQueryDB shareDB;


    public SnapshotKVDB()
    {
        //sharing the db with other files.
        shareDB = new OpQueryDB(this);

        //Initialising commits array and counter. 
        commits = new Operations[100]; // Capacity for 100 snapshots
        _commitCount = 1;

        //MakeWatcher and ViewWatcher Variables

        _watcherCount = 1;
        WatcherKeys = new int[5];
        WatcherStarts = new int[5];
        WatcherEnds = new int[5];


        //Modify here. To manually test, change to 6 or else 0 fornormal testing
        _count = 0;
        Keys = new int[] { 0, 4, 9, 14, 19, 24 };
        //Keys = new int[]{ };

        //Database
        Database = new int[6][];
        Database[0] = [10, 9, 8, 5, 4, 2, 4, 0];
        Database[1] = [7, 2, 5, 9, 9];
        Database[2] = [10, 11, 12, 13, 14];
        Database[3] = [15, 16, 17, 18, 19];
        Database[4] = [20, 21, 22, 23, 24];
        Database[5] = [25, 26, 27, 28, 29];


    }


    //Database, Keys and Count
    public int[] GetKeysArray() => Keys;
    public int[][] GetDatabaseArray() => Database;
    public int GetKeyCount() => _count;
    public void IncrementKeyCount() => _count++;  //Increments Key Counts.
    public void DecrementKeyCount() => _count--;

    //Watcher
    public int GetWatcherCount() => _watcherCount;
    public int[] GetWatcherKey() => WatcherKeys;
    public int[] GetWatcherStarts() => WatcherStarts;
    public int[] GetWatcherEnds() => WatcherEnds;
    public void IncrementWatcherCount() => _watcherCount++;

    //Makewatch and ViewWatcher - Logic Queries.cs
    public SnapshotQueryResult MakeWatcher(int key, int startIndex, int endIndex) => shareDB.MakeWatcher(key, startIndex, endIndex);
    public SnapshotQueryResult ViewWatcher(int watcherKey) => shareDB.ViewWatcher(watcherKey);

    //Commit - Logic Operations.cs
    public SnapshotDBOpResult Commit() => shareDB.Commit();
    public int GetCommitCount() => _commitCount; //Gets the Commit Counts
    public void SetCommit(int id, Operations op) => commits[id] = op;
    public void IncrementCommitCount() => _commitCount++; //Increments Commit Counts


    //Checkout - Logic Operations.cs
    public SnapshotDBOpResult Checkout(int id) => shareDB.Checkout(id);
    public Operations GetCommit(int id) => commits[id];
    public void SetKey(int[] newKey) => Keys = newKey;
    public void SetDatabase(int[][] newDatabase) => Database = newDatabase;
    public void SetCount(int newCount) => _count = newCount;


    //ListKeys - Queries.cs

    public SnapshotQueryResult ListKeys() => shareDB.ListKeys();

    //GetEntry - Logic in Queries.cs
    public SnapshotQueryResult GetEntry(int key) => shareDB.GetEntry(key);

    //SetEntry - Logic code in Queries.cs. Increment key count method create to increase _count; 
    public SnapshotQueryResult SetEntry(int key, int[] data) => shareDB.SetEntry(key, data);

    //Remove Entry - Logic in Quiries.cs. Decrements key count method created to decrease _count;
    public SnapshotQueryResult RemoveEntry(int key) => shareDB.RemoveEntry(key);

    //Push - Logic in Quieries.cs. Methods used are arrayExtend(), findKey(key) and IncrementKeyCount().
    public SnapshotQueryResult Push(int key, int[] data) => shareDB.Push(key, data);

    //Append Queries.cs. ArrayExtend(), findKey() and IncrementKeyCount() are used.
    public SnapshotQueryResult Append(int key, int[] data) => shareDB.Append(key, data);

    //View - Logic in Queries.cs
    public SnapshotQueryResult View(int key, int startIndex, int endIndex) => shareDB.View(key, startIndex, endIndex);

    //Sort - Logic in Queires.cs
    public SnapshotQueryResult Sort(int key) => shareDB.Sort(key);

    //Partial Sort - Queries.cs
    public SnapshotQueryResult PartialSort(int key, int startIndex, int endIndex) => shareDB.PartialSort(key, startIndex, endIndex);

    //Union Queries.cs
    public SnapshotQueryResult Union(int key1, int key2) => shareDB.Union(key1, key2);

    //Intersect Queries.cs
    public SnapshotQueryResult Intersect(int key1, int key2) => shareDB.Intersect(key1, key2);

    //Save Logic in Operations.cs
    public SnapshotDBOpResult Save(string path) => shareDB.Save(path);

    //Load Logic in Operations.cs
    public SnapshotDBOpResult Load(string path) => shareDB.Load(path);





    ///-------------------------- Other Methods  -----------------------------------------///


    // finds and validates correct key and returns the index
    public int findKey(int num)
    {
        int index = 0;
        for (int i = -0; i < _count; i++)
        {
            if (Keys[i] == num)
            {
                index = i;
                return index;
            }


        }
        return -1;
    }

    //Extends the array. 
    public void arrayExtend(int num)
    {
        int[] newKeys = new int[num];
        int[][] newDatabase = new int[num][];
        for (int i = 0; i < Keys.Length; i++)
        {
            newKeys[i] = Keys[i];
            newDatabase[i] = Database[i];
        }

        Keys = newKeys;
        Database = newDatabase;
    }

    //Extends the watcher array.  
    public void ExtendWatcherArrays(int num)
    {
        int[] NewWatcherKeys = new int[num];
        int[] NewWatcherStart = new int[num];
        int[] NewWatcherEnds = new int[num];

        for (int i = 0; i < WatcherKeys.Length; i++)
        {
            NewWatcherKeys[i] = WatcherKeys[i];
            NewWatcherStart[i] = WatcherStarts[i];
            NewWatcherEnds[i] = WatcherEnds[i];
        }

        WatcherKeys = NewWatcherKeys;
        WatcherStarts = NewWatcherStart;
        WatcherEnds = NewWatcherEnds;
    }

    //Resets the Watcher Keys, Starts and Ends
    public void ResetInternalState()
    {

        WatcherKeys = new int[5];
        WatcherStarts = new int[5];
        WatcherEnds = new int[5];

        commits = new Operations[100];
        _commitCount = 1;

        _watcherCount = 1;

        Console.WriteLine("All watcher and commit values Dropped!");
    }

}
