namespace Grapevine.Abstractions;

public class RemoteAddress
{
    /// <summary>
    /// Returns the IP address of the remote host
    /// </summary>
    public string Address { get; }

    /// <summary>
    /// Returns the port number of the remote host
    /// </summary>
    public int Port { get; }

    public RemoteAddress(string address, int port)
    {
        Address = address;
        Port = port;
    }

    public override string ToString() => $"{Address}:{Port}";

    public static RemoteAddress Parse(string address)
    {
        var parts = address.Split(':');
        if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || !int.TryParse(parts[1], out var port))
        {
            throw new FormatException($"Invalid remote address format: {address}");
        }

        return new RemoteAddress(parts[0], port);
    }
}
