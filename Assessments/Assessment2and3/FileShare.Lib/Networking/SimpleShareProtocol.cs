namespace FileShare.Lib.Networking;

using System.Buffers.Binary;
using System.Text;

public enum SimpleShareCode : ushort
{
    Pog = 0x00,
    Ida = 0x01,
    Idr = 0x02,
    Dsn = 0x03,
    Err = 0x04,
    Req = 0x06,
    Res = 0x07,
    Acp = 0x09,
    Ack = 0x0c,
    Png = 0xff
}

public sealed record SimpleShareMessage(SimpleShareCode Code, byte[] Payload)
{
    public const int HeaderSize = 4;
    public const int MaxMessageSize = 4096;
    public const int MaxPayloadSize = MaxMessageSize - HeaderSize;
}

/// <summary>Little-endian encoder/decoder for the simple-share TCP framing.</summary>
public static class SimpleShareProtocol
{
    public static async Task WriteAsync(Stream stream, SimpleShareMessage message, CancellationToken cancellationToken = default)
    {
        if (message.Payload.Length > SimpleShareMessage.MaxPayloadSize)
            throw new InvalidDataException("simple-share message exceeds 4096 bytes.");

        var header = new byte[4];
        BinaryPrimitives.WriteUInt16LittleEndian(header.AsSpan(0, 2), (ushort)message.Code);
        BinaryPrimitives.WriteUInt16LittleEndian(header.AsSpan(2, 2), checked((ushort)message.Payload.Length));
        await stream.WriteAsync(header, cancellationToken);
        if (message.Payload.Length > 0) await stream.WriteAsync(message.Payload, cancellationToken);
        await stream.FlushAsync(cancellationToken);
    }

    public static async Task<SimpleShareMessage?> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var header = new byte[4];
        if (!await ReadExactlyOrEofAsync(stream, header, cancellationToken)) return null;
        var code = (SimpleShareCode)BinaryPrimitives.ReadUInt16LittleEndian(header.AsSpan(0, 2));
        var length = BinaryPrimitives.ReadUInt16LittleEndian(header.AsSpan(2, 2));
        if (length > SimpleShareMessage.MaxPayloadSize) throw new InvalidDataException("Invalid simple-share message length.");
        var payload = new byte[length];
        if (length > 0 && !await ReadExactlyOrEofAsync(stream, payload, cancellationToken))
            throw new EndOfStreamException("Peer disconnected during a simple-share message.");
        return new SimpleShareMessage(code, payload);
    }

    public static byte[] EncodeIdentifier(string identifier)
    {
        var result = new byte[1024];
        var source = Encoding.UTF8.GetBytes(identifier ?? string.Empty);
        if (source.Length > result.Length) throw new ArgumentException("Identifier is larger than 1024 bytes.", nameof(identifier));
        source.CopyTo(result, 0);
        return result;
    }

    public static string DecodeIdentifier(ReadOnlySpan<byte> bytes)
    {
        var end = bytes.IndexOf((byte)0);
        if (end < 0) end = bytes.Length;
        return Encoding.UTF8.GetString(bytes[..end]).Trim();
    }

    private static async Task<bool> ReadExactlyOrEofAsync(Stream stream, byte[] buffer, CancellationToken cancellationToken)
    {
        var offset = 0;
        while (offset < buffer.Length)
        {
            var read = await stream.ReadAsync(buffer.AsMemory(offset), cancellationToken);
            if (read == 0) return offset == 0 ? false : throw new EndOfStreamException();
            offset += read;
        }
        return true;
    }
}
