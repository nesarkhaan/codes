namespace FileExaminer;
using FileExaminer.Models;
using FileExaminer.Abstractions;


class Program
{
    static void Main(string[] args)
    {

        while(true) {

            Console.Write(">>>");
            string UserInput = Console.ReadLine().ToLowerInvariant();
            

            if(UserInput == "exit") {

                Console.WriteLine("Exiting....");
                break;

            }




        }


    }
}
