---
title: "My Code Snippets" 
author: Nes
date: "2025-10-27"
subject: "Programming"
keywords: [Programming, Code, Scripts, csharp]
lang: "en-AU" 
geometry: margin=1in
fontsize: 12pt
---

//Anonymous Function

() => { return ..... }


Merge sort over selection sort for large data 10 thousand otherwise selection sort is better.

heapsort with selection sort.


#C Sharp Snippets#


## Condition Statements

### Switch Cases

These are very handy. You can use it instead of if statement. You can provide several cases for different codes as well as the default condition. Example below is vowels and consonants
'''
switch(lane)
case "1":
	Statement;
	break
default:
	statement;
	break;
'''




#Tips 

- When writing code, ensure to break it down into functions. The main function should read like a story, one step after the other. 

- If code is repeated, place it in a function to reuse it. It ensures the code is clear.

- Breaking the functions into smaller functions, makes the code easier to read as well as helps with testing. If there are bugs found within the code, they can also be rectified easily. 

- If using set amounts, try to use global const variables, however there are side effects to this. 

##Loops  
All the different kind of Loops. 
###If-Function Loop
```
public static void CounterLoop(int num1, int num2) 
 { 
 if (num1 < num2)
 {
 Console.WriteLine(num1);
 CounterLoop(num1 + 1, num2);
 }
 }
 
 static void Main(string[] args)
 { 
 int number1 = 0; 
 int number2 = 30; 
 CounterLoop(number1, number2); 
 } 
 ```
 Data Structures  

Binary Tree


```
            Root 
            / \ 
          //   \\
     Child1    Child2
      / \        / \ 
    GC1 GC2     GC3 GC4
```
 
Mastering BST Removal: The "Easy" Cases (0 or 1 Child)

When removing a node from a Binary Search Tree, you always check its children first. If the node has zero children or only one child, we call this an "Easy Case."

Why is it easy? Because you don't need to find a Successor. You simply "bypass" the node you want to delete and reconnect the tree.

Case 0: The Leaf Node (Zero Children)

A leaf node sits at the very bottom of the tree. It has no left child and no right child.

The Initial Tree

Imagine we want to remove [20].
```
          [50] <-- Parent
         /    \
  --->[20]    [70]
     /    \      \
  (null) (null)  [80]
```

The Logic Steps

Identify Target: We found [20].

Check Children: Left is null. Right is null. It's a leaf!

Determine Replacement: Since there are no children to save, the "replacement" for [20] is simply null.

The Snip: We tell [50] (the Parent): "Whatever hand was holding [20], let go and hold null instead."

The Resulting Tree

The node [20] is dropped, and garbage collection will clean it up in memory.
```
          [50] 
         /    \
     (null)   [70]
                 \
                 [80]
```

Case 1: The Single Child Node

A node with one child acts like a single link in a chain. If you remove that link, you just connect the two pieces on either side of it together.

The Initial Tree

Imagine we want to remove [70].
```
          [50] <-- Parent
         /    \
      [20]   [70] <--- Target to delete
                 \
                 [80] <-- The only child
```

The Logic Steps

Identify Target: We found [70].

Check Children: Left is null. Right is [80]. It has exactly one child!

Determine Replacement: The replacement for [70] is its only child, [80].

The Bypass: We tell [50] (the Parent): "Whatever hand was holding [70], let go and grab [80] instead."

The Resulting Tree

The node [70] is entirely bypassed. [50] now connects directly to [80]. The tree structure remains perfectly valid!
```
          [50] 
         /    \
      [20]    [80] 
```

The "Root" Exception

There is one special scenario in both of these cases. What if the node you are deleting is the very top of the tree (the Root)?

If you delete the Root, there is no Parent. (Parent == null).

If the Root is a Leaf (Case 0): The whole tree becomes null.

If the Root has One Child (Case 1): That single child gets promoted to become the brand new Root of the entire tree.

The Code Logic Concept

In both Case 0 and Case 1, your code only needs to ask two questions:

What is the Replacement? (Answer: Whichever child is NOT null. If both are null, the replacement is null).

Where does the Replacement go? (Answer: Attach it to the Parent. If there is no Parent, it becomes the new Root).


#### Removal ####

* Removal in Binary Search Tree involves finding the correct node for removal. Storing the value 

Mastering BST Removal: The "Two-Child" Case

When you remove a node with zero children (a leaf) or one child, it's easy: you just cut the string and tie it to the next thing down.

But when a node has two children, you can't just delete it. If you delete the root, what happens to the left and right branches? They fall apart.

To fix this, we use a strategy called "The Stand-In" (or finding the In-Order Successor).

The Initial Tree

Let's imagine a tree where we want to remove the Root (50).

