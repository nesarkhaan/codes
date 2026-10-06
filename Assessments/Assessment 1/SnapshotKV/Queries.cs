namespace SnapshotKV;


public partial class OpQueryDB
{
    //Lists the keys currently in the Keys Array
    public SnapshotQueryResult ListKeys()
    {
        int count = DB.GetKeyCount();
        //Create the exact-sized array
        int[] activeKeys = new int[count];

        if (activeKeys.Length == 0 || DB.GetKeysArray().Length < 0)
        {
            return new MyQueryResult("ListKeys", new int[0], null, false);
        }

        //Iterate over the keys array
        for (int i = 0; i < count; i++)
        {
            activeKeys[i] = DB.GetKeysArray()[i];
        }
        return new MyQueryResult("ListKeys", activeKeys, null, true);

    }



    //GetEntry - Gets a copy of the database entry based on the searched key
    public SnapshotQueryResult GetEntry(int key)
    {
        int index = -1;
        //findKey(key) finds and validates key before returning it.
        index = DB.findKey(key);

        //if key is found, then get data else return false. 
        if (index != -1)
        {
            int[] getData = DB.GetDatabaseArray()[index];
            return new MyQueryResult("GetEntry", getData, key, true);
        }
        else
        {
            return new MyQueryResult("GetEntry", new int[0], key, false);
        }
    }


    //SetEntry - It sets a new key entry and adds values to the corresponding database. If key exists, it replaces the data in the database. If it doesn't exist, it creates a new entry. 
    public SnapshotQueryResult SetEntry(int key, int[] data)
    {
        int index = -1;
        //findKey(key) method finds and validates keys before returning it. 
        index = DB.findKey(key);

        //Key does not exist
        if (index == -1)
        {
            //If the Key counter is the same then extend the Keys Array
            if (DB.GetKeyCount() >= DB.GetKeysArray().Length)
            {
                DB.arrayExtend(DB.GetKeysArray().Length + 1);
            }

            DB.GetKeysArray()[DB.GetKeyCount()] = key;
            DB.GetDatabaseArray()[DB.GetKeyCount()] = data;
            DB.IncrementKeyCount(); // Increment AFTER saving

            return new MyQueryResult("SetEntry", data, key, true);
        }
        // If there is an existing key, then write over it. 
        else if (DB.GetKeysArray()[index] == key)
        {
            DB.GetDatabaseArray()[index] = data;
            return new MyQueryResult("SetEntry", data, key, true);
        }
        else
        {
            return new MyQueryResult("SetEntry", new int[0], key, false);
        }
    }

    //Removes a key and the corresponding database entry
    public SnapshotQueryResult RemoveEntry(int key)
    {
        // Make two new arrays smaller than the last
        int index = -1;
        index = DB.findKey(key);

        if (index == -1)
        {
            return new MyQueryResult("RemoveEntry", new int[0], key, true);
        }

        for (int i = index; i < DB.GetKeyCount() - 1; i++)
        {
            DB.GetKeysArray()[i] = DB.GetKeysArray()[i + 1];
            DB.GetDatabaseArray()[i] = DB.GetDatabaseArray()[i + 1];
        }

        DB.DecrementKeyCount();
        return new MyQueryResult("RemoveEntry", new int[0], key, true);
    }

    //Push
    public SnapshotQueryResult Push(int key, int[] data)
    {
        int index = -1;
        index = DB.findKey(key);
        if (index == -1) // NEW KEY if no existing key found
        {
            if (DB.GetKeyCount() >= DB.GetKeysArray().Length)
            {
                DB.arrayExtend(DB.GetKeysArray().Length * 2);
            }

            DB.GetKeysArray()[DB.GetKeyCount()] = key;
            DB.GetDatabaseArray()[DB.GetKeyCount()] = data;
            DB.IncrementKeyCount(); // Increment AFTER saving

            return new MyQueryResult("Push", data, key, true);
        }
        else // if EXISTING KEY is found
        {
            int[] newData = new int[DB.GetDatabaseArray()[index].Length + data.Length];
            for (int i = 0; i < data.Length; i++)
                newData[i] = data[i];


            for (int j = 0; j < DB.GetDatabaseArray()[index].Length; j++)
                newData[j + data.Length] = DB.GetDatabaseArray()[index][j];

            DB.GetDatabaseArray()[index] = newData;
            return new MyQueryResult("Push", newData, key, true);
        }
    }


    // Append
    public SnapshotQueryResult Append(int key, int[] data)
    {
        int index = -1;
        index = DB.findKey(key);

        if (index == -1) //  NEW KEY if no existing key found
        {
            if (DB.GetKeyCount() >= DB.GetKeysArray().Length)
            {
                DB.arrayExtend(DB.GetKeysArray().Length + 1);
            }

            DB.GetKeysArray()[DB.GetKeyCount()] = key;
            DB.GetDatabaseArray()[DB.GetKeyCount()] = data;
            DB.IncrementKeyCount(); // Increment AFTER saving

            return new MyQueryResult("Append", data, key, true);
        }
        else // If there is EXISTING KEY
        {
            int[] newData = new int[DB.GetDatabaseArray()[index].Length + data.Length];
            for (int i = 0; i < DB.GetDatabaseArray()[index].Length; i++)
                newData[i] = DB.GetDatabaseArray()[index][i];

            for (int j = 0; j < data.Length; j++)
                newData[j + DB.GetDatabaseArray()[index].Length] = data[j];

            DB.GetDatabaseArray()[index] = newData;
            return new MyQueryResult("Append", newData, key, true);
        }
    }

