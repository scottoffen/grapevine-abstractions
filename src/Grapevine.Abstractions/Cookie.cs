using System.Diagnostics;
using System.Text;

namespace Grapevine.Abstractions;

[DebuggerDisplay("{Name}={Value}")]
public class Cookie
{
    private string _name = string.Empty;
    private string _value = string.Empty;
    private string _domain = string.Empty;
    private string _path = "/";
    private DateTime? _expires;

    /// <summary>
    /// Gets or sets the name of the cookie.
    /// Must not be null, empty, or contain invalid characters.
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Cookie name cannot be null, empty, or whitespace.", nameof(value));

            if (!IsValidToken(value))
                throw new ArgumentException("Invalid characters in cookie name.", nameof(value));

            _name = value;
        }
    }

    /// <summary>
    /// Gets or sets the value of the cookie.
    /// Must not be null and cannot contain the ';' character.
    /// </summary>
    public string Value
    {
        get => _value;
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.Contains(';'))
                throw new ArgumentException("Cookie value cannot contain the ';' character.", nameof(value));

            _value = value;
        }
    }

    /// <summary>
    /// Gets or sets the domain associated with the cookie.
    /// Must be a valid domain format (e.g., example.com).
    /// </summary>
    /// <remarks>
    /// A leading dot is not required but may be used to allow subdomain sharing.
    /// For example, setting Domain to "example.com" allows the cookie on www.example.com, api.example.com, etc.
    /// </remarks>
    public string Domain
    {
        get => _domain;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            if (Uri.CheckHostName(value) == UriHostNameType.Unknown)
                throw new ArgumentException("Invalid domain format.", nameof(value));

            _domain = value;
        }
    }

    /// <summary>
    /// Gets or sets the path associated with the cookie.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     The path restricts the scope of the cookie to requests under the specified path on the domain.
    ///     For example, setting Path to "/account" means the cookie will be sent only with requests to "/account" or any sub-paths like "/account/settings".
    ///     </para>
    ///     <para>Path must start with a forward slash ('/').</para>
    /// </remarks>
    public string Path
    {
        get => _path;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Path cannot be null or empty", nameof(value));

            if (!value.StartsWith("/"))
                throw new ArgumentException("Invalid path format. Path must start with '/'.", nameof(value));

            _path = value;
        }
    }

    /// <summary>
    /// Gets or sets the expiration date and time of the cookie in UTC.
    /// </summary>
    public DateTime? Expires
    {
        get => _expires;
        set
        {
            if (value.HasValue && value.Value.Kind != DateTimeKind.Utc)
                throw new ArgumentException("Expires must be in UTC format.", nameof(value));

            _expires = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the cookie should only be sent over secure (HTTPS) connections.
    /// </summary>
    public bool Secure { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the cookie is accessible only through HTTP(S) and not via client-side scripts.
    /// </summary>
    public bool HttpOnly { get; set; }

    /// <summary>
    /// Gets or sets the SameSite attribute of the cookie.
    /// This attribute controls whether the cookie is sent with cross-site requests.
    /// </summary>
    public SameSiteMode? SameSite { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Cookie"/> class with the specified name and value.
    /// </summary>
    /// <param name="name">The name of the cookie.</param>
    /// <param name="value">The value of the cookie.</param>
    public Cookie(string name, string value)
    {
        Name = name;
        Value = value;
    }

    /// <summary>
    /// Returns a string representation of the cookie suitable for HTTP headers.
    /// </summary>
    /// <returns>A string representing the cookie.</returns>
    public override string ToString()
    {
        var sb = new StringBuilder(Name.Length + Value.Length + 64);
        sb.Append($"{Name}={Value}");

        if (!string.IsNullOrEmpty(Domain))
            sb.Append($"; Domain={Domain}");

        if (!string.IsNullOrEmpty(Path))
            sb.Append($"; Path={Path}");

        if (Expires.HasValue)
            sb.Append($"; Expires={Expires.Value:R}");

        if (Secure)
            sb.Append("; Secure");

        if (HttpOnly)
            sb.Append("; HttpOnly");

        if (SameSite.HasValue)
            sb.Append($"; SameSite={SameSite.Value}");

        return sb.ToString();
    }

    private static bool IsValidToken(string input)
    {
        foreach (var c in input)
        {
            if (c <= 0x20 || c >= 0x7f || "()<>@,;:\\\"/[]?={} \t".IndexOf(c) >= 0)
                return false;
        }
        return true;
    }
}
