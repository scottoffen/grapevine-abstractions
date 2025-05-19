using System.Collections;
using System.Diagnostics;

namespace Grapevine.Abstractions;

[DebuggerDisplay("{Count})")]
public class CookieCollection : ICollection<Cookie>, IEnumerable<Cookie>, IEnumerable, IReadOnlyCollection<Cookie>
{
    private readonly Dictionary<string, Cookie> _cookies = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<string> _order = new();

    /// <summary>
    /// Gets the number of cookies in the collection
    /// </summary>
    public int Count => _cookies.Count;

    /// <summary>
    /// Gets a value indicating whether the collection is read-only
    /// </summary>
    /// <returns>Always returns false</returns>
    public bool IsReadOnly => false;

    /// <summary>
    /// Adds a cookie to the collection
    /// </summary>
    /// <param name="cookie"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Add(Cookie cookie)
    {
        if (cookie == null)
            throw new ArgumentNullException(nameof(cookie));

        if (_cookies.ContainsKey(cookie.Name))
        {
            _cookies[cookie.Name] = cookie;
        }
        else
        {
            _cookies.Add(cookie.Name, cookie);
            _order.Add(cookie.Name);
        }
    }

    /// <summary>
    /// Adds a range of cookies to the collection
    /// </summary>
    /// <param name="cookies"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void AddRange(IEnumerable<Cookie> cookies)
    {
        if (cookies == null)
            throw new ArgumentNullException(nameof(cookies));
        foreach (var cookie in cookies)
            Add(cookie);
    }

    /// <summary>
    /// Removes all cookies from the collection
    /// </summary>
    public void Clear()
    {
        _cookies.Clear();
        _order.Clear();
    }

    /// <summary>
    /// Determines whether the collection contains a specific cookie
    /// </summary>
    /// <param name="cookie"></param>
    /// <returns></returns>
    public bool Contains(Cookie cookie) => _cookies.ContainsKey(cookie.Name);

    /// <summary>
    /// Copies the elements of the collection to an array, starting at a particular array index
    /// </summary>
    /// <param name="array"></param>
    /// <param name="arrayIndex"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public void CopyTo(Cookie[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        if (arrayIndex < 0 || arrayIndex > array.Length)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));

        if (array.Length - arrayIndex < _order.Count)
            throw new ArgumentException("The destination array has insufficient space.");

        for (var i = 0; i < _order.Count; i++)
        {
            array[arrayIndex + i] = _cookies[_order[i]];
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection
    /// </summary>
    /// <returns></returns>
    public IEnumerator<Cookie> GetEnumerator()
    {
        foreach (var name in _order)
        {
            if (_cookies.TryGetValue(name, out var cookie))
            {
                yield return cookie;
            }
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Returns a string representation of the cookies in the collection
    /// suitable for HTTP headers.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> GetHeaderString()
    {
        foreach (var name in _order)
        {
            if (_cookies.TryGetValue(name, out var cookie))
            {
                yield return cookie.ToString();
            }
        }
    }

    /// <summary>
    /// Removes the first occurrence of a specific cookie from the collection
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Remove(Cookie item)
    {
        return Remove(item.Name);
    }

    /// <summary>
    /// Removes the cookie with the specified name from the collection
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool Remove(string name)
    {
        if (_cookies.Remove(name))
        {
            _order.Remove(name);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Get or sets a cookie by name
    /// </summary>
    /// <param name="name"></param>
    /// <remarks>
    /// Cookie name must match the indexer key
    /// </remarks>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public Cookie? this[string name]
    {
        get => _cookies.TryGetValue(name, out var cookie) ? cookie : null;
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (!string.Equals(value.Name, name, StringComparison.Ordinal))
                throw new ArgumentException("Cookie name must match the indexer key.", nameof(value));

            if (_cookies.ContainsKey(name))
            {
                _cookies[name] = value;
            }
            else
            {
                _cookies.Add(name, value);
                _order.Add(name);
            }
        }
    }

    /// <summary>
    /// Gets the cookie at the specified index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Cookie? this[int index]
    {
        get
        {
            if (index < 0 || index >= _order.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var name = _order[index];
            return _cookies.TryGetValue(name, out var cookie)
                ? cookie
                : null;
        }
    }
}
