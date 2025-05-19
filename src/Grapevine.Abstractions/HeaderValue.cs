using System.Diagnostics;
using System.Globalization;

namespace Grapevine.Abstractions;

/// <summary>
/// Represents a single HTTP header value with an optional quality value.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public class HeaderValue : IEquatable<HeaderValue>, IComparable<HeaderValue>
{
    /// <summary>
    /// The value of the header.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// The optional quality value of the header.
    /// </summary>
    public double? Quality { get; }

    public HeaderValue(string value, double? quality = null)
    {
        Value = value?.Trim() ?? throw new ArgumentNullException(nameof(value));

        if (quality.HasValue && (quality < 0 || quality > 1))
        {
            throw new ArgumentOutOfRangeException(nameof(quality), "Quality must be between 0 and 1.");
        }

        Quality = quality;
    }

    /// <summary>
    /// Returns a string representation of the header value in the format of "value;q=quality".
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Quality.HasValue
            ? $"{Value};q={Quality.Value.ToString("0.###", CultureInfo.InvariantCulture)}"
            : Value;
    }

    public bool Equals(HeaderValue? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase) && Nullable.Equals(Quality, other.Quality);
    }

    public override bool Equals(object? obj)
    {
        return obj is HeaderValue other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
            hash = (hash * 397) ^ (Quality?.GetHashCode() ?? 0);
            return hash;
        }
    }

    /// <summary>
    /// Compares this instance to another <see cref="HeaderValue"/> based on descending <term>Quality</term> and ascending <term>Value</term>.
    /// </summary>
    public int CompareTo(HeaderValue? other)
    {
        if (other is null) return -1;

        // Sort by Quality descending
        var thisQuality = Quality ?? 1.0;
        var otherQuality = other.Quality ?? 1.0;

        var qualityComparison = otherQuality.CompareTo(thisQuality);
        if (qualityComparison != 0) return qualityComparison;

        // Tiebreaker: Value ascending
        return string.Compare(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public static bool operator ==(HeaderValue? left, HeaderValue? right) =>
        Equals(left, right);

    public static bool operator !=(HeaderValue? left, HeaderValue? right) =>
        !Equals(left, right);
}
