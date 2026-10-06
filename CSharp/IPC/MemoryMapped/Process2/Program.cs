namespace Process2;

using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        try 
        {
            // Open the shared memory file created by Process 1
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile("/dev/shm/ExampleTestFile", FileMode.Open))
            {
                Mutex mutex = Mutex.OpenExisting("ExampleTextFilemutex");

                mutex.WaitOne();

                // FIX 1: Start at offset 1 (after Process 1's boolean) and map the remaining capacity (0)
                using(MemoryMappedViewStream stream = mmf.CreateViewStream(1, 0))
                {
                    BinaryWriter writer = new BinaryWriter(stream);
                    // FIX 2: Write a boolean (false) instead of an integer (0) to match the reader
                    writer.Write(false); 
                }

                mutex.ReleaseMutex();
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Memory-mapped file does not exist. Run process A first");
        }
    }
}
