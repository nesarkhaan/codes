namespace FileShare.Web.Models;

public sealed record PeerItem(string Id, string Nickname, string IpAddress,int Port, bool IsConnected)
{
    // Returns the address of the peer in the format "IP:Port".
    public string Address => $"{IpAddress}:{Port}";
}
