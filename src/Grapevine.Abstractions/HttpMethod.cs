using System.Collections.Concurrent;
using System.Diagnostics;

namespace Grapevine.Abstractions;

[DebuggerDisplay("{Name}")]
public partial class HttpMethod : IEquatable<HttpMethod>
{
    /// <summary>
    /// Returns the uppercase name of the HTTP method (e.g., "GET", "POST").
    /// </summary>
    public string Name { get; }

    private HttpMethod(string name)
    {
        Name = name.Trim().ToUpperInvariant();
    }

    public bool Equals(HttpMethod? other)
    {
        return other is not null && Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return obj is HttpMethod other && Equals(other);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
    }

    /// <summary>
    /// Determines if the current <see cref="HttpMethod"/> object matches the specified <see cref="HttpMethod"/> object.
    /// </summary>
    /// <param name="method"></param>
    /// <returns>Returns true if the either object is <see cref="HttpMethod.Any"/> </returns>
    public bool Matches(HttpMethod? method)
    {
        return Equals(method) || method == Any || this == Any;
    }

    public override string ToString()
    {
        return Name;
    }
}

public partial class HttpMethod
{
    public static bool operator ==(HttpMethod? left, HttpMethod? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    public static bool operator !=(HttpMethod? left, HttpMethod? right)
    {
        return !(left == right);
    }

    public static implicit operator HttpMethod(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name), "HTTP method name cannot be null or empty.");
        }

        return Parse(name);
    }

    public static implicit operator string(HttpMethod method)
    {
        return method.Name;
    }
}

public partial class HttpMethod
{
    private static readonly ConcurrentDictionary<string, HttpMethod> _methods = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Returns a read-only collection of all known HTTP methods.
    /// </summary>
    public static IReadOnlyCollection<HttpMethod> Known => _methods.Values.ToArray();

    /// <summary>
    /// Parses a string representation of an HTTP method into a <see cref="HttpMethod"/> object.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static HttpMethod Parse(string name)
    {
        var method = new HttpMethod(name);
        return _methods.GetOrAdd(method.Name, method);
    }

    /// <summary>
    /// Represents an HTTP method that can be used with any HTTP request.
    /// </summary>
    public static readonly HttpMethod Any = Parse("*");

    /// <summary>
    /// Represents an HTTP CONNECT method.
    /// </summary>
    public static readonly HttpMethod Connect = Parse("Connect");

    /// <summary>
    /// Represents an HTTP DELETE method.
    /// </summary>
    public static readonly HttpMethod Delete = Parse("Delete");

    /// <summary>
    /// Represents an HTTP GET method.
    /// </summary>
    public static readonly HttpMethod Get = Parse("Get");

    /// <summary>
    /// Represents an HTTP HEAD method.
    /// </summary>
    public static readonly HttpMethod Head = Parse("Head");

    /// <summary>
    /// Represents an HTTP OPTIONS method.
    /// </summary>
    public static readonly HttpMethod Options = Parse("Options");

    /// <summary>
    /// Represents an HTTP PATCH method.
    /// </summary>
    public static readonly HttpMethod Patch = Parse("Patch");

    /// <summary>
    /// Represents an HTTP POST method.
    /// </summary>
    public static readonly HttpMethod Post = Parse("Post");

    /// <summary>
    /// Represents an HTTP PUT method.
    /// </summary>
    public static readonly HttpMethod Put = Parse("Put");

    /// <summary>
    /// Represents an HTTP TRACE method.
    /// </summary>
    public static readonly HttpMethod Trace = Parse("Trace");
}
