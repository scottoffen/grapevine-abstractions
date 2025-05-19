using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Grapevine.Abstractions;

/// <summary>
/// Represents a single HTTP header, either from a request or response, with optional values and quality values.
/// </summary>
[DebuggerDisplay("{Name} {Values.Count} value(s)")]
public class Header : IEquatable<Header>
{
    private readonly List<HeaderValue> _values = new();

    /// <summary>
    /// The name of the header.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The value of the header, if any, or the first value if there are multiple values.
    /// </summary>
    public string? Value => _values.FirstOrDefault()?.ToString();

    /// <summary>
    /// All values of the header, including optional quality values.
    /// </summary>
    public IReadOnlyList<HeaderValue> Values => _values.AsReadOnly();

    /// <summary>
    /// Indicates whether the header has a value.
    /// </summary>
    public bool HasValue => _values.Count > 0;

    /// <summary>
    /// Indicates whether the header has multiple values.
    /// </summary>
    public bool IsMultipart => _values.Count > 1;

    public Header(string name, string? value = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ParseValues(value);
    }

    public override string ToString()
    {
        var header = new StringBuilder(Name);

        if (_values.Count > 0)
        {
            header.Append(": ");
            header.Append(string.Join(", ", _values));
        }

        return header.ToString();
    }

    public bool Equals(Header? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase) &&
               _values.SequenceEqual(other._values);
    }

    public override bool Equals(object? obj)
    {
        return obj is Header other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = StringComparer.OrdinalIgnoreCase.GetHashCode(Name);

            foreach (var val in _values)
            {
                hash = (hash * 397) ^ val.GetHashCode();
            }

            return hash;
        }
    }

    private void ParseValues(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return;

        var segments = value!.Split(',');
        foreach (var segment in segments)
        {
            var trimmed = segment.Trim();
            if (trimmed.Length == 0) continue;

            var semiIndex = trimmed.IndexOf(';');
            if (semiIndex >= 0)
            {
                var val = trimmed.Substring(0, semiIndex).Trim();
                var qPart = trimmed.Substring(semiIndex + 1).Trim();

                if (qPart.StartsWith("q=", StringComparison.OrdinalIgnoreCase) &&
                    double.TryParse(qPart.Substring(2), NumberStyles.Float, CultureInfo.InvariantCulture, out var q))
                {
                    _values.Add(new HeaderValue(val, q));
                }
                else
                {
                    _values.Add(new HeaderValue(val));
                }
            }
            else
            {
                _values.Add(new HeaderValue(trimmed));
            }
        }
    }


    public static implicit operator string(Header header) => header.ToString();

    public static bool operator ==(Header? left, Header? right) =>
        Equals(left, right);

    public static bool operator !=(Header? left, Header? right) =>
        !Equals(left, right);
}
