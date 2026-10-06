using System.Runtime.ConstrainedExecution;
using System.Security.Principal;

namespace Hashset;

class MyHashSet
{
    private int MAX_VALUE = 1000000;
    private int ARRAY_SIZE = 100; // Total number of available buckets

    // A list of lists to handle collisions (Separate Chaining)
    private List<List<int>> parentList;

    public MyHashSet()
    {
        // Initialize the parent list with a capacity of 100 buckets
        parentList = new List<List<int>>(ARRAY_SIZE);
        
        // Populate all 100 buckets with null placeholders to start empty
        for(int i = 0; i < ARRAY_SIZE; i++)
        {
            parentList.Add(null);
        }
    }

    public void Add(int key)
    {
        // calculate the bucket index (always 0 to 99). 
        int index = key % ARRAY_SIZE;

        // Grab the bucket list at that specific index
        List<int> childList = parentList.ElementAt(index);

        // Case 1: The bucket is empty, create the very first link in the chain
        if(childList == null)
        {
            List<int> list = new List<int>();
            list.Add(key);
            parentList[index] = list; 
        }
        // Case 2: A collision happened (bucket already exists)
        else
        {
            // Only append the key if it is not a duplicate (Set rule)
            if (!childList.Contains(key))
            {
                childList.Add(key);
            }
        }
    }

    public void Remove(int key)
    {
        // Calculate the bucket index where the key should live
        int index = key % ARRAY_SIZE;

        List<int> childList = parentList.ElementAt(index);

        // If the bucket exists, look inside and remove the specific key
        if(childList != null)
        {
            childList.Remove(key);
        }
    }

    public Boolean contains(int key)
    {
        // Jump straight to the expected index
        int index = key % ARRAY_SIZE;
        List<int> childList = parentList[index];

        // Return true if the bucket is not empty AND contains the key
        return childList != null && childList.Contains(key);
    }
}

class Program
{
    static void Main(string[] args)
    {
        MyHashSet hashSet = new MyHashSet();

        // Adding int data. 
        hashSet.Add(5);
        hashSet.Add(255);
        Console.WriteLine($"Contains 5? {hashSet.contains(5)}");     // Expected: True
        Console.WriteLine($"Contains 20? {hashSet.contains(20)}");   // Expected: False
        Console.WriteLine($"Contains 255? {hashSet.contains(255)}"); // Expected: True

        // --- Collision Testing ---
        // 105 and 5 share the same bucket (105 % 100 == 5)
        Console.WriteLine("\n Collision Testing - Adding 105. It should collide with index 5");
        hashSet.Add(105);
        Console.WriteLine($"Contains 5? {hashSet.contains(5)}");     // Expected: True
        Console.WriteLine($"Contains 105? {hashSet.contains(105)}"); // Expected: True
        
        // --- Removal Testing ---
        Console.WriteLine("\n Testing Remove - Removing 105");
        hashSet.Remove(105);
        Console.WriteLine($"Contains 105? {hashSet.contains(105)}"); // Expected: False
    }
}
