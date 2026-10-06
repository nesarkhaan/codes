namespace bubblesort;

class Program
{
    static void Main(string[] args)
    {
        int[] arr = new int[]{9, 10, 5, 2, 6, 1};
        
        for(int i=arr.Length - 1; i>0; i--)
        { //first loop moving down
           Console.WriteLine("First Loop" + i);
            for (int j=0; i>j; j++)
            { //second loop moving up
            Console.WriteLine("2nd Loop" + j);
               if (arr[j] > arr[j + 1]){
                  int exchange = arr[j];
                  arr[j] = arr[j+1];
                  arr[j+1]= exchange;
                  Console.WriteLine(arr[j+1] + " is bigger than " + arr[j] + " so they are swapped.");
                  
                  
               
               }
               else
               {
                  Console.WriteLine(arr[j] + " is smaller than " + arr[j+1] + " so no");
               
               
               }

            }

            }
        Console.WriteLine("Final Array: ");
        for (int k = 0; k < arr.Length; k++)
            {
            Console.Write(arr[k] + " ,");
        
        
        }
        
        
    }
}
