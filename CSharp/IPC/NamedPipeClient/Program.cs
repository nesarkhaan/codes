namespace NamedPipeClient;

using System.IO; 
using System.Text; 

class Program
{
    static async Task Main(string[] args)
    {
        string NamedPipeServer = "linux_pipe_demo";


        Console.WriteLine("Named Pipe Client");
        Console.WriteLine($"Process ID: {Environment.ProcessId} ");
        Console.WriteLine($"");



        using var client = new System.IO.Pipes.NamedPipeClientStream(
            serverName: ".",
            pipeName: NamedPipeServer,
            System.IO.Pipes.PipeDirection.InOut,
            System.IO.Pipes.PipeOptions.Asynchronous
            );

        Console.WriteLine("Connecting....");

        await client.ConnectAsync(timeout: 5000);

        client.ReadMode = System.IO.Pipes.PipeTransmissionMode.Byte;
        Console.WriteLine("Connected! Type a message or QUIT to exit");

        byte[] buffer = new byte[4096];

        while (true)
        {

            Console.Write(">");
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input)) continue;

            await client.WriteAsync(Encoding.UTF8.GetBytes(input));
            await client.FlushAsync();


            if(input.Equals("QUIT", StringComparison.OrdinalIgnoreCase))
            break;


            int bytesRead = await client.ReadAsync(buffer);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($" <-- {response}");

        }

        Console.WriteLine("Done");
    }
}
