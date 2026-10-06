namespace Loops;

class Program
{
    static void Diamond()
    {
        for (int i = 1; i < 10; i++)
        {

            for (int j = 10; j >= i; j--)
            {
                Console.Write($" ");
            }

            for (int k = 1; k <= (i * 2) - 1; k++)
            {

                Console.Write("*");

            }

            Console.WriteLine("");
        }
        for (int i = 10; i >= 1; i--)
        {

            for (int j = 10; j >= i; j--)
            {
                Console.Write(" ");
            }

            for (int k = 1; k <= (i * 2) - 1; k++)
            {

                Console.Write("*");

            }
            Console.WriteLine(" ");



        }


    }

    static void Rectangle()
    {

        for (int i = 1; i < 10; i++)
        {
            Console.Write("*");

        }
        Console.WriteLine("");

        for (int i = 1; i <= 10; i++)
        {

            Console.Write("*");
            for (int j = 1; j < 8; j++)
            {

                Console.Write(" ");

            }

            Console.WriteLine("*");

        }

        for (int i = 1; i < 10; i++)
        {
            Console.Write("*");

        }
        Console.WriteLine("");

    }
    static void Main(string[] args)
    {
        Rectangle();

    }


}
