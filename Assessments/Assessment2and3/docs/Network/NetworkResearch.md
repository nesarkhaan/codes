# Network

## Sockets

This document contains research into Network Protocols to assist with building the file sharing application. In C#, the sockets class is used to transfer data from one endpoint to a second endpoint.

###Sockets - Synchronous 

Functions 
    - Socket function is used to initiate the socket. i.e. Socket listener = new Socket()
    - socket.Bind() is ussed to associate the local endpoint address to the socket object.
    - socket.Listen(backlog) allows the number of connections at the time connected to a single socket thread. If set to 1, 2nd user will not be able to join. Terminal bash command of 'ulimit -n' prints the number of sockets allowed by the OS. For Ubuntu, the default value is 1024. The amount of connections can be increased or decreased.
    - Bind() and Listen() is only a requirement for the server. Not the client.
    - Send() and Receive() is used to send data from the server to the client and black.

Synchronous sockets only allows one client/server communication. When other clients connect to the server, they are held in the backlog .listen() until the first client disconnects. Messages from second client are kept in the buffer until the first client disconnects. 

For example, check socket prototype. 
```html
https://github.com/lexli201101/Assignment3FileShare/tree/master/docs/Network/Prototypes/sockets%20(synchronous)
```

### Sockets - Asynchronous

Asynchronous works on the idea of concurrency, as in running multiple tasks at the same time. Asynchronous sockets allows multiple connection at the same time. All the clients are able to communicate with the server at the same time. Asynchronous connections are made to the server on a single thread. Thread limit can be checked on the server device and can be checked using the command of "ulimit -s". By default, it has been set to 8192 (usually 8mb on the ram). 
For example, check socket prototype (Asynchrnous)

Functions
    - await
    - async
    - tasks

```html
https://github.com/lexli201101/Assignment3FileShare/tree/master/docs/Network/Prototypes/sockets%20(asynchronous)

```


### Sockets - Multithreading

Multithreading allows a process to execute multiple threads concurrently with threads sharing the same memory and resources
if each client connection is given its own thread with 8mb of space then 1000 x 8 will result in 80gb of ram for the stacks.

### Sockets / OSI model layers

C# sockets operate on the userspace in the application layer. In a Linux system or any operating system, the application layer has to make a call to the kernel that is able to interpret and communicate with the physical machine. In Linux, this is achieved through making a system call. 

A user or an application are able to invoke a system call using glibc library which is a wrapper function which assists in performing the necessary steps required to make all the correct syscalls. It basically copies the arguments and the unique system call number to the registers where the kernel expects them. The kernel is then able to process the instructions based on the syscall that has been invoked. 

OSI Model layer 5-7 is the Application layer and focused on the Data. Sockets sit in the session layer and interconnects the application layer to the Data Flow Layers. When a packet is packaged using sockets, necessary headers are placed onto the packet at the different the different layers.

| Headers | Layer |
| :--- | ---: |
| Data | Application Layer |
| TCP/Data | Transport Layer |
| IP/TCP/Data | Network Link Layer |
| Data Link / IP / TCP / Data | Data Link Layer |


System calls are numbered however they differ on different operating systems / architecture. On a Linux x86_64, the sys call for .socket() is 41. The arguments placed in the socket initiation is passed through invoking the call which passes this argument to the sys_socket function in the kernel. All the required sys calls are loaded when the kernel boots the system and are available. 



## Uri class 

[needs research]


## System.Net.NetworkInformation 

this class allows for the gathering of information about network events, changes, statistics and properties. For example, to check the status of a network, the ping can be utilised to instantiate a ping object which calls onto ping.Sendpingasync(String) and the status of the ping is returned. 

```csharp
using Ping ping = new();

string hostName = "stackoverflow.com";
PingReply reply = await ping.SendPingAsync(hostName);
Console.WriteLine($"Ping status for ({hostName}): {reply.Status}");
if (reply is { Status: IPStatus.Success })
{
    Console.WriteLine($"Address: {reply.Address}");
    Console.WriteLine($"Roundtrip time: {reply.RoundtripTime}");
    Console.WriteLine($"Time to live: {reply.Options?.Ttl}");
    Console.WriteLine();
}
```
### Network statistics and properties

* The NetworkInterface, NetworkInterfaceType, and PhysicalAddress classes give information about a particular network interface

*  the IPInterfaceProperties, IPGlobalProperties, IPGlobalStatistics, TcpStatistics, and UdpStatistics classes give information about layer 3 and layer 4 packets

[needs further research]

### Network change events 

* The System.Net.NetworkInformation.NetworkChange class enables you to determine whether the network address or availability has changed

[needs further research]
