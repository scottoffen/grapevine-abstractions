using System.Diagnostics;
using System.Globalization;

namespace Grapevine.Abstractions;

/// <summary>
/// Represents a network endpoint as an IP address and a port number
/// </summary>
[DebuggerDisplay("{Address}:{Port}")]
public class ConnectionInfo
{
    public static readonly int MinPort = 0;

    public static readonly int MaxPort = 65535;

    /// <summary>
    /// Gets the IP address or hostname of the client that made the request.
    /// </summary>
    public string Address { get; }

    /// <summary>
    /// Gets the port number of the client that made the request.
    /// </summary>
    public int Port { get; }

    public ConnectionInfo(string address, int port)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException("Address cannot be null or empty.", nameof(address));
        }

        if (!IsValidAddress(address))
        {
            throw new ArgumentException("Address contains invalid characters.", nameof(address));
        }

        if (port < MinPort || port > MaxPort)
        {
            throw new ArgumentOutOfRangeException(nameof(port), $"Port must be between {MinPort} and {MaxPort}.");
        }

        Address = address;
        Port = port;
    }

    public override string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "{0}:{1}", Address, Port);
    }

    private static bool IsValidAddress(string address)
    {
        foreach (var c in address)
        {
            if (char.IsLetterOrDigit(c)) continue;
            if ("-._:[]".IndexOf(c) >= 0) continue; // allow hyphen, period, colon, brackets (IPv6), underscore

            return false;
        }

        return true;
    }
}
