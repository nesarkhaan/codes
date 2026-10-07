namespace NamedPipeClient;

using System.IO; 
using System.Text; 

class Program
{
    static async Task Main(string[] args)
    {
        // Must exactly match the pipe name defined in the server application
        string NamedPipeServer = "linux_pipe_demo";

        Console.WriteLine("Named Pipe Client");
        Console.WriteLine($"Process ID: {Environment.ProcessId} ");
        Console.WriteLine($"");

        // Set up the client pipe connection.
        // "serverName: '.'" means we are looking for the server on the local machine.
        // It's configured for data to go both ways and enables async operations.
        using var client = new System.IO.Pipes.NamedPipeClientStream(
            serverName: ".",
            pipeName: NamedPipeServer,
            System.IO.Pipes.PipeDirection.InOut,
            System.IO.Pipes.PipeOptions.Asynchronous
            );

        Console.WriteLine("Connecting....");

        // Try to hook up to the server pipe, giving it up to 5 seconds before throwing an error
        await client.ConnectAsync(timeout: 5000);

        // Tell the pipe to read incoming data as a continuous stream of raw bytes
        client.ReadMode = System.IO.Pipes.PipeTransmissionMode.Byte;
        Console.WriteLine("Connected! Type a message or QUIT to exit");

        // Set up a 4KB buffer window to hold the server's reply
        byte[] buffer = new byte[4096];

        // Main messaging loop
        while (true)
        {
            Console.Write(">");
            string? input = Console.ReadLine();

            // Skip this iteration if the user just hits Enter without typing anything
            if (string.IsNullOrEmpty(input)) continue;

            // Convert the user's text into bytes and push it through the pipe
            await client.WriteAsync(Encoding.UTF8.GetBytes(input));
            
            // Push any buffered data out immediately so the server gets the message right away
            await client.FlushAsync();

            // If the user typed "QUIT", break out of the loop and shut down the client
            if(input.Equals("QUIT", StringComparison.OrdinalIgnoreCase))
                break;

            // Wait for the server's response and read it into our buffer array
            int bytesRead = await client.ReadAsync(buffer);
            
            // Convert the server's response bytes back into a readable string
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($" <-- {response}");
        }

        Console.WriteLine("Done");
    }
}
