namespace Test5_Memory;

class Program
{
    static void Main(string[] args)
    {

        string text = "S5280FT";
        Console.WriteLine("{0}", text);
        unsafe
        {
            fixed (char* pText = text)

            {

                /* First Method of doing it    */
                // char* p = pText;
                // *++p = 'm';
                // *++p = 'i';
                // *++p = 'l';
                // *++p = 'e';
                // *++p = '8';
                // *++p = '8';


                /* Second Method   */

                pText[1] = 'm';
                pText[2] = 'i';
                pText[3] = 'l';
                pText[4] = 'e';
                pText[5] = ' ';
                pText[6] = ' ';


            }
        }
        Console.WriteLine(text);
    }
}
