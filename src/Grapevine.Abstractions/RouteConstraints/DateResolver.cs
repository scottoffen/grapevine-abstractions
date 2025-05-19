namespace Grapevine.Abstractions.RouteConstraints;

public static class DateResolver
{
    private static readonly Dictionary<string, string> _patterns = new(StringComparer.OrdinalIgnoreCase)
    {
        // ISO: 2023-05-21
        ["iso"] = @"\d{4}-\d{2}-\d{2}",

        // Year-Month-Day: 2023/05/21
        ["ymd"] = @"\d{4}[-/]\d{2}[-/]\d{2}",

        // Month-Day-Year: 05/21/2023 or 05-21-2023
        ["mdy"] = @"\d{1,2}[-/]\d{1,2}[-/]\d{4}",

        // Day-Month-Year: 21/05/2023 or 21-05-2023
        ["dmy"] = @"\d{1,2}[-/]\d{1,2}[-/]\d{4}",

        // Compact: 20230521
        ["basic"] = @"\d{8}"
    };

    /// <summary>
    /// Resolves the date route constraint to a regular expression pattern that matches common date formats.
    /// <para>
    /// Supports the following formats via arguments:
    /// <list type="bullet">
    ///     <item><term>null or empty</term> ISO format (yyyy-MM-dd)</item>
    ///     <item><term>iso</term> 2023-05-21</item>
    ///     <item><term>ymd</term> 2023/05/21 or 2023-05-21</item>
    ///     <item><term>mdy</term> 05/21/2023 or 05-21-2023</item>
    ///     <item><term>dmy</term> 21/05/2023 or 21-05-2023</item>
    ///     <item><term>basic</term> 20230521 (compact format)</item>
    /// </list>
    /// </para>
    /// </summary>
    /// <param name="name"/>
    /// <param name="args"/>
    /// <returns>A regular expression pattern string wrapped in a named capture group</returns>
    /// <exception cref="ArgumentException">Thrown if the argument is not a supported date format</exception>
    public static string Resolve(string name, string? args)
    {
        if (string.IsNullOrWhiteSpace(args))
            return $"(?<{name}>{_patterns["iso"]})";

        if (_patterns.TryGetValue(args!.Trim(), out var value))
            return $"(?<{name}>{value})";

        throw new ArgumentException($"The 'date' constraint does not support the argument '{args}'. Supported values: {string.Join(", ", _patterns.Keys)}.", nameof(args));
    }
}
