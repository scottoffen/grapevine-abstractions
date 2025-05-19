using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace Grapevine.Abstractions;

/// <summary>
/// Represents a collection of HTTP headers that supports duplicate entries
/// and provides enum-based access for known headers.
/// </summary>
[DebuggerDisplay("{Count}")]
public class HeaderCollection<TEnum> : ICollection<Header>, IEnumerable<Header>, IEnumerable, IReadOnlyCollection<Header> where TEnum : struct, Enum
{
    private static readonly Dictionary<TEnum, string> _headerStrings = new();

    static HeaderCollection()
    {
        if (!typeof(TEnum).IsEnum)
            throw new InvalidOperationException($"Type parameter '{typeof(TEnum).Name}' must be an enum.");

        foreach (var value in Enum.GetValues(typeof(TEnum)).Cast<TEnum>())
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<ToStringAttribute>();
            if (attribute != null)
            {
                _headerStrings.Add(value, attribute.Value);
            }
            else
            {
                _headerStrings.Add(value, value.ToString());
            }
        }
    }

    private readonly List<Header> _headers = new();

    /// <summary>
    /// Gets the number of headers in the collection
    /// </summary>
    public int Count => _headers.Count;

    /// <summary>
    /// Gets a value indicating whether the collection is read-only
    /// </summary>
    /// <returns>Always returns false</returns>
    public bool IsReadOnly => false;

    /// <summary>
    /// Adds a header to the collection
    /// </summary>
    /// <param name="header"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Add(Header header)
    {
        if (header == null)
            throw new ArgumentNullException(nameof(header));
        _headers.Add(header);
    }

    /// <summary>
    /// Adds a header to the collection
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Add(TEnum key, string value) => Add(new Header(_headerStrings[key], value));

    /// <summary>
    /// Adds a header to the collection
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Add(string key, string value) => Add(new Header(key, value));

    /// <summary>
    /// Adds a range of headers to the collection
    /// </summary>
    /// <param name="headers"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void AddRange(IEnumerable<Header> headers)
    {
        if (headers == null)
            throw new ArgumentNullException(nameof(headers));
        _headers.AddRange(headers);
    }

    /// <summary>
    /// Removes all headers from the collection
    /// </summary>
    public void Clear() => _headers.Clear();

    /// <summary>
    /// Determines whether the collection contains a specific header
    /// </summary>
    /// <param name="header"></param>
    /// <returns></returns>
    public bool Contains(Header header) => _headers.Contains(header);

    /// <summary>
    /// Determines whether the collection contains a specific header
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool Contains(TEnum key)
    {
        var target = _headerStrings[key];

        for (var i = 0; i < _headers.Count; i++)
        {
            if (_headers[i].Name.Equals(target, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Copies the elements of the collection to an array, starting at a particular array index
    /// </summary>
    /// <param name="array"></param>
    /// <param name="arrayIndex"></param>
    public void CopyTo(Header[] array, int arrayIndex) => _headers.CopyTo(array, arrayIndex);

    /// <summary>
    /// Copies the elements of the collection to an array, starting at a particular array index
    /// </summary>
    /// <param name="array"></param>
    /// <param name="index"></param>
    /// <exception cref="ArgumentException">Thrown when the destination array is not of type <see cref="Header"/>[]</exception>
    public void CopyTo(Array array, int index)
    {
        if (array is Header[] headers)
            _headers.CopyTo(headers, index);
        else
            throw new ArgumentException("Array must be of type Header[]", nameof(array));
    }

    /// <summary>
    /// Returns an enumeration of headers whose name matches the specified enum key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public IEnumerable<Header> GetAll(TEnum key)
    {
        var name = _headerStrings[key];
        foreach (var header in GetAll(name))
        {
            yield return header;
        }
    }

    /// <summary>
    /// Returns an enumeration of headers whose name matches the specified value
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IEnumerable<Header> GetAll(string name)
    {
        foreach (var header in _headers)
        {
            if (header.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                yield return header;
            }
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection
    /// </summary>
    /// <returns></returns>
    public IEnumerator<Header> GetEnumerator() => _headers.GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through the collection
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Removes the first occurrence of a specific header from the collection
    /// </summary>
    /// <param name="header"></param>
    /// <returns></returns>
    public bool Remove(Header header) => _headers.Remove(header);

    /// <summary>
    /// Removes all occurrence of a specific header from the collection
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool RemoveAll(TEnum key) => RemoveAll(_headerStrings[key]);

    /// <summary>
    /// Removes all occurrence of a specific header from the collection
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool RemoveAll(string key)
    {
        var removed = false;

        for (var i = _headers.Count - 1; i >= 0; i--)
        {
            if (_headers[i].Name.Equals(key, StringComparison.OrdinalIgnoreCase))
            {
                _headers.RemoveAt(i);
                removed = true;
            }
        }

        return removed;
    }

    /// <summary>
    /// Attempts to retrieve the first header whose name matches the specified enum key.
    /// </summary>
    /// <param name="key">The enum key to search for.</param>
    /// <param name="header">The first matching header, if found.</param>
    /// <returns>True if a match is found; otherwise, false.</returns>
    public bool TryGetValue(TEnum key, out Header? header)
    {
        return TryGetValue(_headerStrings[key], out header);
    }

    /// <summary>
    /// Attempts to retrieve the first header whose name matches the specified value.
    /// </summary>
    /// <param name="name">The header name to search for.</param>
    /// <param name="header">The first matching header, if found.</param>
    /// <returns>True if a match is found; otherwise, false.</returns>
    public bool TryGetValue(string name, out Header? header)
    {
        for (var i = 0; i < _headers.Count; i++)
        {
            if (_headers[i].Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                header = _headers[i];
                return true;
            }
        }

        header = null;
        return false;
    }

    /// <summary>
    /// Gets the first header associated with the specified enum key, using a case-insensitive match.
    /// </summary>
    public Header? this[TEnum key]
    {
        get
        {
            var name = _headerStrings[key];
            return this[name];
        }
    }

    /// <summary>
    /// Gets the first header associated with the specified name, using a case-insensitive match.
    /// </summary>
    public Header? this[string name]
    {
        get
        {
            foreach (var header in _headers)
            {
                if (header.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return header;
            }

            return null;
        }
    }
}
