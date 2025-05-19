using System.Text.RegularExpressions;

namespace Grapevine.Abstractions.RouteConstraints;

public static class RegexResolver
{
    /// <summary>
    /// Resolves the regex route constraint to a regular expression pattern defined by the user.
    /// <para>
    /// The provided pattern must:
    /// <list type="bullet">
    ///     <item><term>Be non-empty</term> and trimmed</item>
    ///     <item><term>Not contain any capture groups</term> other than the outer named one</item>
    ///     <item><term>Not be anchored</term> — must not start with "^" or end with "$"</item>
    /// </list>
    /// </para>
    /// </summary>
    /// <param name="name"/>
    /// <param name="args"/>
    /// <returns>A regular expression pattern string wrapped in a named capture group</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the pattern is null, empty, anchored, contains capture groups, or fails to compile
    /// </exception>
    public static string Resolve(string name, string? args)
    {
        if (string.IsNullOrWhiteSpace(args))
            throw new ArgumentException("Regex constraint requires a non-empty pattern.", nameof(args));

        var pattern = args!.Trim();

        if (IsAnchored(pattern))
            throw new ArgumentException("Regex constraint must not start with ^ or end with $.", nameof(args));

        Regex regex;

        try
        {
            regex = new Regex(pattern);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Invalid regular expression pattern.", nameof(args), ex);
        }

        if (regex.GetGroupNumbers().Length > 1)
            throw new ArgumentException("Regex constraint must not contain any capture groups.", nameof(args));

        return $"(?<{name}>{pattern})";
    }

    private static bool IsAnchored(string pattern)
    {
        return pattern.StartsWith("^") || pattern.EndsWith("$");
    }
}