```
          [50] <--- (Current: Target to delete)
         /    \
      [20]    [70]
             /    \
          [60]    [80]
          /  \
       [55]  [65]

```
Why is this hard?

If we just delete [50], we have two floating subtrees ([20] and [70]). We need a New King to take the [50] spot.

The Rule: The New King MUST be greater than everything on the left ([20]), and less than everything on the right ([70], [60], etc.).

Step 1: Find the Successor (The "New King")

The only logical replacement is the smallest number in the right branch.
How do we find it logically in code?

Take one step to the Right.

Walk as far Left as possible until you hit a dead end.

Let's trace it:

Go Right from [50] -> You are at [70].

Go Left -> You are at [60].

Go Left -> You are at [55].

Try to go Left -> It's null. You found the Successor!
```
          [50] <-- Current
         /    \
      [20]    [70]
             /    \
          [60]    [80]
          /  \
  Succ->[55] [65]
```

In your code logic:
You create a successor variable that points to [55], and a successorParent variable that points to [60].

Step 2: The "Identity Theft" (Swap the Data)

Instead of moving the actual [55] node object all the way to the top (which means rewriting tons of Left and Right pointers), we just copy the data.

We overwrite the Target's Key and Value with the Successor's Key and Value.
```
          [55] <-- 50 is gone! Data is overwritten.
         /    \
      [20]    [70]
             /    \
          [60]    [80]
          /  \
        [55] [65]  <-- Wait, we have two 55s now!

```
In your code logic:
Current.Key = successor.Key;

Step 3: The Cleanup (Snip the old node)

We successfully updated the top of the tree, but we left the old "Stand-In" node at the bottom. We must delete it.

The Golden Rule of the Successor: Because we went as far Left as possible to find it, the Successor is guaranteed to have NO Left child. It might have a Right child, or no children at all.

This means deleting the old successor is an "Easy Case" removal!

We tell the successorParent ([60]) to bypass [55] and point to whatever is on [55]'s right side (in this case, null).
```
          [55] 
         /    \
      [20]    [70]
             /    \
          [60]    [80]
          /  \
     (null)  [65]  <-- The old 55 is snipped away!
```

In your code logic:
Since the old [55] was the Left child of [60], we do:
successorParent.Left = successor.Right;

Summary of the Logic

Target has two children? Find the Successor.

Successor = Right once, then Left-Left-Left.

Overwrite Target's Data with Successor's Data.

Tell Successor's Parent to bypass the Successor.
 
 
##Sorting Algorithms ##  
###Quick Sort###
```cs
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
```
 
##Files and Data##  
#### File Write and Load ####  
```cs
class Program  
{  
 static private int[]? loc;  
 static private string[][]? array;  
 static private int Count;  
 static void textWrite(string text1, string text2)  
   
   
 {  
   
 //name of the file  
 string filename = "data.csv";  
 //Sub directory  
 string path1 = @"data/";  
   
 //Getting full path  
 string fullpath = System.IO.Path.Combine(System.IO.Path.GetFullPath(path1), filename);  
   
 //Declaring writer object  
 System.IO.StreamWriter writer;  
 //Initializing  
 writer = new System.IO.StreamWriter(fullpath);  
  
  
 writer.WriteLine(text1);  
 writer.WriteLine(text2);  
 //Flush() and close() file to save memory.   
 Console.WriteLine("Success");  
 //Close and flush writer so data can be transferred from buffer to the file.  
 writer.Close();  
  
  
  
 }  
  
 static void arrayWrite()  
 {  
 Count = 5;  
 loc = new int[]{0, 1, 2, 3, 4};  
 array = new string[Count][];  
 array[0] = ["Ajmal", "Emal", "Ahmad", "Qomandan", "Toofan"];  
 array[1] = ["Ben", "mark", "ahmad", "megan", "lala", "mikhael"];  
 array[2] = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];  
 array[3] = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];  
 array[4] = ["Summer", "Winter", "Autumn", "Spring"];  
  
   
  //name of the file  
 string filename = "data.csv";  
   
 //Sub directory  
 string path1 = @"data/";  
   
 //Getting full path  
 string fullpath = System.IO.Path.Combine(System.IO.Path.GetFullPath(path1), filename);  
   
 //Declaring writer object  
 System.IO.StreamWriter writer;  
 //Initializing  
 writer = new System.IO.StreamWriter(fullpath);  
  
 for(int i = 0; i < loc.Length; i++)  
 {  
 writer.Write($"{loc[i]}"); Console.WriteLine($"Writing: LOC. {loc[i]}");  
 for (int j = 0; j < array[i].Length; j++)  
 {  
 writer.Write($",{array[i][j]}");  
 }  
 writer.Write("\n");  
 }  
 writer.Close();  
 }  
  
 static int[] ConvertToInt(string[] parts){  
   
 int[] Parts = new int[parts.Length];  
  
 for (int i = 0; i < parts.Length; i++)  
 {  
 Parts[i] = Convert.ToInt32(parts[i]);  
 }  
  
 return Parts;  
  
 }  
  
 static void LoadFile(string file){  
   
 string line = null;  
 int lineCounter = 0;  
 int[] LoadKeys = new int[100];  
 string[][] LoadDatabase = new string[100][];  
  
 string filePath = $"./data/{file}";  
 System.IO.StreamReader reader;  
 reader = new System.IO.StreamReader(filePath);  
  
  
 while ((line = reader.ReadLine()) != null){  
   
 string[] Parts = line.Split(",");  
 int[] PartsConvertedToInt = ConvertToInt(Parts);  
 LoadKeys[lineCounter] = PartsConvertedToInt[0];  
 LoadDatabase[lineCounter] = new string[Parts.Length - 1];  
   
 for(int i = 1; i < Parts.Length; i++){  
 LoadDatabase[lineCounter][i - 1] = PartsConvertedToInt[i];  
  }  
 lineCounter++;  
  
 }  
 loc = LoadKeys;  
 array = LoadDatabase;  
 reader.Close();  
   
 for(int i = 0; i < lineCounter; i++)  
 {  
 Console.Write($"{loc[i]}");  
 for (int j = 0; j < array[i].Length; j++)  
 {  
 Console.Write($",{array[i][j]}");  
 }  
 Console.Write("\n");  
 }  
  
 }  
 static void Main(string[] args)  
 {  
 //Console.Write("Text1: ");  
 //string Input1 = Console.ReadLine();  
 //string Input2 = Console.ReadLine();  
  
 //textWrite(Input1, Input2);  
 //arrayWrite();  
 string FileName = Console.ReadLine();  
 LoadFile(FileName);  
  
   
 // to load arrayWrite();  
 }  
   
}  
  
```
 
