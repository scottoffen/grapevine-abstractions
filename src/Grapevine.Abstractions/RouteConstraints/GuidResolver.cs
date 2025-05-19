namespace Grapevine.Abstractions.RouteConstraints;

public static class GuidResolver
{
    private static readonly string _pattern = @"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";

    /// <summary>
    /// Resolves the <term>guid</term> route constraint to a regular expression pattern that matches a 36-character canonical GUID (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).
    /// This constraint does not support arguments.
    /// </summary>
    /// <param name="name"/>
    /// <param name="args"/>
    /// <returns>A regular expression pattern string wrapped in a named capture group</returns>
    /// <exception cref="ArgumentException">Thrown if an argument is provided</exception>
    public static string Resolve(string name, string? args)
    {
        if (!string.IsNullOrWhiteSpace(args))
            throw new ArgumentException("Constraint type 'guid' does not accept any arguments.", nameof(args));

        return $"(?<{name}>{_pattern})";
    }
}
