using System.Collections.Specialized;
using System.Diagnostics;

namespace Grapevine.Abstractions;

/// <summary>
/// Represents a collection of key/value pairs that are passed in the query string of a request
/// </summary>
[DebuggerDisplay("{ToString()}")]
public class QueryParams : NameValueCollection
{
    /// <summary>
    /// Initializes a new instance of the <term>QueryParams</term> class with no key/value pairs.
    /// </summary>
    public QueryParams() { }

    /// <summary>
    /// Initializes a new instance of the <term>QueryParams</term> class with the contents of the specified <term>NameValueCollection</term>.
    /// </summary>
    public QueryParams(NameValueCollection collection) : base(collection) { }

    /// <summary>
    /// Initializes a new instance of the <term>QueryParams</term> class with the specified collection of key/value pairs.
    /// </summary>
    public QueryParams(IEnumerable<KeyValuePair<string, string?>> items)
    {
        foreach (var item in items)
        {
            Add(item.Key, item.Value);
        }
    }

    /// <summary>
    /// Returns a URL-encoded query string representation of the key/value pairs in the collection.
    /// </summary>
    public override string ToString()
    {
        return string.Join("&", AllKeys.SelectMany(k =>
        {
            var values = GetValues(k);
            return values is null
                ? Enumerable.Empty<string>()
                : values.Select(v => $"{k}={v}");
        }));
    }

    /// <summary>
    /// Parses a query string into a new instance of the <term>QueryParams</term> class.
    /// </summary>
    /// <param name="queryString">The raw query string to parse, with or without a leading <c>?</c> character.</param>
    /// <returns>A new <term>QueryParams</term> instance containing the parsed key/value pairs.</returns>
    public static QueryParams Parse(string queryString)
    {
        var result = new QueryParams();

        if (string.IsNullOrEmpty(queryString)) return result;

        var length = queryString.Length;
        var i = queryString[0] == '?' ? 1 : 0;

        while (i < length)
        {
            var keyStart = i;
            var keyEnd = -1;
            var valueStart = -1;

            while (i < length)
            {
                var c = queryString[i];
                if (c == '=' && keyEnd < 0)
                {
                    keyEnd = i;
                    valueStart = ++i;
                }
                else if (c == '&')
                {
                    break;
                }
                else
                {
                    i++;
                }
            }

            var key = Uri.UnescapeDataString(queryString.Substring(keyStart, (keyEnd >= 0 ? keyEnd : i) - keyStart));
            var value = (keyEnd >= 0)
                ? Uri.UnescapeDataString(queryString.Substring(valueStart, i - valueStart))
                : null;

            if (!string.IsNullOrWhiteSpace(key)) result.Add(key, value);
            if (i < length && queryString[i] == '&') i++;
        }

        return result;
    }
}
