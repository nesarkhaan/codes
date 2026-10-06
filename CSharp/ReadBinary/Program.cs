namespace Test4_ReadBinary;

public class myQueue
{
    private string[]? Array;
    private int Capacity;
    private int Size;

    //the constructor
    public myQueue(int capacity)
    {
        this.Capacity = capacity;

        Array = new string[Capacity];

        Size = 0;

    }



    public void Enqueue(string hex)
    {
        Array[Size] = hex;
        Size++;
    }

    public void Dequeue()
    {

        for (int i = 1; i < Size; i++)
        {

            Array[i - 1] = Array[i];
            Size--;

        }
        ;
    }




}


class Program
{

    static void Main(string[] args)
    {
        /* This method validates the Magic Byte (the first 5 bytes of the file). If the signature is validated, it further validates hex bytes for other strings such as size, filename, hashes, nhashes. If all return true, the method returns a true or false.  */
        //bools for different hex bytes - if all are true, true will be returned, so data can be parsed. 
        bool isSignatureVerified = false;
        bool isIdentVerified;
        // bool size = false;
        // bool filename = false;
        // bool hashes = false;

        //Signature Array
        const int CAPACITY = 5;
        string[] SignatureArray = new string[CAPACITY];


        //string filepath = "badfile.tpk";
        string filepath = "complete_1.tpk";
        FileStream fs = new FileStream(filepath, FileMode.Open);
        int HexValue;
        string hex;

        string[] ident = new string[] { "69", "64", "65", "6E", "74" };


        for (int i = 0; (HexValue = fs.ReadByte()) != 50; i++) //loop should be -1
        {
            Console.WriteLine($"{i} . Loop");
            hex = string.Format("{0:X2}", HexValue);
            Console.WriteLine($"This is the Hex Data: {hex}");

            if (i <= 4)
            {
                SignatureArray[i] = hex;
                Console.WriteLine($"Inside i < 5{i} {SignatureArray[i]}");
            }
            else
            {

                if (isSignatureVerified == true)
                {
                    continue;
                }
                else
                {

                    bool result = VerifySignature(SignatureArray, ident);
                    isSignatureVerified = result;

                    Console.WriteLine($"VerifySignature Failed - Result is {result}");

                }
            }
            //if result of true is found then break from the loop. 
            if (isSignatureVerified == true)
            {
                break;

            }
            //if signature is false then break
            else if (isSignatureVerified == false && i > 4)
            {

                break;
            }

        }





        // if (CompareArrays(array1, array2) == true)
        // {
        //
        //     Console.WriteLine("True");
        // }
        //
        // else
        //
        // {
        //     Console.WriteLine("False");
        //
        // }

    }


    public static bool VerifySignature(string[] signature, string[] ident)
    {

        bool result = false;
        int CountBoolTrue = 0;

        for (int i = 0; i < ident.Length; i++)
        {

            if (signature[i] != ident[i])
            {

                CountBoolTrue--;
                Console.WriteLine($" Value of: {CountBoolTrue}");
            }
            else
            {
                Console.WriteLine($" Value of: {CountBoolTrue}");
                CountBoolTrue++;
            }

            if (CountBoolTrue == 4)
            {
                result = true;
                Console.WriteLine($" Value of: {CountBoolTrue} inside comparison with 4");

            }


            //
            // if (signature[i] == ident[i])
            // {
            //
            //     //CountBoolTrue++;
            //     if (CountBoolTrue != 5)
            //     {
            //         CountBoolTrue++;
            //         Console.WriteLine(CountBoolTrue);
            //
            //         Console.Write($"{i}. False - Inside VerifySignature");
            //         result = false;
            //     }
            //     else
            //
            //     {
            //         Console.WriteLine($"{i}True");
            //         result = true;
            //     }
            //
            // }
        }

        return result;
    }

    public static bool CompareArrays(string[] array1, string[] array2)
    {

        bool result = false;
        int count = 0;

        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] == array2[i])
            {

                Console.Write($"{i}. {array1[i]} True");
                if (count == 4)
                {
                    result = true;
                }
                count++;

            }
            else
            {
                result = false;
                //Console.WriteLine($"{i}. False");
            }


        }

        return result;

    }


}



//
// for (int i = 0; (HexValue = fs.ReadByte()) != -1; i++)
// {
//     string[] watch = new string[size];
//     watch[].Append
//
//
//     hex = string.Format("{0:X2}", HexValue);
//
//     Console.Write(hex);
//
//
// }
