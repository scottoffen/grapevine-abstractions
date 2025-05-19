using System.Diagnostics;

namespace Grapevine.Abstractions;

[DebuggerDisplay("{ToString()}")]
public sealed class ProblemDetailsExtensions : Dictionary<string, object>, IDictionary<string, object>
{
    private static readonly HashSet<string> _restricted = new(StringComparer.OrdinalIgnoreCase)
    {
        "detail",
        "instance",
        "status",
        "title",
        "type"
    };

    public ProblemDetailsExtensions() : base(StringComparer.OrdinalIgnoreCase) { }

    public new void Add(string key, object value)
    {
        ValidateKey(key);
        base[key] = value;
    }

    public new object this[string key]
    {
        get => base[key];
        set
        {
            ValidateKey(key);
            base[key] = value;
        }
    }

    public void AddRange(IEnumerable<KeyValuePair<string, object>> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));

        foreach (var item in items)
        {
            Add(item.Key, item.Value);
        }
    }

    public override string ToString()
    {
        return string.Join(", ", this.Select(kvp => $"{kvp.Key}={kvp.Value}"));
    }

    public void TryAdd(string key, object value)
    {
        if (ContainsKey(key)) return;
        Add(key, value);
    }

    private static void ValidateKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("The key cannot be null, empty, or whitespace.", nameof(key));

        key = key.Trim();

        if (_restricted.Contains(key))
            throw new ArgumentException($"The key '{key}' conflicts with a reserved field name and cannot be used as an extension.");
    }
}
