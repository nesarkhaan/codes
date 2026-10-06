using System.Globalization;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace BinarySearch;


public class SearchResult
{    
    public int Index {get; set;}
    public bool Success {get; set;}

}

class Program
{
    static void Main(string[] args)
    {
        int[] _NumberedData = new int[] {1, 2, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 100};

        while(true) {
            Console.Write("Number > ");
            string userInput = Console.ReadLine().ToLowerInvariant();

            if(userInput == "exit")
                {
                    break;
                }
            
            int number = Convert.ToInt32(userInput);
            var result  = BinarySearch(number, _NumberedData);


            if (result.Success == true ) { 
                Console.Write($"Found at Index: {result.Index} \n"); 
            } 
            else
            {    
                Console.Write($"Number: {number} not found!! \n"); 
            }
        }
        
       
    }

    static SearchResult BinarySearch(int number, int[] array){
        //Left split
        int Left = 0;

        //right split
        int Right = array.Length -1;
        // binary search
        while (Left <= Right) {

            //Split and get the middle
            int Middle = (Right + Left) / 2;  

            //base case - number is found, return object with index and success bool. 
            if(number == array[Middle]){

                return new SearchResult {Index = Middle, Success = true};
            }

            //if number is less than the middle then left is moved middle + 1.

            if (array[Middle] < number) {
                Left = Middle + 1;
            } else {

                Right = Middle - 1;
            }
        }

        //result is not found. index return is -1, success is false. 
        return new SearchResult {Index = -1, Success = false};
    }

}
