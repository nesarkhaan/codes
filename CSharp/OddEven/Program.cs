namespace Test2;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            string UserInput = Console.ReadLine();
            int num = int.Parse(UserInput);
            int ProcessedNum = num % 2;

            if (ProcessedNum == 0)
            {

                Console.WriteLine($"{num} is an even number");
            }
            else
            {

                Console.WriteLine($"{num} is an odd number");
            }





        }
    }
}
