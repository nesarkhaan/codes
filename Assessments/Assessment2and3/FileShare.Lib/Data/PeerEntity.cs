namespace FileShare.Lib.Data;

// Moved from FileShare.Web so Service can write the same database that Web reads.
public class PeerEntity
{
    // Unique identifier for the peer entity in the database.
    public int Id { get; set; }
    // Unique identifier for the runtime instance of the peer, used for tracking and identification.
    public string RuntimeId { get; set; } = "";
    // Nickname of the peer, used for display and identification in the network.
    public string Nickname { get; set; } = "";
    // IP address of the peer, used for network communication.
    public string IPAddress { get; set; } = "";
    // Port number on which the peer is listening for incoming connections.
    public int Port { get; set; }
    // Indicates whether the peer is currently connected to the network.
    public bool Connected { get; set; }
    // Date and time when the peer was last seen or active in the network.
    public DateTime LastSeen { get; set; }
}
