namespace Demo2_constructor;

class Program
{

       // private int[] liveKeys;
        //private int[][] Database;
        //private int _countLive;

    static void Main(string[] args)
    {

        int _countLive = 2;
        int[] liveKeys = new int[2]{101, 102};
        int[][] Database = new int[2][];
        Database[0] = [1, 2, 3];
        Database[1] = [4, 5];


        DataEntry Save1 = new DataEntry(liveKeys, Database, _countLive);
        liveKeys[0] = 999;
        Database[0][0] = 888;
        _countLive = 99;
        Console.WriteLine("Printing liveKeys[0]");
        Console.WriteLine(liveKeys[0]);
        Console.WriteLine(Database[0][0]);
        Console.WriteLine(_countLive = 99);
        DataEntry Save2 = new DataEntry(liveKeys, Database, _countLive);

        Console.WriteLine("---- Printing Save 1 -------");
        Save1.PrintSave();

        Console.WriteLine("---- Printing Save2 -------");
        Save2.PrintSave();


        
        
    }
}

class DataEntry
{
    private int[] _savedKeys;
    private int[][] _savedDatabase;
    private int _savedCount;

    public DataEntry(int[] liveKeys, int[][] liveDatabase, int liveCount)
    {
        _savedCount = liveCount;
        _savedKeys = new int[liveKeys.Length];
        for (int i = 0; i < liveKeys.Length; i++)
        {
            _savedKeys[i] = liveKeys[i];
        }

        _savedDatabase = new int[liveDatabase.Length][];

        for (int i = 0; i < liveDatabase.Length; i++)
        {
            _savedDatabase[i] = new int[liveDatabase[i].Length];
            for (int j = 0; j < liveDatabase[i].Length; j++)
            {
                _savedDatabase[i][j] = liveDatabase[i][j];
            }
        }


    }

    public void PrintSave()
{
    Console.WriteLine("--- INSIDE THE TIME CAPSULE ---");
    Console.WriteLine($"Saved Count: {_savedCount}");
    
    Console.Write("Saved Keys: ");
    for (int i = 0; i < _savedKeys.Length; i++)
    {
        Console.Write(_savedKeys[i] + " ");
    }
    Console.WriteLine();

    Console.WriteLine("Saved Database:");
    for (int i = 0; i < _savedDatabase.Length; i++)
    {
        Console.Write($"Row {i}: ");
        for (int j = 0; j < _savedDatabase[i].Length; j++)
        {
            Console.Write(_savedDatabase[i][j] + " ");
        }
        Console.WriteLine();
    }
    Console.WriteLine("-------------------------------");
}

    public int GetSavedCount()
    {
        return _savedCount;
    }


    public int[] GetSavedKeys()
    {

    int[] keyCopy = new int[_savedKeys.Length];
    for (int i = 0; i < _savedKeys.Length; i++)
    {
        keyCopy[i] = _savedKeys[i];
    }
    return keyCopy;


    }

    public int[][] GetSavedDatabase()
    {
    int[][] dbCopy = new int[_savedDatabase.Length][];
        for (int i = 0; i < _savedDatabase.Length; i++)
        {
            dbCopy[i] = new int[_savedDatabase[i].Length];
            for (int j = 0; j < _savedDatabase[i].Length; j++)
            {
                dbCopy[i][j] = _savedDatabase[i][j];
            }
        }

    return dbCopy;
    }

}

class saveFile

{

    private int _savedCount;
    private int _versionID;

}

