namespace Grapevine.Abstractions.RouteConstraints;

public static class DefaultResolver
{
    private static readonly string _pattern = @"[^/]";

    /// <summary>
    /// Resolves the default route constraint to a regular expression pattern that matches any non-slash characters.
    /// <para>
    /// Supports optional length constraints:
    /// <list type="bullet">
    ///     <item><term>null or empty</term> matches one or more non-slash characters</item>
    ///     <item><term>3</term> matches exactly 3 characters</item>
    ///     <item><term>1,</term> matches at least 1 character</item>
    ///     <item><term>,5</term> matches up to 5 characters</item>
    ///     <item><term>1,5</term> matches between 1 and 5 characters</item>
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
            return $"(?<{name}>{_pattern}+)";

        var length = LengthPatternResolver.Resolve(args);
        return $"(?<{name}>{_pattern}{length})";
    }
}
