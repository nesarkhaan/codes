using FileShare.Web.Models;
using FileShare.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FileShare.Web.Pages;

public sealed class PackagesModel(IFileShareUiService fileShareService) : PageModel
{
    // Store the list of packages to display in the table.
    public IReadOnlyList<PackageItem> Packages { get; private set; } = [];

    // Store the number of packages in each category.
    // These counts remain unchanged when the displayed list is filtered.
    public int CompleteCount { get; private set; }
    public int IncompleteCount { get; private set; }
    public int TotalCount { get; private set; }

    // Read the selected package filter from the URL query string.
    // The default value displays all managed packages.
    [BindProperty(SupportsGet = true)]
    public string Filter { get; set; } = "all";

    [BindProperty]
    public IFormFile? PackageFile { get; set; }

    public void OnGet()
    {
        // Retrieve the complete package list from the UI service.
        var allPackages = fileShareService.GetPackages();

        // Calculate the totals before filtering the displayed list.
        TotalCount = allPackages.Count;
        CompleteCount = allPackages.Count(package => package.State == PackageState.Complete);
        IncompleteCount = TotalCount - CompleteCount;

        // Check the filter supplied in the URL.
        // Unknown or missing values default to displaying all packages.
        Filter = Filter?.Trim().ToLowerInvariant() switch
        {
            "complete" => "complete",
            "incomplete" => "incomplete",
            _ => "all"
        };

        // Select which packages should be displayed in the table.
        Packages = Filter switch
        {
            "complete" => allPackages
                .Where(package => package.State == PackageState.Complete)
                .ToArray(),

            "incomplete" => allPackages
                .Where(package => package.State == PackageState.Incomplete)
                .ToArray(),

            _ => allPackages
        };
    }
    // Handles the POST request to add a new package.
    public async Task<IActionResult> OnPostAddAsync(CancellationToken cancellationToken)
    {
        var result = await fileShareService.AddPackageAsync(PackageFile, cancellationToken);
        SetMessage(result);
        return RedirectToPage();
    }
    //  
    public IActionResult OnPostRemove(string? identifier)
    {
        var result = fileShareService.RemovePackage(identifier);
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
