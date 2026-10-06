using FileShare.Web.Models;
using FileShare.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FileShare.Web.Pages;



// Represents the configuration page for the FileShare application.
public sealed class ConfigurationModel(IFileShareUiService fileShareService) : PageModel
{
    [BindProperty]
    public FileShareConfiguration Input { get; set; } = new();

    public void OnGet()
    {
        Input = fileShareService.GetConfiguration();
    }

    // Handles the POST request to save the configuration.
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            TempData["Message"] = "Configuration rejected: all fields must contain valid values.";
            TempData["MessageType"] = "error";
            return Page();
        }

        var result = fileShareService.SaveConfiguration(Input);
        TempData["Message"] = result.Message;
        TempData["MessageType"] = result.Succeeded ? "success" : "error";

        return result.Succeeded ? RedirectToPage() : Page();
    }
}
