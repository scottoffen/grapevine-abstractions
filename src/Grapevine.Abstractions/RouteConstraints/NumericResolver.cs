namespace Grapevine.Abstractions.RouteConstraints;

public static class NumericResolver
{
    private static readonly string _pattern = @"\d";
    private static readonly string _unbound = @"\d+";

    /// <summary>
    /// Resolves the <term>numeric</term> route constraint to a regular expression pattern that matches one or more digits (0–9).
    /// <para>
    /// Supports optional length constraints on the number of digits:
    /// <list type="bullet">
    ///     <item><term>null or empty</term> matches any number of digits</item>
    ///     <item><term>3</term> matches exactly 3 digits</item>
    ///     <item><term>1,</term> matches at least 1 digit</item>
    ///     <item><term>,5</term> matches up to 5 digits</item>
    ///     <item><term>1,5</term> matches between 1 and 5 digits</item>
    /// </list>
    /// </para>
    /// </summary>
    /// <param name="name"/>
    /// <param name="args"/>
    /// <returns>A regular expression pattern string wrapped in a named capture group</returns>
    /// <exception cref="ArgumentException">Thrown if the length argument is invalid</exception>
    public static string Resolve(string name, string? args)
    {
        if (string.IsNullOrWhiteSpace(args))
            return $"(?<{name}>{_unbound})";

        var length = LengthPatternResolver.Resolve(args);
        return $"(?<{name}>{_pattern}{length})";
    }
}
