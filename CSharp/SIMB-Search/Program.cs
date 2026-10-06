namespace SIMB_Search;

using System;
using System.Numerics;
using System.Runtime.Intrinsics;

class Program
{
    static void Main(string[] args)
    {
        int[] data = IntegerData(200);
        int target = 190; // Located near the end (index 95)

        int foundIndex = SimdSearch(data, target);



        Console.WriteLine(string.Join(" | ", data));
    }



    static int[] IntegerData(int amount)
    {
        
        int[] temp = new int[amount];

        for(int i = 1; i < amount; i++)
        {
            temp[i] = i;

        }
        return temp;

    }

  public static int SimdSearch(ReadOnlySpan<int> data, int target)
    {
        // STEP 1: Broadcast target across all 8 lanes of a 256-bit vector
        // Register = [190, 190, 190, 190, 190, 190, 190, 190]
        Vector256<int> vTarget = Vector256.Create(target);

        int i = 0;
        int vectorSize = Vector256<int>.Count; // 8 elements (256 bits / 32 bits)

        // STEP 2: Main Vector Loop (Processes 8 elements per iteration)
        // For 100 items: Runs from i = 0 up to i = 96 (12 iterations total)
        while (i <= data.Length - vectorSize)
        {
            // Load 8 integers from memory into a vector register
            Vector256<int> vData = Vector256.Create(data.Slice(i, vectorSize));

            // Compare all 8 elements against vTarget in parallel
            // Matching lanes become 0xFFFFFFFF, non-matching lanes become 0x00000000
            Vector256<int> vMatches = Vector256.Equals(vData, vTarget);

            // Extract the top bit of each lane into an 8-bit scalar mask (e.g., 0b10000000)
            uint mask = vMatches.ExtractMostSignificantBits();

            // Branchless check: if mask != 0, at least one lane matched
            if (mask != 0)
            {
                // Count trailing zeros gives the exact lane index (0 through 7)
                int laneOffset = BitOperations.TrailingZeroCount(mask);
                
                Console.WriteLine($"[SIMD MATCH] Found in chunk starting at index {i}, lane offset {laneOffset}");
                return i + laneOffset;
            }

            i += vectorSize; // Jump ahead 8 elements
        }

        // STEP 3: Scalar Cleanup Loop
        // Handles remaining elements when data.Length is not a multiple of 8
        // For 100 items: Handles indices 96, 97, 98, and 99
        Console.WriteLine($"[CLEANUP] SIMD finished 96 items. Running scalar loop for indices {i}..{data.Length - 1}");
        
        for (; i < data.Length; i++)
        {
            if (data[i] == target)
                return i;
        }

        return -1; // Target not found
    }
    
    
 }
