namespace SIMD_Parallel;

using System;
using System.Runtime.Intrinsics;



class Program
{
    static void Main(string[] args)
    {
        // Two spans of 8 integers
ReadOnlySpan<int> a = [10, 20, 30, 40, 50, 60, 70, 80];
ReadOnlySpan<int> b = [ 1,  2,  3,  4,  5,  6,  7,  8];



Vector256<int> vectorA = Vector256.Create(a);
Vector256<int> vectorB = Vector256.Create(b);


Vector256<int> vc = vectorA + vectorB;

Console.WriteLine(vc);

    }
}