## All Other Projects ##  

### Binary Tree OOP ###
```csharp
using System;

// ==============================================================================
// 1. THE NODE CLASS
// Represents a single "box" in the tree, holding a Key, a Value, and pointers.
// ==============================================================================
public class BinaryTreeNode<K, V>
{
    public K Key { get; set; }
    public V Value { get; set; }
    public BinaryTreeNode<K, V>? Left { get; set; }
    public BinaryTreeNode<K, V>? Right { get; set; }

    public BinaryTreeNode(K key, V value)
    {
        Key = key;
        Value = value;
        Left = null;
        Right = null;
    }
}

// ==============================================================================
// 2. THE BINARY SEARCH TREE CLASS
// Manages the nodes and enforces the Left-is-Smaller, Right-is-Larger rules.
// ==============================================================================
public class BinarySearchTree<K, V>
{
    private BinaryTreeNode<K, V>? root;
    private int count;
    
    // A delegate (function) used to compare two keys.
    // Returns 0 if equal, < 0 if key1 < key2, and > 0 if key1 > key2.
    private readonly Func<K, K, int> comparator;

    public BinarySearchTree(Func<K, K, int> customComparator)
    {
        root = null;
        count = 0;
        comparator = customComparator;
    }

    /// <summary>
    /// Returns the total number of nodes in the tree in O(1) time.
    /// </summary>
    public int Size()
    {
        return count;
    }

    /// <summary>
    /// Inserts a new Key/Value pair into the correct sorted position.
    /// Updates the value if the key already exists.
    /// </summary>
    public void Insert(K key, V value)
    {
        if (root == null)
        {
            root = new BinaryTreeNode<K, V>(key, value);
            count++;
            return;
        }

        BinaryTreeNode<K, V> Current = root;
        BinaryTreeNode<K, V>? Parent = null;

        while (Current != null)
        {
            int ComparedKeyResult = comparator(key, Current.Key);

            if (ComparedKeyResult == 0)
            {
                // Key already exists, update the value and exit
                Current.Value = value;
                return;
            }
            else if (ComparedKeyResult < 0)
            {
                Parent = Current;
                Current = Current.Left;
            }
            else
            {
                Parent = Current;
                Current = Current.Right;
            }
        }

        // We hit a dead end, which means we found the spot to insert!
        BinaryTreeNode<K, V> newNode = new BinaryTreeNode<K, V>(key, value);
        
        // Use the final parent to figure out which side to attach the new node
        if (comparator(key, Parent.Key) < 0)
        {
            Parent.Left = newNode;
        }
        else
        {
            Parent.Right = newNode;
        }
        
        count++; // Successfully added a new node
    }

    /// <summary>
    /// Searches for a specific key and returns its associated value.
    /// </summary>
    public V? Get(K key)
    {
        BinaryTreeNode<K, V> Current = root;

        while (Current != null)
        {
            int ComparedKeyResult = comparator(key, Current.Key);

            if (ComparedKeyResult == 0)
            {
                return Current.Value; // Target found!
            }
            else if (ComparedKeyResult < 0)
            {
                Current = Current.Left;
            }
            else
            {
                Current = Current.Right;
            }
        }

        return default(V); // Target not found
    }

    /// <summary>
    /// Removes a node with the specified key. Handles all 3 deletion cases.
    /// </summary>
    public V? Remove(K key)
    {
        BinaryTreeNode<K, V> Current = root;
        BinaryTreeNode<K, V>? Parent = null;

        if (root == null) return default(V);

        while (Current != null)
        {
            int ComparedKeyResult = comparator(key, Current.Key);

            if (ComparedKeyResult == 0)
            {
                V DeletedValue = Current.Value; // Save the treasure before surgery

                // CASE 1: Leaf Node (0 Children)
                if (Current.Left == null && Current.Right == null)
                {
                    if (Parent == null) root = null;
                    else if (Parent.Left == Current) Parent.Left = null;
                    else Parent.Right = null;
                    
                    count--;
                    return DeletedValue;
                }

                // CASE 2: Exactly 1 Child
                else if (Current.Left == null || Current.Right == null)
                {
                    BinaryTreeNode<K, V> grandchild = (Current.Left != null) ? Current.Left : Current.Right;

                    if (Parent == null) root = grandchild;
                    else if (Parent.Left == Current) Parent.Left = grandchild;
                    else Parent.Right = grandchild;

                    count--;
                    return DeletedValue;
                }

                // CASE 3: 2 Children
                else
                {
                    BinaryTreeNode<K, V> SuccessorParent = Current;
                    BinaryTreeNode<K, V> Successor = Current.Right;

                    // Find the In-Order Successor (Smallest node on the right)
                    while (Successor.Left != null)
                    {
                        SuccessorParent = Successor;
                        Successor = Successor.Left;
                    }

                    // Overwrite data
                    Current.Key = Successor.Key;
                    Current.Value = Successor.Value;

                    // Detach the physical Successor node
                    if (SuccessorParent.Left == Successor)
                    {
                        SuccessorParent.Left = Successor.Right;
                    }
                    else
                    {
                        SuccessorParent.Right = Successor.Right;
                    }

                    count--;
                    return DeletedValue;
                }
            }
            else if (ComparedKeyResult < 0)
            {
                Parent = Current;
                Current = Current.Left;
            }
            else
            {
                Parent = Current;
                Current = Current.Right;
            }
        }

        return default(V);
    }
}


```

