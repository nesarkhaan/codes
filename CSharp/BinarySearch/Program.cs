namespace BinarySearch;

class Program
{
    static void Main(string[] args)
    {
        int[] _NumberedData = new int[] {1, 2, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 100}; 
    

        for(int i = 0; i < _NumberedData.Length; i++) {

                Console.WriteLine($"Index: {i}     |    {_NumberedData[i]}");

        }



        int number = 1;

        Console.WriteLine($"Binary Search: {number} {BinarySearch(number, _NumberedData)}");

        Console.WriteLine($"LinearSearch Search: {number} {LinearSearch(number, _NumberedData)}");
        
    }

    static bool BinarySearch(int number, int[] array){

        int Left = 0;
        int Right = array.Length -1;
        // binary search
        while (Left <= Right) {

            int Middle = (Right + Left) / 2;  

            //Base Case    
            if(number == array[Middle]){

                return true;
            } 

            //if less than

            if (array[Middle] < number) {
                Console.WriteLine($"Left is {Left}");
                Console.WriteLine($"Right is {Right}");
                Left = Middle + 1;
                Console.WriteLine($"Target number higher than Middle. Left Jumping up to {Left}");
            } else {
                Console.WriteLine($"Left is {Left}");
                Console.WriteLine($"Right is {Right}");
                Right = Middle - 1;
                Console.WriteLine($"Target number less than middle. Right Jumping down to {Right}");
            }
        }
        return false; 
    }


    static bool LinearSearch(int number, int[] array)
    {

        for(int i = 0; i < array.Length; i++) {

            Console.WriteLine($"i = {array[i]} -- Target: {number}" );

            if (i == array[i]) {

                return true;


            } else {


                return false;

            }


        }
        return false;
    }
}
