namespace NamedPipeServer;

using System.IO; 
using System.Text; 



class Program
{
    static async Task Main(string[] args)
    {

        string NamedPipeServer = "linux_pipe_demo";

        Console.WriteLine($"Process ID: {Environment.ProcessId} ");
        Console.WriteLine($"Process ID: {Environment.UserName} ");
        Console.WriteLine($"Process ID: {NamedPipeServer} ");
        Console.WriteLine($"");




        using var server = new System.IO.Pipes.NamedPipeServerStream(
            NamedPipeServer,
            System.IO.Pipes.PipeDirection.InOut,
            maxNumberOfServerInstances: 1,
            System.IO.Pipes.PipeTransmissionMode.Byte,
            System.IO.Pipes.PipeOptions.Asynchronous
            );
        

        Console.WriteLine("Waiting for client ... ");

        await server.WaitForConnectionAsync();


        byte[] buffer = new byte[4096];

        while (server.IsConnected)
        {
            try 
            {
                int bytesRead = await server.ReadAsync(buffer);

                if (bytesRead == 0) break;

                string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received: {received} ");

                if(received.Equals("QUIT", StringComparison.OrdinalIgnoreCase))
                break;

                string reply = $"Echo from PID {Environment.ProcessId}: {received}";
                await server.WriteAsync(Encoding.UTF8.GetBytes(reply));
                await server.FlushAsync();

            }
            catch (IOException)
            {
                break;
            }

        }

        Console.WriteLine("Done....");
    }
}



