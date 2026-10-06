namespace Process1;

using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        // Pass null for the mapName parameter to make it cross-platform
        using(MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile("/dev/shm/ExampleTestFile", System.IO.FileMode.OpenOrCreate, null, 1000)) {
            
            bool mutexCreated;

            // Note: Named Mutexes also have platform limitations on Linux. 
            // They work on newer .NET versions but map to files under /tmp/
            Mutex mutex = new Mutex(true, "ExampleTextFilemutex", out mutexCreated);

            using (MemoryMappedViewStream stream = mmf.CreateViewStream()) {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(true); // Changed to Write(true) since your reader expects Booleans
            }

            mutex.ReleaseMutex();

            Console.WriteLine("Start Process B and press ENTER to continue.");
            Console.ReadLine();

            Console.WriteLine("Start Process C and press Enter to continue.");
            Console.ReadLine();

            mutex.WaitOne();

            using (MemoryMappedViewStream stream = mmf.CreateViewStream())
            {
                BinaryReader reader = new BinaryReader(stream);
                Console.WriteLine("Process A says: {0}", reader.ReadBoolean());
                Console.WriteLine("Process B says: {0}", reader.ReadBoolean());
                Console.WriteLine("Process C says: {0}", reader.ReadBoolean());
            }
            mutex.ReleaseMutex();
        }
    }
}
