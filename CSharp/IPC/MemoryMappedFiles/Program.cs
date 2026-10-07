namespace MemoryMappedFiles;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading; 

class Program
{
    static void Main(string[] args)
    {
        // Define a 64 KB total buffer size for the memory-mapped file
        int BufferSize = 1024 * 64;

        // Create a named system-wide memory-mapped file shared across processes
        using var mmf = MemoryMappedFile.CreateNew(
            "TestMemoryLocation", 
            BufferSize, 
            MemoryMappedFileAccess.ReadWrite
        );

        // Create a named system-wide Mutex to synchronise access between processes.
        // 'false' means the current thread does not initially own the Mutex.
        using var mutex = new Mutex(false, "TestMemoryMutex");
        
        // Create a view accessor to read/write a specific 1 KB section of the shared memory
        using var ViewAccessor = mmf.CreateViewAccessor(0, 1024);
        
        Console.WriteLine("Enter a message or write Quit to exit");

        // Main application loop
        while (true)
        {
            Console.Write(">>> ");
            string input = Console.ReadLine();

            // Exit the loop if the user types "Quit"
            if (input.Equals("Quit", StringComparison.OrdinalIgnoreCase)) 
                break;
            
            // Block the current thread until it safely acquires exclusive access to the Mutex
            mutex.WaitOne();

            try
            {

                int status = ViewAccessor.ReadInt32(0);

                if(status == 1)
                {
                    Console.WriteLine("Client hasn't read the message yet");
                }


                string message = input; 
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                
                // Write the length of the payload as a 4-byte integer at byte offset 0
                ViewAccessor.Write(4, bytes.Length);
                
                // Write the actual string byte array immediately after, starting at byte offset 4
                ViewAccessor.WriteArray(8, bytes, 0, bytes.Length);

                ViewAccessor.Write(0, 1);
                
                Console.WriteLine("Message written.");
            }
            finally
            {
                // Always release the Mutex 
                mutex.ReleaseMutex();
            }
        }
    }
}
