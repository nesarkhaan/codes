using FileShare.Abstractions.Interfaces;
using FileShare.Lib;
using FileShare.Lib.Adapters;
using FileShare.Lib.Configuration;
using FileShare.Lib.Data;
using FileShare.Lib.Networking;
using FileShare.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

FileShareConfig config;

// Load the configuration first. If this fails, the daemon cannot start.
try
{
    var configPath = Path.Combine(AppContext.BaseDirectory, "config.conf");
    config = ConfigurationLoader.Load(configPath);
}
catch (Exception ex)
{
    Console.WriteLine("\n Failed to Start Daemon: ");
    Console.WriteLine(ex.Message);
    return;
}

// Set up the existing generic host for the daemon.
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton(config);

        // FileShare depends on the abstraction; this selects FileValidate at startup.
        services.AddSingleton<IFileValidationAdapter, TideMetadataAdapter>();
        services.AddSingleton<P2PFileManager>();
        services.AddSingleton<PeerNetworkService>();

        services.AddDbContextFactory<FileShareContext>(options => options.UseSqlite(FileShareDatabase.ConnectionString));
        services.AddSingleton<FileShareSnapshotStore>();

        services.AddSingleton<IpcServer>();
        services.AddHostedService<DaemonWorker>();
    })
    .Build();

Console.WriteLine("FileShare Daemon is starting...");
host.Run();
