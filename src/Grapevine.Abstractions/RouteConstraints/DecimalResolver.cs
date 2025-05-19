namespace Grapevine.Abstractions.RouteConstraints;

public static class DecimalResolver
{
    private static readonly string _pattern = @"[-]?\d+(?:\.\d{length})?";
    private static readonly string _unbound = @"[-]?\d+(?:\.\d+)?";

    /// <summary>
    /// Resolves the decimal route constraint to a regular expression pattern that matches signed or unsigned decimal numbers.
    /// <para>
    /// Supports optional precision constraints on decimal places:
    /// <list type="bullet">
    ///     <item><term>null or empty</term> matches whole and fractional numbers of any precision</item>
    ///     <item><term>2</term> matches numbers with exactly 2 decimal places</item>
    ///     <item><term>1,</term> matches numbers with at least 1 decimal place</item>
    ///     <item><term>,3</term> matches numbers with up to 3 decimal places</item>
    ///     <item><term>1,3</term> matches numbers with between 1 and 3 decimal places</item>
    /// </list>
    /// </para>
    /// </summary>
    /// <param name="name"/>
    /// <param name="args"/>
    /// <returns>A regular expression pattern string wrapped in a named capture group</returns>
    /// <exception cref="ArgumentException">Thrown if the precision argument is invalid</exception>
    public static string Resolve(string name, string? args)
    {
        if (string.IsNullOrWhiteSpace(args))
            return $"(?<{name}>{_unbound})";

        var length = LengthPatternResolver.Resolve(args, true);
        return $"(?<{name}>{_pattern.Replace("{length}", length)})";
    }
}
