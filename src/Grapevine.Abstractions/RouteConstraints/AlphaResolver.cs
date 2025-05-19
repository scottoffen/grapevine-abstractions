namespace Grapevine.Abstractions.RouteConstraints;

public static class AlphaResolver
{
    /// <summary>
    /// Resolves the alpha route constraint to a regular expression pattern that matches alphabetic characters.
    /// <para>
    /// Supports optional length constraints:
    /// <list type="bullet">
    ///     <item><term>null or empty</term> matches one or more letters</item>
    ///     <item><term>3</term> matches exactly 3 letters</item>
    ///     <item><term>1,</term> matches at least 1 letter</item>
    ///     <item><term>,5</term> matches up to 5 letters</item>
    ///     <item><term>1,5</term> matches between 1 and 5 letters</item>
    /// </list>
    /// </para>
    /// </summary>
    /// <param name="name">The name of the route parameter to use for the named capture group</param>
    /// <param name="args">An optional string specifying a length constraint for the match</param>
    /// <returns>A regular expression pattern string wrapped in a named capture group</returns>
    public static string Resolve(string name, string? args)
    {
        if (string.IsNullOrWhiteSpace(args))
            return $"(?<{name}>[a-zA-Z]+)";

        var length = LengthPatternResolver.Resolve(args);
        return $"(?<{name}>[a-zA-Z]{length})";
    }
}
