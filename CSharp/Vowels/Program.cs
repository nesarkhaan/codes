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
                        break;
                    }

                    else
                    {
                        Console.WriteLine($"{userinput} is not a vowel");
                        break;
                    }
            }



        }

    }
}