### Minimum of Two ###  

```csharp
namespace MinimumOfTwo;  
//This Program takes two inputs of numbers and places them in seperate arrays. It compares array1 index to array2 index to find the larger number. The larger number is placed into a temporary array which is returned.   
class Program  
{  
 static int[] CompareTwoArrays(string[]? array1, string[]? array2)  
 {  
 int GetLength = array1.Length > array2.Length ? array1.Length : array2.Length;  
 int[] result = new int[GetLength];  
  
 for (int i = 0; i < GetLength; i++)  
 {  
  
 if (Int32.Parse(array1[i]) > Int32.Parse(array2[i]))  
 {  
 result[i] = Int32.Parse(array1[i]);  
  
 }  
 else  
 {  
 result[i] = Int32.Parse(array2[i]);  
  
 }  
  
 }  
 return result;  
  
 }  
  
 static void Main(string[] args)  
 {  
 string[] delimiter = new string[] { " ", "," };  
 Console.Write("Enter Input1: ");  
 string[]? input1 = Console.ReadLine().Split(delimiter, StringSplitOptions.None);  
 Console.Write("Enter Input2: ");  
 string[]? input2 = Console.ReadLine().Split(delimiter, StringSplitOptions.None);  
  
 int[] result = CompareTwoArrays(input1, input2);  
 Console.WriteLine("Results: ");  
 for (int i = 0; i < result.Length; i++)  
 {  
  
 Console.Write(result[i] + " ");  
  
 }  
  
  
 }  
}  
```


### Switch Case Statement

'''csharp
namespace Test3;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            string userinput = Console.ReadLine();



            switch (userinput.ToLower())
            {

                case "a":
                    Console.WriteLine($"{userinput} is a vowel");
                    break;
                case "e":
                    Console.WriteLine($"{userinput} is a vowel");
                    break;
                case "i":
                    Console.WriteLine($"{userinput} is a vowel");
                    break;

                case "o":
                    Console.WriteLine($"{userinput} is a vowel");
                    break;
                case "u":
                    Console.WriteLine($"{userinput} is a vowel");
                    break;
                default:
                    if (userinput.Length > 1 || userinput.Length <= 0)
                    {
                        Console.WriteLine("Please enter correct character");


'''









































































