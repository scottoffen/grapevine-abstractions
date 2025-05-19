using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Grapevine.Abstractions;

/// <summary>
/// Represents a method that defines a condition to evaluate a prefix before adding it to a collection.
/// </summary>
/// <param name="prefix">The prefix to be evaluated by the condition.</param>
/// <returns>True if the prefix meets the condition; otherwise false.</returns>
public delegate bool PrefixPredicate(string prefix);

[DebuggerDisplay("Count = {Count}")]
public class PrefixCollection : ICollection<string>, IEnumerable<string>, IEnumerable
{
    private static readonly Regex _prefixRegex = new(
        @"^https?://[^/]+/+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    private readonly HashSet<string> _prefixes = new();

    private PrefixPredicate _prefixEvaluator = DefaultPrefixPredicate;

    /// <summary>
    /// Gets the number of elements contained in the collection.
    /// </summary>
    public int Count => _prefixes.Count;

    /// <summary>
    /// Gets a value indicating whether the collection is read-only.
    /// </summary>
    /// <returns>Always returns false</returns>
    public bool IsReadOnly => false;

    /// <summary>
    /// Gets or sets the method used to evaluate prefixes before adding them to the collection
    /// </summary>
    public PrefixPredicate PrefixEvaluator
    {
        get => _prefixEvaluator;
        set
        {
            _prefixEvaluator = value
                ?? throw new ArgumentNullException(nameof(value), "Prefix evaluator cannot be null.");
        }
    }

    /// <summary>
    /// Adds a prefix to the collection
    /// </summary>
    /// <param name="prefix"></param>
    /// <exception cref="ArgumentException"></exception>
    public void Add(string prefix)
    {
        if (!PrefixEvaluator(prefix))
            throw new ArgumentException($"The prefix '{prefix}' is invalid according to the configured evaluation rules.", nameof(prefix));

        _prefixes.Add(prefix);
    }

    /// <summary>
    /// Removes all prefixes from the collection
    /// </summary>
    public void Clear() => _prefixes.Clear();

    /// <summary>
    /// Determines whether the collection contains a specific prefix
    /// </summary>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public bool Contains(string prefix) => _prefixes.Contains(prefix);

    /// <summary>
    /// Copies the elements of the collection to an array, starting at a particular array index
    /// </summary>
    /// <param name="array"></param>
    /// <param name="arrayIndex"></param>
    public void CopyTo(string[] array, int arrayIndex) => _prefixes.CopyTo(array, arrayIndex);

    /// <summary>
    /// Returns an enumerator that iterates through the collection
    /// </summary>
    /// <returns></returns>
    public IEnumerator<string> GetEnumerator() => _prefixes.GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through the collection
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Removes the first occurrence of a specific prefix from the collection
    /// </summary>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public bool Remove(string prefix) => _prefixes.Remove(prefix);

    /// <summary>
    /// Adds a prefix to the collection if it meets the evaluation criteria
    /// </summary>
    /// <param name="prefix"></param>
    /// <returns>True if the prefix was added, false if it did not meet the evaluation criteria.</returns>
    public bool TryAdd(string prefix)
    {
        if (!PrefixEvaluator(prefix))
            return false;

        _prefixes.Add(prefix);
        return true;
    }

    /// <summary>
    /// The default method used to evaluate prefixes before adding them to the collection
    /// </summary>
    /// <param name="prefix"></param>
    /// <returns>Returns true if the prefix is not null or empty, starts with http:// or https:// and ends with a forward slash ("/"); otherwise false.</returns>
    public static bool DefaultPrefixPredicate(string prefix)
    {
        return !string.IsNullOrWhiteSpace(prefix) && _prefixRegex.IsMatch(prefix);
    }
}
