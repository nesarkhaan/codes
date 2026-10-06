using FileShare.Web.Models;
using FileShare.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FileShare.Web.Pages;

public sealed class PeersModel(IFileShareUiService fileShareService) : PageModel
{
    // Store the list of peers to display in the table.
    public IReadOnlyList<PeerItem> Peers { get; private set; } = [];   
    public int ConnectedCount => Peers.Count(peer => peer.IsConnected);

    [BindProperty]
    public string? IpAddress { get; set; }

    [BindProperty]
    // Store the port number for connecting to a peer.
    public int? Port { get; set; }

    // Handles the GET request to display the list of peers.
    public void OnGet()
    {
        Peers = fileShareService.GetPeers();
    }
    // Handles the POST request to connect to a new peer.
    public IActionResult OnPostConnect()
    {
        var result = fileShareService.ConnectPeer(IpAddress, Port);
        SetMessage(result);
        return RedirectToPage();
    }

    // Handles the POST request to disconnect from a peer.
    public IActionResult OnPostDisconnect(string? peerId)
    {
        var result = fileShareService.DisconnectPeer(peerId);
        SetMessage(result);
        return RedirectToPage();
    }

    // Handles the POST request to reconnect to a peer.
    public IActionResult OnPostReconnect(string? peerId)
    {
        var result = fileShareService.ReconnectPeer(peerId);
        SetMessage(result);
        return RedirectToPage();
    }

    // Handles the POST request to remove a peer from the list.
    public IActionResult OnPostRemove(string? peerId)
    {
        var result = fileShareService.RemovePeer(peerId);
        SetMessage(result);
        return RedirectToPage();
    }

    // Sets a message to be displayed to the user after an operation.
    private void SetMessage(OperationResult result)
    {
        TempData["Message"] = result.Message;
        TempData["MessageType"] = result.Succeeded ? "success" : "error";
    }
}
