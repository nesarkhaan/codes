namespace SumsArrayPair;


class Program
{
    static int[] Sum(string[] array1, string[] array2)
    {
        int[] total = new int[array1.Length];
        for (int i = 0; i < array1.Length; i++)
        {

            int num1 = Int32.Parse(array1[i]);
            int num2 = Int32.Parse(array2[i]);
            total[i] = num1 + num2;
        }
        return total;

    }

    static void Main(string[] args)
    {
        string[] input1 = Console.ReadLine().Split(' ');
        string[] input2 = Console.ReadLine().Split(' ');
        int[] result = Sum(input1, input2);
        Console.Write("Parralel Sum is: ");
        for (int i = 0; i < result.Length; i++)
        {
            Console.Write(result[i] + " ");
        }
    }
}
