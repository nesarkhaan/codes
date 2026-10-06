namespace FileShare.Web;

using FileShare.Lib.Data;
using FileShare.Lib.Networking;
using FileShare.Web.Services;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        builder.Services.AddSingleton<IFileShareUiService, FileShareUiService>();
        builder.Services.AddSingleton<IpcClient>();

        // Web reads the shared database; Service owns all writes.
        builder.Services.AddDbContextFactory<FileShareContext>(options => options.UseSqlite(FileShareDatabase.ConnectionString));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.MapRazorPages().WithStaticAssets();
        app.Run();
    }
}
