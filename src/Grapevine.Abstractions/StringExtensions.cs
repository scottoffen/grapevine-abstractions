using System.Text;

namespace Grapevine.Abstractions;

internal static class StringExtensions
{
    /// <summary>
    /// Returns true if the string contains any of the provided values, ignoring case.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool ContainsAny(this string? source, params string[] values)
    {
        return source.ContainsAny(values, StringComparison.CurrentCultureIgnoreCase);
    }

    /// <summary>
    /// Returns true if the string contains any of the provided values, using the specified comparison.
    /// This is useful for case-sensitive comparisons or other culture-specific comparisons.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="comparison"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public static bool ContainsAny(this string? source, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase, params string[] values)
    {
        return source.ContainsAny(values, comparison);
    }

    /// <summary>
    /// Returns true if the string contains any of the provided values, using the specified comparison.
    /// This is useful for case-sensitive comparisons or other culture-specific comparisons.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="values"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    public static bool ContainsAny(this string? source, IEnumerable<string>? values, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
    {
        if (string.IsNullOrEmpty(source) || values == null)
            return false;

        foreach (var value in values)
        {
            if (source.ContainsIgnoreCase(value, comparison))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Returns true if the string contains the provided value, ignoring case.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="value"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    public static bool ContainsIgnoreCase(this string? source, string? value, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
    {
        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(value))
            return false;

        return source!.IndexOf(value, comparison) >= 0;
    }

    /// <summary>
    /// Returns true if the string ends with any of the provided suffixes.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="suffixes"></param>
    /// <returns></returns>
    public static bool EndsWithAny(this string value, params string[] suffixes)
    {
        foreach (var suffix in suffixes)
        {
            if (value.EndsWith(suffix, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns true if the string starts with any of the provided prefixes.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="prefixes"></param>
    /// <returns></returns>
    public static bool StartsWithAny(this string value, params string[] prefixes)
    {
        foreach (var prefix in prefixes)
        {
            if (value.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Trims leading and trailing slashes and whitespace from the provided string, normalizing multiple slashes to a single slash.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string TrimPath(this string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // Step 1: Trim whitespace
        var trimmed = input!.Trim();

        // Step 2: Trim leading slashes
        var start = 0;
        while (start < trimmed.Length && trimmed[start] == '/')
            start++;

        // Step 3: Trim trailing slashes
        var end = trimmed.Length - 1;
        while (end >= start && trimmed[end] == '/')
            end--;

        if (start > end)
            return string.Empty;

        // Step 4: Normalize multiple slashes to single slash
        var sb = new StringBuilder(trimmed.Length);
        var previousWasSlash = false;

        for (var i = start; i <= end; i++)
        {
            var c = trimmed[i];

            if (c == '/')
            {
                if (!previousWasSlash)
                {
                    sb.Append('/');
                    previousWasSlash = true;
                }
                // else skip the slash
            }
            else
            {
                sb.Append(c);
                previousWasSlash = false;
            }
        }

        return sb.ToString();
    }
}
