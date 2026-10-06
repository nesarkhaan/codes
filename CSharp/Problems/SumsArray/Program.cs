namespace SumsArray;

class Program
{
    static int Sum(string[] array)
    {
        int total = 0;
        for (int i = 0; i < array.Length; i++)
        {

            int num = Int32.Parse(array[i]);
            total = num + total;
        }
        return total;

    }

    static void Main(string[] args)
    {
        string[] input = Console.ReadLine().Split(' ');
        int result = Sum(input);
        Console.WriteLine($"Sum is: {result}");

    }
}
