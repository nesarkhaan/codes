namespace Test8;

using myLibs.Maths;

class Game
{

    private string[][] BoardArray;
    private string? Location; //Location of 
    private string? Selection;
    private string? PrintableString; //PrintableString to print error messages, notices, player wins, etc. 
    private int xScore = 0; // Score for Player X
    private int oScore = 0; // Score for Player O
    private int DrawScore = 0;
    private string PlayerTurn = "Player X Turn"; //Setting it Player X Turn as player X always starts first. 


    public bool PlayerX = true;
    public bool PlayerO = false;
    public bool ResetLoop = false;

    public Game()
    {



        BoardArray = new string[3][];
        BoardArray[0] = new string[] { " ", " ", " " };
        BoardArray[1] = new string[] { " ", " ", " " };
        BoardArray[2] = new string[] { " ", " ", " " };
    }
    // Gets the cell number for printing
    public string Get(int CellNumber)
    {
        int row = (CellNumber - 1) / 3;
        int col = (CellNumber - 1) % 3;
        return BoardArray[row][col];

    }
    public int PlayerXScore()
    {

        return xScore;

    }

    public int PlayerOScore()
    {

        return oScore;

    }


    public void Turn(string player)
    {
        PlayerTurn = player;

    }

    public void reset()
    {

        for (int i = 0; i < BoardArray.Length; i++)
        {
            for (int j = 0; j < BoardArray[i].Length; j++)
            {

                BoardArray[i][j] = " ";

            }

        }


    }
    public void CheckWin()
    {
        string First = string.Empty;
        string Second = string.Empty;
        string Third = string.Empty;
        //Win Row
        for (int i = 0; i < 3; i++)
        {
            First = BoardArray[i][0];
            Second = BoardArray[i][1];
            Third = BoardArray[i][2];

            if (First != " " && First == Second && Second == Third)
            {
                if (First == "X" && Second == First && Third == Second)
                {
                    PrintableString = $"Player X Wins";
                    xScore++;

                    Thread.Sleep(1000);
                    reset();
                }
                else
                {

                    PrintableString = $"Player O Wins";
                    oScore++;

                    Thread.Sleep(3000);
                    reset();
                }
            }
        }

        for (int i = 0; i < 3; i++)
        {

            First = BoardArray[0][i];
            Second = BoardArray[1][i];
            Third = BoardArray[2][i];

            if (First != " " && First == Second && Second == Third)
            {

                if (First == "X")
                {
                    PrintableString = $"Player X Wins";
                    xScore++;

                    Thread.Sleep(3000);
                    reset();
                }
                else
                {
                    PrintableString = $"Player O Wins";
                    oScore++;

                    Thread.Sleep(3000);
                    reset();
                }


            }

        }

        for (int i = 0; i < 1; i++)
        {

            First = BoardArray[0][0];
            Second = BoardArray[1][1];
            Third = BoardArray[2][2];

            if (First != " " && First == Second && Second == Third)
            {

                if (First == "X")
                {
                    PrintableString = $"Player X Wins";
                    xScore++;

                    Thread.Sleep(3000);
                    reset();
                }
                else
                {
                    PrintableString = $"Player O Wins";
                    oScore++;

                    Thread.Sleep(3000);
                    reset();
                }


            }

        }

        for (int i = 0; i < 1; i++)
        {

            First = BoardArray[0][2];
            Second = BoardArray[1][1];
            Third = BoardArray[2][0];

            if (First != " " && First == Second && Second == Third)
            {

                if (First == "X")
                {
                    PrintableString = $"Player X Wins";
                    xScore++;

                    Thread.Sleep(3000);
                    reset();
                }
                else
                {
                    PrintableString = $"Player O Wins";
                    oScore++;

                    Thread.Sleep(3000);
                    reset();
                }


            }

        }


    }

    //this function checks if all the " " empty strings in the array has been filled and CheckWin is not activated hence the draw. It updates class global counter. 
    public void CheckDraw()
    {
        int counter = 0;
        for (int i = 0; i < BoardArray.Length; i++)
        {

            for (int j = 0; j < BoardArray[i].Length; j++)
            {

                if (BoardArray[i][j] != " ")
                {
                    counter++;
                    if (counter == 9)
                    {
                        PrintableString = "It's a draw";

                        DrawScore++;
                        reset();
                    }

                }

            }

        }

    }


    public void EnterUserSelection(string userinput)
    {
        string[] InputParts = userinput.Split(",");
        Location = InputParts[0];
        Selection = InputParts[1].ToUpper();

        if (int.Parse(Location) >= 1 && int.Parse(Location) <= 9)
        {
            int row = (int.Parse(Location) - 1) / 3;
            int col = (int.Parse(Location) - 1) % 3;

            if (BoardArray[row][col] != "X" && BoardArray[row][col] != "O")
            {
                if (Selection == "X" || Selection == "O")
                {
                    BoardArray[row][col] = Selection;
                }
            }
            else
            {

                PrintableString = $"Please make another selection: ";
                if (Selection == "X")
                {

                    PlayerX = true;
                    PlayerO = false;
                    PlayerTurn = "Player X Turn";
                    ResetLoop = true;

                }
                if (Selection == "O")
                {
                    PlayerO = true;
                    PlayerX = false;
                    PlayerTurn = "Player O Turn";
                    ResetLoop = true;

                }



            }
        }
    }




    //This method prints the board and updates it. 
    public void PrintBoard()
    {
        Console.WriteLine(PlayerTurn);
        Console.WriteLine($"\nBoard: \n\t======================\n\t|  {Get(1)}   |   {Get(2)}  |   {Get(3)}  | \n\n\t|  {Get(4)}   |   {Get(5)}  |   {Get(6)}  | \n\n\t|  {Get(7)}   |   {Get(8)}  |   {Get(9)}  |\n\t======================");
    }

    public void PrintNotice()
    {
        //Prints the top where player scores are printed. 
        Console.WriteLine($"Score \n Player X: {PlayerXScore()}\n Player O: {PlayerOScore()} \n Draw    : {DrawScore}");
        Console.WriteLine("-------------------------");

        //Prints PrintableString which display wins, and errors. 
        Console.WriteLine(PrintableString);

    }

    //ResetingPrintNotice so it is printed changed prior to next iteration of the loop.
    public void ResetPrintNotice()
    {
        PrintableString = string.Empty;
    }




}

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        string? UserInput = string.Empty;

        while (true)
        {
            if (game.ResetLoop == true)
            {
                game.ResetLoop = false;
                continue;
            }

            Console.Clear();
            game.PrintNotice();
            game.PrintBoard();
            game.ResetPrintNotice();


            if (game.PlayerX == true)
            {

                Console.WriteLine("Please Enter Selection (1-9. X): ");
                UserInput = Console.ReadLine() + "," + "X";
                game.PlayerX = false;
                game.PlayerO = true;
                game.Turn("Player O Turn");

            }
            else if (game.PlayerO == true)
            {
                Console.WriteLine("Please Enter Selection (1-9. X): ");
                UserInput = Console.ReadLine() + "," + "O";
                game.PlayerX = true;
                game.PlayerO = false;
                game.Turn("Player X Turn");

            }


            game.EnterUserSelection(UserInput);
            if (game.ResetLoop == true)
            {
                game.ResetLoop = false;
                continue;


            }
            Console.Clear();
            game.PrintNotice();
            game.PrintBoard();


            game.CheckWin();
            game.CheckDraw();









        }




    }
}

