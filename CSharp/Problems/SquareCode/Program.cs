namespace SquareCode;

class Program
{
    static void Main(string[] args)
    {
        string UserInput = Console.ReadLine().Trim();
        string[][] SquareCode = new string[UserInput.Length][];
        int row = 0;
        for (int i = 0; i < UserInput.Length; i++)
        {
            for (int j = 0; j < SquareCode[i].Length; j++)
            {
                if (row < 8)
                {
                    SquareCode[row][j] = UserInput[i].ToString();
                }
                else
                {
                    row++;
                }
            }
        }

        for (int i = 0; i < SquareCode.Length; i++)
        {
            for (int j = 0; j < SquareCode[i].Length; j++)
            {
                Console.WriteLine(SquareCode[i][j]);

            }

        }
    }
}
