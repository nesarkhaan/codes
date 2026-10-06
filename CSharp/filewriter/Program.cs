namespace filewriter;


    
class Program
{
    static private int[]? loc;
    static private string[][]? array;
    static private int Count;
    static void textWrite(string text1, string text2)
    
    
    {
        
        //name of the file
        string filename = "data.csv";
        //Sub directory
        string path1 = @"data/";
        
        //Getting full path
        string fullpath = System.IO.Path.Combine(System.IO.Path.GetFullPath(path1), filename);
        
        //Declaring writer object
        System.IO.StreamWriter writer;
        //Initializing
        writer = new System.IO.StreamWriter(fullpath);


            writer.WriteLine(text1);
            writer.WriteLine(text2);
            //Flush() and close() file to save memory. 
            Console.WriteLine("Success");
            //Close and flush writer so data can be transferred from buffer to the file.
            writer.Close();



    }

    static void arrayWrite()
    {
        Count = 5;
        loc = new int[]{0, 1, 2, 3, 4};
        array = new string[Count][];
        array[0] = ["Ajmal", "Emal", "Ahmad", "Qomandan", "Toofan"];
        array[1] = ["Ben", "mark", "ahmad", "megan", "lala", "mikhael"];
        array[2] = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        array[3] = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
        array[4] = ["Summer", "Winter", "Autumn", "Spring"];

        
        //name of the file
        string filename = "data.csv";
        
        //Sub directory
        string path1 = @"data/";
        
        //Getting full path
        string fullpath = System.IO.Path.Combine(System.IO.Path.GetFullPath(path1), filename);
       
          //Declaring writer object
        System.IO.StreamWriter writer;
        //Initializing
        writer = new System.IO.StreamWriter(fullpath);

        for(int i = 0; i < loc.Length; i++)
        {
            writer.Write($"{loc[i]}"); Console.WriteLine($"Writing: LOC. {loc[i]}");
            for (int j = 0; j < array[i].Length; j++)
            {
                writer.Write($",{array[i][j]}");
            }
            writer.Write("\n");
        }
        writer.Close();
    }

    static int[] ConvertToInt(string[] parts){
        
        int[] Parts = new int[parts.Length];

        for (int i = 0; i < parts.Length; i++)
        {
            Parts[i] = Convert.ToInt32(parts[i]);
        }

        return Parts;

    }

    static void LoadFile(string file){
        
        string line = null;
        int lineCounter = 0;
        int[] LoadKeys = new int[100];
        string[][] LoadDatabase = new string[100][];

        string filePath = $"./data/{file}";
        System.IO.StreamReader reader;
        reader = new System.IO.StreamReader(filePath);


       while ((line = reader.ReadLine()) != null){
            
           string[] Parts = line.Split(",");
           int[] PartsConvertedToInt = ConvertToInt(Parts);
           LoadKeys[lineCounter] = PartsConvertedToInt[0];
           LoadDatabase[lineCounter] = new string[Parts.Length - 1];
           
           for(int i = 1; i < Parts.Length; i++){
              LoadDatabase[lineCounter][i - 1] = PartsConvertedToInt[i];
            }
           lineCounter++;

         }
        loc = LoadKeys;
        array = LoadDatabase;
        reader.Close();
        
        for(int i = 0; i < lineCounter; i++)
        {
            Console.Write($"{loc[i]}");
            for (int j = 0; j < array[i].Length; j++)
            {
                Console.Write($",{array[i][j]}");
            }
            Console.Write("\n");
        }

    }
    static void Main(string[] args)
    {
       //Console.Write("Text1: ");
       //string Input1 = Console.ReadLine();
       //string Input2 = Console.ReadLine();

        //textWrite(Input1, Input2);
        //arrayWrite();
        string FileName = Console.ReadLine();
        LoadFile(FileName);

        
        // to load arrayWrite();
    }
    
}
