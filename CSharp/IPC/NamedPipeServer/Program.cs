namespace NamedPipeServer;

using System.IO; 
using System.Text; 

class Program
{
    static async Task Main(string[] args)
    {
        // The name for our pipe so the client application knows where to connect
        string NamedPipeServer = "linux_pipe_demo";

        // Print out basic environment details to the console for debugging
        Console.WriteLine($"Process ID: {Environment.ProcessId} ");
        Console.WriteLine($"Process ID: {Environment.UserName} "); // Note: Prints the username, despite the 'Process ID' label
        Console.WriteLine($"Process ID: {NamedPipeServer} ");      // Note: Prints the pipe name, despite the 'Process ID' label
        Console.WriteLine($"");

        // Set up the server stream. We're configuration it to handle data coming in and out,
        // allowing only 1 client at a time, and enabling async operations so it doesn't freeze the thread.
        using var server = new System.IO.Pipes.NamedPipeServerStream(
            NamedPipeServer,
            System.IO.Pipes.PipeDirection.InOut,
            maxNumberOfServerInstances: 1,
            System.IO.Pipes.PipeTransmissionMode.Byte,
            System.IO.Pipes.PipeOptions.Asynchronous
            );
        
        Console.WriteLine("Waiting for client ... ");

        // Pause here asynchronously until a client application actually connects to the pipe
        await server.WaitForConnectionAsync();

        // Create a 4KB buffer window to hold raw incoming data
        byte[] buffer = new byte[4096];

        // Keep looping as long as the client stays connected to our pipe
        while (server.IsConnected)
        {
            try 
            {
                // Wait for the client to send data and read it into our buffer array
                int bytesRead = await server.ReadAsync(buffer);

                // If 0 bytes are read, it means the client cleanly disconnected or closed their end
                if (bytesRead == 0) break;

                // Convert the raw bytes we just received back into a readable string
                string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received: {received} ");

                // Break out of the loop and close shop if the client sends the word "QUIT"
                if(received.Equals("QUIT", StringComparison.OrdinalIgnoreCase))
                    break;

                // Build a response message including this server's process ID
                string reply = $"Echo from PID {Environment.ProcessId}: {received}";
                
                // Convert our response string to bytes and send it back down the pipe
                await server.WriteAsync(Encoding.UTF8.GetBytes(reply));
                
                // Force any buffered data out immediately so the client gets it without delays
                await server.FlushAsync();
            }
            catch (IOException)
            {
                // If the client abruptly crashes or disconnects, catch the error and break the loop 
                break;
            }
        }

        Console.WriteLine("Done....");
    }
}
