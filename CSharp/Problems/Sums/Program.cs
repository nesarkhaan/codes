namespace Sums;

class Program
{

    static int Sum(int x, int y)
    {

        return x + y;

    }

    static void Main(string[] args)
    {
        string[] input = Console.ReadLine().Split(' ');
        Console.WriteLine(Sum(Int32.Parse(input[0]), Int32.Parse(input[1])));
    }
}
