namespace FileShare.Web.Models;

public class ErrorViewModel
{
    // Property to hold the request ID for error tracking.
    public string? RequestId { get; set; }
    // Property to indicate whether the request ID is available (not null or empty).
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
