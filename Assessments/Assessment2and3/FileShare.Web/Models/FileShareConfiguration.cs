using System.ComponentModel.DataAnnotations;

namespace FileShare.Web.Models;

public sealed class FileShareConfiguration
{
    // The nickname of the peer in the file sharing network.
    [Required(ErrorMessage = "Nickname is required.")]
    [Display(Name = "Nickname")]
    // Default value for the nickname is set to "peer-iso".
    public string Nickname { get; set; } = "peer-iso";

    // The directory where metadata files are stored.
    [Required(ErrorMessage = "Metadata directory is required.")]

    // Display name for the metadata directory in the UI.
    [Display(Name = "Metadata directory")]
    // Default value for the metadata directory is set to "descriptors".
    public string MetafileDirectory { get; set; } = "descriptors";
    //
    [Required(ErrorMessage = "File directory is required.")]
    // Display name for the file directory in the UI.
    [Display(Name = "File directory")]

    // Default value for the file directory is set to "downloads".
    public string FileDirectory { get; set; } = "downloads";   
    [Range(1, 2048, ErrorMessage = "max_peers within the configuration file is set to an invalid value")]
    [Display(Name = "Maximum peers")]

    // Default value for the maximum number of peers is set to 128.
    public int MaxPeers { get; set; } = 128;

    [Range(1, 65535, ErrorMessage = "Port specified is either in use or invlaid")]
    [Display(Name = "Listening port")]
    public int Port { get; set; } = 9000;

    // Creates a copy of the current FileShareConfiguration instance.
    public FileShareConfiguration Copy() => new()
    {
        Nickname = Nickname,
        MetafileDirectory = MetafileDirectory,
        FileDirectory = FileDirectory,
        MaxPeers = MaxPeers,
        Port = Port
    };
}
