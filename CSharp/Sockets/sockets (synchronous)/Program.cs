namespace sockets;

using System.Net.Sockets;
using System.Net;
using System.Text;

class Server
{

    IPAddress IPListener { get; set; }
    int PortListener { get; set; }
    IPEndPoint serverEndPoint { get; set; }


    //constructor takes an ip addres in bytes
    public Server(byte[] ipaddress, int port)
    {
        this.IPListener = IpFormatter(ipaddress);
        this.PortListener = port;
    }
    //create the listener endpoint
    public void CreateListener(int bytesize)
    {
        Console.WriteLine($"Listening on... {IPListener} : {PortListener}");
        serverEndPoint = CreateEndPoint(IPListener, PortListener);


        using (Socket listener = new(
            serverEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp
                    ))
        {

            byte[] recieveBuffer = BufferSize(bytesize);
            listener.Bind(serverEndPoint);

            /// Socket .Listen(backlog) stores number of connections
            /// - only one connection can transmit and receive
            /// - if multiple connections, 1st has the connection. transmission from other connections are held until connections before are disconnected.
            /// - Double loop required for continous messages
            while (true)
            {
                //Able to listen to a number of connections
                listener.Listen(32);
                //Will only take transmissions from one connection at a time. 
                Socket client = listener.Accept();


                //client.Shutdown(SocketShutdown.Send);
                //client.Close();





                while (true)
                {
                    int counter = 0;

                    //Receives the buffer and places it into recieveBuffer byte[]
                    //if placed in an integer variable - you get the buffer Length

                    int recvLength = client.Receive(recieveBuffer);


                    if (recvLength <= 0)
                    {
                        break;
                    }

                    //Decodes the message. We pass the recvLength to stop ghost values
                    string message = Decode(recvLength, recieveBuffer);


                    Console.WriteLine(message);

                    string confirmationMessage = $"{counter++}Acknowledged";

                    byte[] writeBuffer = Encode(confirmationMessage);
                    client.Send(writeBuffer);
                }
            }



        }
    }

    public byte[] Encode(string message)
    {
        return Encoding.ASCII.GetBytes(message);

    }

    public string Decode(int bytesize, byte[] bytes)
    {
        return Encoding.ASCII.GetString(bytes, 0, bytesize);

    }


    //creates a buffer of inputted bytesize
    public byte[] BufferSize(int bytesize)
    {

        return new byte[256];

    }

    //format the IP
    public IPAddress IpFormatter(byte[] ipaddress)
    {
        return new IPAddress(ipaddress);
    }


    //return a new endpoint using the formatted ip
    public IPEndPoint CreateEndPoint(IPAddress ipaddress, int port)
    {
        return new IPEndPoint(ipaddress, port);
    }



}

class Program
{
    static void Main(string[] args)
    {
        byte[] ip = new byte[] { 127, 0, 0, 1 };
        int port = 31242;

        Server s = new Server(ip, port);
        s.CreateListener(256);




    }
}
