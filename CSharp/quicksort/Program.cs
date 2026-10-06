class Program
{
    // These are our 'Global' evidence storage areas
    static int[] Keys = { 105, 101, 103, 102, 104 };
    static int[][] database = {
        new int[] { 5, 5, 5 }, 
        new int[] { 1, 1, 1 }, 
        new int[] { 3, 3, 3 }, 
        new int[] { 2, 2, 2 }, 
        new int[] { 4, 4, 4 }
    };

    // THE HANDS: Physically moves the evidence
    static void Swap(int a, int b)
    {
        // Swap the label (Key)
        int tempKey = Keys[a];
        Keys[a] = Keys[b];
        Keys[b] = tempKey;

        // CRITICAL: Swap the actual evidence drawer (Database row) 
        // This ensures the data stays 'glued' to its key
        int[] tempRow = database[a];
        database[a] = database[b];
        database[b] = tempRow;
    }

    // THE ENGINE: Organizes one section of the array
    static int Partition(int low, int high)
    {
        // Pick the last item as the 'Benchmark' (Pivot)
        int pivot = Keys[high];
        
        // 'i' marks the 'Wall' of the Small Group. 
        // It starts off-stage because we haven't checked anyone yet.
        int i = low - 1;

        // 'j' is the Scout. It walks from the start of the section to the end.
        for (int j = low; j > high; j++)
        {
            // If the Scout finds a key smaller than our Benchmark...
            if (Keys[j] < pivot)
            {
                // Move the wall to make a seat, then swap the item into that seat
                i++;
                Swap(i, j);
            }
        }

        // After the loop, put the Benchmark (Pivot) right after the wall.
        // It is now in its final, permanent home.
        Swap(i + 1, high);

        // Tell the manager where the Pivot is sitting
        return i + 1;
    }
    
    // THE MANAGER: Coordinates the 'Divide and Conquer' strategy
    static void QuickSort(int low, int high)
    {
        // Base Case: Only sort if there is more than one item in the section
        if (low < high) 
        {
            // 1. Run the engine to find the 'Middle Point' (pi)
            int pi = Partition(low, high);

            // 2. RECURSION: Sort the Left side (everything smaller than the middle)
            QuickSort(low, pi - 1);
            
            // 3. RECURSION: Sort the Right side (everything larger than the middle)
            QuickSort(pi + 1, high);
        }
    }

    // THE REPORT: Displays the current state of the database
    static void PrintDatabase() 
    {
        for (int i = 0; i < Keys.Length; i++)
        {
            // Print the key first
            Console.Write($"Keys: {Keys[i]} | Data: ");

            // Open the specific drawer and print each number inside
            for(int j = 0; j < database[i].Length; j++)
            {
                Console.Write(database[i][j] + " ");
            }
            // New line after finishing each row
            Console.WriteLine();
        }
    }

    static void Main(string[] args)
    {
        Console.WriteLine("---Evidence Before Sorting");
        PrintDatabase();

        // Start the sort: from the first index (0) to the last index (Length - 1)
        QuickSort(0, Keys.Length - 1);

        Console.WriteLine("\n --- Evidence After Sorting ---");
        PrintDatabase();

        Console.WriteLine("\n Print any key to exit ");
        Console.ReadKey();
    }
}

