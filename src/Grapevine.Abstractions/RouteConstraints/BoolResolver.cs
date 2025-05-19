namespace Grapevine.Abstractions.RouteConstraints;

public static class BoolResolver
{
    /// <summary>
    /// Resolves the bool route constraint to a regular expression pattern that matches true or false.
    /// <para>This constraint does not support arguments.</para>
    /// </summary>
    /// <param name="name"/>
    /// <param name="args"/>
    /// <returns>A regular expression pattern string wrapped in a named capture group</returns>
    /// <exception cref="ArgumentException">Thrown if an argument is provided</exception>
    public static string Resolve(string name, string? args)
    {
        if (!string.IsNullOrWhiteSpace(args))
            throw new ArgumentException("The 'bool' constraint does not accept arguments.", nameof(args));

        return $"(?<{name}>true|false)";
    }
}
