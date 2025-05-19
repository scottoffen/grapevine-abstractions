namespace Grapevine.Abstractions.RouteConstraints;

public static class DateTimeResolver
{
    private static readonly Dictionary<string, string> _patterns = new(StringComparer.OrdinalIgnoreCase)
    {
        // ISO 8601: 2023-05-21T14:30:00
        ["iso"] = @"\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}",

        // Date only: 2023-05-21
        ["date"] = @"\d{4}-\d{2}-\d{2}",

        // Time only: 14:30:00
        ["time"] = @"\d{2}:\d{2}:\d{2}",

        // YearMonthDay (no separators): 20230521
        ["basic"] = @"\d{8}",

        // RFC1123 (very loosely): Sun, 21 May 2023 14:30:00 GMT
        ["rfc"] = @"[A-Za-z]{3}, \d{2} [A-Za-z]{3} \d{4} \d{2}:\d{2}:\d{2} GMT"
    };

    /// <summary>
    /// Resolves the datetime route constraint to a regular expression pattern that matches common date and time formats.
    /// <para>
    /// Supports the following formats via arguments:
    /// <list type="bullet">
    ///     <item><term>null or empty</term> ISO format with time (yyyy-MM-ddTHH:mm:ss)</item>
    ///     <item><term>iso</term> 2023-05-21T14:30:00</item>
    ///     <item><term>date</term> 2023-05-21</item>
    ///     <item><term>time</term> 14:30:00</item>
    ///     <item><term>basic</term> 20230521 (compact date format)</item>
    ///     <item><term>rfc</term> Sun, 21 May 2023 14:30:00 GMT</item>
    /// </list>
    /// </para>
    /// </summary>
    /// <param name="name"/>
    /// <param name="args"/>
    /// <returns>A regular expression pattern string wrapped in a named capture group</returns>
    /// <exception cref="ArgumentException">Thrown if the argument is not a supported datetime format</exception>
    public static string Resolve(string name, string? args)
    {
        if (string.IsNullOrWhiteSpace(args))
            return $"(?<{name}>{_patterns["iso"]})";

        if (_patterns.TryGetValue(args!.Trim(), out var value))
            return $"(?<{name}>{value})";

        throw new ArgumentException($"The 'datetime' constraint does not support the argument '{args}'. Supported values: {string.Join(", ", _patterns.Keys)}.", nameof(args));
    }
}
