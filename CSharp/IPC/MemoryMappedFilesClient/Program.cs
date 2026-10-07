namespace MemoryMappedFilesClient;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading; 

class Program
{
    static void Main(string[] args)
    {
        // Hook into the shared memory space the server already created
        using var mmf = MemoryMappedFile.OpenExisting(
            "TestMemoryLocation", 
            MemoryMappedFileRights.ReadWrite 
        );

        // Grab the same global lock the server is using so we don't trip over each other
        using var mutex = new Mutex(false, "TestMemoryMutex");
        
        // Open a 1KB window into that shared memory with read/write access
        using var ViewAccessor = mmf.CreateViewAccessor(0, 1024, MemoryMappedFileAccess.ReadWrite);

        Console.WriteLine("Listening for new messages...");

        while (true)
        {
            Thread.Sleep(100); 
            // Wait until the server finishes writing 
            mutex.WaitOne();
            try
            {
                // Check position 0 to see if there's a new message waiting (1 = new data)
                int status = ViewAccessor.ReadInt32(0);

                if (status == 1) 
                {
                    // Read the size of the message from position 4
                    int length = ViewAccessor.ReadInt32(4);
                    
                    // Boundary check to make sure the data size actually fits inside our window
                    if (length > 0 && length <= 1016) // 1024 total bytes minus the 8 bytes used for flags
                    {
                        // Set up a byte array perfectly sized for the incoming text
                        byte[] bytes = new byte[length];
                        
                        // Pull the actual text data out, starting at position 8
                        ViewAccessor.ReadArray(8, bytes, 0, length);
                        
                        // Turn those raw bytes back into a readable string
                        string message = Encoding.UTF8.GetString(bytes);
                        
                        Console.WriteLine($"Read: {message}");
                    }

                    // Tell the server we finished reading this message so it knows the slot is empty
                    ViewAccessor.Write(0, 0);
                }
            }
            finally
            {
                // unlock the mutex so the server can write the next message
                mutex.ReleaseMutex(); 
            }
        }
    }
}
