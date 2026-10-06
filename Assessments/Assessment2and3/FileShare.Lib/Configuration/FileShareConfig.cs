namespace FileShare.Lib.Configuration;

public class FileShareConfig
{   // Nickname of the user or node in the file sharing network.
    public string Nickname { get; set; }
    // Directory where metadata files are stored.
    public string MetaFileDirectory { get; set; }
    // Directory where shared files are stored.
    public string FileDirectory { get; set; }
    // Maximum number of peers that can connect to this node.
    public int MaxPeers { get; set; }
    //  Port number on which the node listens for incoming connections.
    public int Port { get; set; }

}
