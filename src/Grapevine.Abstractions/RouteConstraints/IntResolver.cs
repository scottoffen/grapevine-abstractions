namespace Grapevine.Abstractions.RouteConstraints;

public static class IntResolver
{
    private static readonly string _pattern = @"-?\d";
    private static readonly string _unbound = @"-?\d+";

    /// <summary>
    /// Resolves the int route constraint to a regular expression pattern that matches signed or unsigned integers.
    /// <para>
    /// Supports optional length constraints on the number of digits (excluding the optional minus sign):
    /// <list type="bullet">
    ///     <item><term>null or empty</term> matches integers of any length</item>
    ///     <item><term>3</term> matches integers with exactly 3 digits</item>
    ///     <item><term>1,</term> matches integers with at least 1 digit</item>
    ///     <item><term>,5</term> matches integers with up to 5 digits</item>
    ///     <item><term>1,5</term> matches integers with between 1 and 5 digits</item>
    /// </list>
    /// </para>
    /// </summary>
    /// <param name="name"/>
    /// <param name="args"/>
    /// <returns>A regular expression pattern string wrapped in a named capture group</returns>
    /// <exception cref="ArgumentException">Thrown if the digit length argument is invalid</exception>
    public static string Resolve(string name, string? args)
    {
        if (string.IsNullOrWhiteSpace(args))
            return $"(?<{name}>{_unbound})";

        var length = LengthPatternResolver.Resolve(args);
        return $"(?<{name}>{_pattern}{length})";
    }
}
