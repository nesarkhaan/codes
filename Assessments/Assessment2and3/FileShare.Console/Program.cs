using System;
using System.Threading.Tasks;
using FileShare.Lib.Networking;

/// <file>
/// <summary>
/// FileShare Console Application
/// </summary>
/// <remarks>
/// This console application acts as an IPC Client of the IPCServer and communicates with the background 
/// <c>FileShare.Service</c> daemon using the shared network library protocol.
/// </remarks>
/// </file>

Console.WriteLine("=== FileShare Terminal (TUI) ===");
Console.WriteLine("Type 'HELP' to see commands or 'EXIT' to quit.");


/// <summary>
/// Initialising the IPC client instance used for communicating with the FileShare daemon service.
/// </summary>

var ipcClient = new IpcClient(); 
while (true)
{
    Console.Write("\nFileShare> ");
    string? input = Console.ReadLine();
    
    //Ignoring any empty input
    if (string.IsNullOrWhiteSpace(input)) continue;
    
    // Exit the loop if the user types "EXIT"
    if (input.ToUpper() == "EXIT") break;
    // Show help if the user types "HELP"
    if (input.ToUpper() == "HELP")
    {
        Console.WriteLine("Available Commands:");
        Console.WriteLine("  CREATE_TPK|<filepath>    - Add a new file to generate tpk file. Tpk is placed into meta folder. File is copied to the files folder");
        Console.WriteLine("  GET_PEERS                - List all connected peers");
        Console.WriteLine("  CONNECT|<ip>|<port>      - Connect to a new peer (e.g., CONNECT|127.0.0.1|9000)");
        Console.WriteLine("  ADD_PACKAGE|<filepath>   - Add a .tpk file to track");
        Console.WriteLine("  GET_PACKAGES             - List all packages and progress");
        Console.WriteLine("  REMOVE_PACKAGE          - Remove a package");
        Console.WriteLine("  DISCONNECT|IDENTIFIER    - Disconnect from a peer using identifier");
        Console.WriteLine("  RECONNECT|IDENTIFIER     - Reconnect with a peer using identifier");
        Console.WriteLine("  GET_CONFIG               - Get all the configruation");
        Console.WriteLine("  SAVE_CONFIG              - Save the configuration file. Save_Config|Nickname|MetaFileDirectory|FileDirectory|MaxPeers|Port");
        continue;
    }
    // Send the command to the FileShare.Service daemon and await the response
    try
    {
        // Uses the same existing FileSharePipe protocol through the shared Lib connection.
        string? response = await ipcClient.SendAsync(input);
        
        if (response != null)
        {
            // format the output based on the SUCCESS/ERROR prefixes we put in the Daemon
            string[] parts = response.Split('|', 2);
            // Set the console color based on the response type
            if (parts[0] == "SUCCESS")
                Console.ForegroundColor = ConsoleColor.Green;
            // If the response is an error, set the console color to red
            else
                Console.ForegroundColor = ConsoleColor.Red;

            // Print the message. 
            Console.WriteLine(parts.Length > 1 ? parts[1].Replace(";", "\n  ") : parts[0]);
            Console.ResetColor();
        }
    }
    // Handle timeout exceptions specifically to inform the user that the daemon might not be running
    catch (TimeoutException)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[!] Error: Could not connect to the Daemon. Is FileShare.Service running in another window?");
        Console.ResetColor();
    }
    // Handle any other exceptions that may occur during IPC communication
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[!] IPC Error: {ex.Message}");
        Console.ResetColor();
    }
}
