using System.ComponentModel.DataAnnotations;

namespace InsertionSort;


public class SortEngine
{
    
    // holds our working array so we dont mess up the original data.
    public int[] Data {get; set;}

    public SortEngine(int[] arraydata)
    {
        //duplicate copy of the original array. 
        this.Data = NewArray(arraydata);
    }

    // Helper method to clone an array
    public int[] NewArray(int[] array)
    {
        int[] temporaryArray = new int[array.Length];
        for(int i = 0; i < array.Length; i++)
        {
            temporaryArray[i] = array[i];
        }
        return temporaryArray;
    }


    public int[] InsertionSort()
    {

        // Start at index 1 because a single element at index 0 is already "sorted"
        for(int i = 1; i < Data.Length; i++) {
            // Pick up the current item we want to insert into the sorted left side
            int key = Data[i]; 

            // Set pointer 'j' to look at the immediate left neighbor .. 
            int j = i - 1; 

            // Keep moving left as long as we're inside array bounds 
            // and the item at Data[j] is bigger than our key. ones j is -1. loop breaks.
            while(j >= 0 && Data[j] > key)
            {
                // Shift the larger number one spot to the right to make room
                Data[j+1] = Data[j];
                // Step backward to inspect the next element to the left
                j = j-1;
            }

            //'j' either stopped at a smaller number or dropped to -1.

            Data[j + 1] = key;

        }

    return Data;

    }
}

class Program
{
    static void Main(string[] args)
    {
        // Sample unsorted array data
        int[] unsortedNumbers = new int[50]
        {
        23, 7, 45, 12, 50, 3, 31, 18, 9, 39,
        4, 28, 14, 49, 1, 33, 20, 42, 6, 16,
        37, 11, 26, 48, 2, 30, 15, 41, 8, 22,
        35, 19, 44, 5, 27, 13, 38, 21, 47, 10,
        32, 17, 29, 46, 25, 34, 3, 40, 24, 36
        };

        // Initialise engine and run insertion sort
        SortEngine sortData = new SortEngine(unsortedNumbers);

        var result = sortData.InsertionSort();
        // Print results to console
        Display(unsortedNumbers, "Unsorted List");
        Console.WriteLine("\n \n");
        Display(result, "Sorted List");

    }

    // Formats and prints array elements on a single readable line
    public static void Display(int[] arraydata, string heading)
    {
        Console.WriteLine($"++++++++++++++[{heading}]+++++++++++++\n\n {string.Join(" | ", arraydata)}");

    }


}
