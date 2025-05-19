namespace Grapevine.Abstractions.RouteConstraints;

public static class LengthPatternResolver
{
    /// <summary>
    /// Resolves a length constraint argument to a regular expression quantifier string like "+", "{3}", "{1,}", or "{1,5}".
    /// <para>
    /// Accepts the following formats:
    /// <list type="bullet">
    ///     <item><term>null or empty</term> any length (equivalent to "+")</item>
    ///     <item><term>"3"</term> exactly 3 characters</item>
    ///     <item><term>"1,"</term> at least 1 character</item>
    ///     <item><term>",5"</term> up to 5 characters (minimum 1 unless <paramref name="allowZeroMin"/> is true)</item>
    ///     <item><term>"1,5"</term> between 1 and 5 characters</item>
    /// </list>
    /// </para>
    /// <param name="args">
    /// A string representing a length constraint. Can be null, empty, a single integer, or a range in "min,max" format.
    /// </param>
    /// <param name="allowZeroMin">
    /// If true, allows a minimum value of zero when parsing the left side of the range or single integer.
    /// </param>
    /// </summary>
    /// <returns>A regex quantifier string suitable for appending to a character pattern (e.g. "[a-z]{1,5}")</returns>
    /// <exception cref="ArgumentException">Thrown when the input is invalid or improperly formatted</exception>
    public static string Resolve(string? args, bool allowZeroMin = false)
    {
        if (string.IsNullOrWhiteSpace(args))
            return "+";

        var min = allowZeroMin ? 0 : 1;
        int? max = null;

        var commaIndex = args!.IndexOf(',');
        if (commaIndex >= 0)
        {
            var left = args.Substring(0, commaIndex);
            var right = args.Substring(commaIndex + 1);

            if (string.IsNullOrWhiteSpace(left) && string.IsNullOrWhiteSpace(right))
                throw new ArgumentException($"Invalid argument '{args}' for length constraint.");

            if (!string.IsNullOrWhiteSpace(left))
            {
                if (!int.TryParse(left, out var minParsed) || (minParsed < 1 && !allowZeroMin))
                    throw new ArgumentException($"Invalid argument '{args}' for length constraint.");

                min = minParsed;
            }

            if (!string.IsNullOrWhiteSpace(right))
            {
                if (!int.TryParse(right, out var maxParsed) || maxParsed < 1)
                    throw new ArgumentException($"Invalid argument '{args}' for length constraint.");

                max = maxParsed;
            }

            if (max != null && max < min)
                throw new ArgumentException($"Invalid argument '{args}' for length constraint.");

            return max.HasValue
                ? $"{{{min},{max}}}"
                : $"{{{min},}}";
        }
        else
        {
            // Exact match
            if (!int.TryParse(args, out var exact) || exact < 1)
                throw new ArgumentException($"Invalid argument '{args}' for length constraint.");

            return $"{{{exact}}}";
        }
    }
}
