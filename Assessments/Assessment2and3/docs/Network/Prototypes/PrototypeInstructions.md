# TCP Socket Server Prototype

The two prototypes have been created to show the usage of sockets and will contain many bugs. 

## Overview
This is a simple prototype presenting a synchronous TCP socket server written in C#. It demonstrates fundamental networking concepts including endpoint configuratin, binding, listening for incoming connections and performing basic encoding/decoding of a continous byte stream over IPv4. 

## Prerequisites 
* [.NET SDK] Version 6.0 or higher
* Terminal / Command-Line Interface (CLI)

### How to Run

Navigate into the specific subdirectory and execute the program:
```bash
cd docs/Network/Prototypes/sockets (synchronous)
dotnet run
```

### Expected Output

```text
Listening on... 127.0.0.1 : 31242

```

### Testing the Server
This is a standalone basic server, you will need a client to send messages to it.


### Use Netcat or telnet to connect to it.

Open a second terminal window and run:

```bash
nc 127.0.0.1 31242
```

Type any message and press 'Enter' from the client. The server will print the message and send back an acknowledgment.


### Open a new terminal window and start another connection. 
```cmd
telnet 127.0.0.1 31242
```
Type any message and send it to the server. The message will not arrive at the server because client 1 is connected to the server. Synchronous only allows communicate with one client at a time. 

Press CTRL+C on the first client. 

The moment the client is disconnected, the message from the second client will arrive.


# Asynchronous TCP Socket Server (Prototype)

## Overview

This prototype is for a asynchronous TCP Server. Unlike a synchronous server that blocks the execution thread while waiting for network activity, this prototype utilises the .net "async/await" pattern which allows the server to handle multiple client connections simultaneously and process a two-way communication (sending and receiving) concurrently without freezing the main application thread.


## Prerequisites
As above for synchronous TCP Socket Server


## How to Run

```bash
cd docs/Network/Prototypes/sockets (asynchronous)
dotnet run
```

### Expected Console Output

```bash
cd docs/Network/Prototypes/sockets (synchronous)
dotnet run
```

### Connect to the server 
Connect with the Server from multiple clients simultaneously and test it by sending a message. The message should arrive from all clients without delay. The server is also able to reply back. 


Using Netcat (Linux/macOS):**
```bash
nc 127.0.0.1 32123
```

Using Telnet (Windows):**
```cmd
telnet 127.0.0.1 32123
```

