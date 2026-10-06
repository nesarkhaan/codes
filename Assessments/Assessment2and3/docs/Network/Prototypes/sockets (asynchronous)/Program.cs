namespace FileServer;

using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

class Server
{
    private readonly int Port;
    private Socket ListenerSocket;
    //constructor takes an ip addres in bytes
    public Server(int port)
    {
        Port = port;
    }
    //create the listener endpoint
    public async Task StartASync()
    {

        IPEndPoint LocalEndPoint = new IPEndPoint(IPAddress.Any, Port);

        ListenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        ListenerSocket.Bind(LocalEndPoint);

        ListenerSocket.Listen(100);

        Console.WriteLine($"Server Started. Listening on Port {Port}. Waiting for Connection");

        try
        {

            while (true)
            {
                Socket ClientSocket = await ListenerSocket.AcceptAsync();
                Console.WriteLine($"Client Connected: {ClientSocket.RemoteEndPoint}");
                _ = HandleReceived(ClientSocket);
                _ = HandleTransmit(ClientSocket);
            }

        }
        catch (SocketException ex)
        {

            Console.WriteLine($"Server Socket exception: {ex.Message}");


        }


    }

    public async Task HandleTransmit(Socket clientsocket)
    {

        using (clientsocket)
        {
            try
            {
                while (true)
                {


                    string userinput = await Task.Run(() => Console.ReadLine());

                    byte[] EchoBytes = Encoding.UTF8.GetBytes($"Server to {clientsocket.RemoteEndPoint} {userinput} \n");
                    await clientsocket.SendAsync(EchoBytes, SocketFlags.None);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");

            }
        }
    }


    public async Task HandleReceived(Socket clientsocket)
    {

        using (clientsocket)
        {
            byte[] buffer = new byte[1024];

            try
            {
                while (true)

                {
                    int BytesReceived = await clientsocket.ReceiveAsync(buffer, SocketFlags.None);

                    if (BytesReceived == 0)
                    {

                        Console.WriteLine("Client Disconnected");
                        clientsocket.Close();
                        clientsocket.Dispose();
                        break;

                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, BytesReceived);
                    Console.WriteLine($"{clientsocket.RemoteEndPoint} {message}");


                }
            }

            catch (Exception ex)

            {

                Console.WriteLine($"Error handling client: {ex.Message}");

            }


        }


    }


}

class Program
{
    static async Task Main(string[] args)
    {
        int port = 32123;

        Server server = new Server(port);

        await server.StartASync();

    }
}
