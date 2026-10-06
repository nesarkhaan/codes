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
