using System.IO.Pipes;

namespace FileShare.Lib.Networking;

// Shared client for the existing FileSharePipe text protocol.
public sealed class IpcClient
{
    public const string PipeName = "FileSharePipe";

    public async Task<string?> SendAsync(
        string command,
        CancellationToken cancellationToken = default)
    {
        using var pipeClient = new NamedPipeClientStream(
            ".",
            PipeName,
            PipeDirection.InOut,
            PipeOptions.Asynchronous);

        await pipeClient.ConnectAsync(3000, cancellationToken);

        using var reader = new StreamReader(pipeClient);
        using var writer = new StreamWriter(pipeClient) { AutoFlush = true };
        await writer.WriteLineAsync(command);
        return await reader.ReadLineAsync(cancellationToken);
    }
}