    //View
    public SnapshotQueryResult View(int key, int startIndex, int endIndex)
    {
        int[] result = new int[(endIndex - startIndex) + 1];
        int index = -1;
        index = DB.findKey(key);
        if (index != -1)
        {
            for (int j = startIndex; j <= endIndex; j++)
            {
                result[j - startIndex] = DB.GetDatabaseArray()[index][j];
            }
            return new MyQueryResult("View", result, key, true);
        }
        return new MyQueryResult("View", new int[0], key, false);
    }


    // MakeWatcher 

    public SnapshotQueryResult MakeWatcher(int key, int startIndex, int endIndex)
    {

        int[] result = new int[] { DB.GetWatcherCount() };

        int index = -1;
        index = DB.findKey(key);

        //WatcherKeys, WatcherStart and WatcherEnds array are set to value 5. Method below extends the array.  
        if (DB.GetWatcherKey().Length - 1 == DB.GetWatcherCount())
        {
            DB.ExtendWatcherArrays(DB.GetWatcherKey().Length + 1);
        }

        // Input Validation. 
        if (startIndex > DB.GetDatabaseArray()[index].Length || startIndex < 0 || endIndex > DB.GetDatabaseArray()[index].Length || endIndex < 0)
        {
            Console.WriteLine($"Please select a range between index 0 and index {DB.GetDatabaseArray()[index].Length} for Key {DB.GetKeysArray()[index]}");
            return new MyQueryResult("MakeWatcher", new int[0], key, false);
        }


        if (index != -1)
        {

            result[0] = DB.GetWatcherCount();
            DB.GetWatcherKey()[DB.GetWatcherCount()] = key;
            DB.GetWatcherStarts()[DB.GetWatcherCount()] = startIndex;
            DB.GetWatcherEnds()[DB.GetWatcherCount()] = endIndex;
            DB.IncrementWatcherCount(); //Increment up to 1.

            // when the if condition is met
            return new MyQueryResult("MakeWatcher", result, key, true);

        }
        else
        {

            // When the condition is not met - return false. 
            return new MyQueryResult("MakeWatcher", result, key, false);
        }
    }

    //ViewWatcher

    public SnapshotQueryResult ViewWatcher(int watcherKey)
    {
        int Key = DB.GetWatcherKey()[watcherKey];
        int start = DB.GetWatcherStarts()[watcherKey];
        int end = DB.GetWatcherEnds()[watcherKey];
        int[] result = new int[(end - start) + 1]; int index = -1;

        index = DB.findKey(Key); //findKey in the key database
        int size = (end - start) + 1;
        if (index != -1)
        {
            //int size = (end - start) + 1;
            for (int j = 0; j < size; j++)
                result[j] = DB.GetDatabaseArray()[index][start + j];
            //return true when the if condition is met. 
            return new MyQueryResult("ViewWatcher", result, Key, true);
        }
        else
        {
            // otherwise Return false
            return new MyQueryResult("ViewWatcher", new int[0], -1, false);
        }
    }

    //Sort 

    public SnapshotQueryResult Sort(int key)
    {
        // Setting a new array and giving it a value of 0. 
        int[] result = new int[0];

        //Checking if key matches the Keys[] array
        int index = -1;
        index = DB.findKey(key);
        // result array size changes to Database based on the index key which is then copied into result array. 
        result = new int[DB.GetDatabaseArray()[index].Length];
        for (int j = 0; j < result.Length; j++)
            result[j] = DB.GetDatabaseArray()[index][j];

        //if index not out of range then bubble loop is used to sort data
        if (index != -1)
        {
            //Sorting
            for (int i = result.Length - 1; i > 0; i--)
            {   //first loop moving down
                for (int j = 0; i > j; j++)
                {      //second loop moving up
                    if (result[j] >= result[j + 1])
                    {
                        int exchange = result[j];
                        result[j] = result[j + 1];
                        result[j + 1] = exchange;
                    }
                }
            }
            //if succesfull
            DB.GetDatabaseArray()[index] = result;
            return new MyQueryResult("Sort", DB.GetDatabaseArray()[index], key, true);
        }
        //if key doesn't exist or not successful
        return new MyQueryResult("Sort", new int[0], -1, false);
    }

    //Partial Sort
    public SnapshotQueryResult PartialSort(int key, int startIndex, int endIndex)
    {
        int[] result = new int[0]; int index = -1;

        index = DB.findKey(key);
        // Iterating the database into result[] for sorting 
        result = new int[DB.GetDatabaseArray()[index].Length];
        for (int j = 0; j < result.Length; j++)
            result[j] = DB.GetDatabaseArray()[index][j];

        //if index not out of range then bubble loop is used to sort data
        if (index != -1)

        {
            for (int i = startIndex - 1; i < endIndex; i++)
                //first loop moving down
                for (int j = startIndex; j < endIndex; j++)
                    //second loop moving up
                    if (result[j] > result[j + 1])
                    {
                        int exchange = result[j];
                        result[j] = result[j + 1];
                        result[j + 1] = exchange;
                    }
            //if succesfull
            DB.GetDatabaseArray()[index] = result;
            return new MyQueryResult("PartialSort", DB.GetDatabaseArray()[index], key, true);
        }
        //if key doesn't exist or not successful
        return new MyQueryResult("PartialSort", new int[0], -1, true);
    }


    //Union between two keys
    public SnapshotQueryResult Union(int key1, int key2)
    {
        int index1 = -1;
        int index2 = -1;
        index1 = DB.findKey(key1);
        index2 = DB.findKey(key2);

        if (index1 != -1 || index2 != -1)
        {
            int[] newData = new int[DB.GetDatabaseArray()[index1].Length + DB.GetDatabaseArray()[index2].Length];
            //copying data from 1st key. This could be made into a method as the code is used twice. 
            for (int i = 0; i < DB.GetDatabaseArray()[index1].Length; i++)
                newData[i] = DB.GetDatabaseArray()[index1][i];

            //copying data from 2nd Database into array newData
            for (int j = 0; j < DB.GetDatabaseArray()[index2].Length; j++)
                newData[j + DB.GetDatabaseArray()[index1].Length] = DB.GetDatabaseArray()[index2][j];



            //Sorting new data

            for (int i = newData.Length - 1; i > 0; i--) //if i is more than 0 then increment down
                                                         //Increment down
                for (int j = 0; i > j; j++) //increment up     
                    if (newData[j] > newData[j + 1])
                    {
                        int exchange = newData[j];
                        newData[j] = newData[j + 1];
                        newData[j + 1] = exchange;
                    }
            //newData has no data, return false. 
            if (newData.Length == 0)
                return new MyQueryResult("Union", new int[0], key1, false);

            //Remove duplicates
            int tempCount = 1;
            for (int i = 1; i < newData.Length; i++)
                if (newData[i] != newData[i - 1])
                {
                    newData[tempCount] = newData[i]; tempCount++;
                }

            //copies the data onto a result array to remove remaining duplicates currently at the end of newData
            int[] result = new int[tempCount];
            for (int i = 0; i < tempCount; i++)
                result[i] = newData[i];

            return new MyQueryResult("Union", result, key1, true);
        }

        return new MyQueryResult("Union", new int[0], -1, false);
    }




    //Intersect 
    public SnapshotQueryResult Intersect(int key1, int key2)
    {

        int index1 = -1;
        int index2 = -1;
        //Finding Keys in the key database for both Key1 and Key2
        index1 = DB.findKey(key1);
        index2 = DB.findKey(key2);

        //if KeyIndex1 or KeyIndex returns -1 - Return false.
        if (index1 != -1 || index2 != -1)
        {
            //Setting up a new array with the size of the databases corresponding to the key
            int[] newData = new int[DB.GetDatabaseArray()[index1].Length + DB.GetDatabaseArray()[index2].Length];

            //Iterate and copy Key1
            for (int i = 0; i < DB.GetDatabaseArray()[index1].Length; i++)
                newData[i] = DB.GetDatabaseArray()[index1][i];


            //copying data from 2nd key, picking up from index position from key1 database ended. 
            for (int j = 0; j < DB.GetDatabaseArray()[index2].Length; j++)
                newData[j + DB.GetDatabaseArray()[index1].Length] = DB.GetDatabaseArray()[index2][j];

            //Sorting the combined data. 

            for (int i = newData.Length - 1; i > 0; i--) //if i is more than 0 then increment down
            { //first loop moving down
                for (int j = 0; i > j; j++)
                {       //Sorting and swapping/shuffling.  
                    if (newData[j] > newData[j + 1])
                    {
                        int exchange = newData[j];
                        newData[j] = newData[j + 1];
                        newData[j + 1] = exchange;
                    }
                }
            }

            if (newData.Length == 0) { return new MyQueryResult("Union", new int[0], key1, false); }


            //Removing Duplicates
            int tempCount = 0;
            int[] tempResult = new int[newData.Length + 2];
            for (int i = 1; i < newData.Length; i++)
            {
                //Comparison 
                if (newData[i] == newData[i - 1])
                {
                    if (tempCount == 0 || newData[i] != tempResult[tempCount - 1])
                    {
                        //Placing the duplicates in array tempResult
                        tempResult[tempCount] = newData[i]; tempCount++;
                    }
                }
            }


            // iterates the new array and saves the result to result, tempResult could be returned but there are a lot of 0s that fails the test. 
            int[] result = new int[tempCount];

            for (int i = 0; i < tempCount; i++)
            {
                result[i] = tempResult[i];
            }
            return new MyQueryResult("Intersect", result, key1, true);
        }
        return new MyQueryResult("Intersect", new int[0], key1, false);
    }


}
