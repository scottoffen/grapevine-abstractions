using System.Collections.Concurrent;

namespace Grapevine.Abstractions;

public class Locals : ConcurrentDictionary<object, object?> { }

public static class LocalsExtensions
{
    /// <summary>
    /// Gets the value associated with the specified key, or null if the key does not exist.
    /// </summary>
    /// <param name="locals"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static object? Get(this Locals locals, object key)
    {
        return locals.TryGetValue(key, out var value) ? value : null;
    }

    /// <summary>
    /// Gets the value associated with the specified key, or null if the key does not exist.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="locals"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T? GetAs<T>(this Locals locals, object key)
    {
        if (!locals.TryGetValue(key, out var value))
            return default;

        return value is T t ? t : default;
    }

    /// <summary>
    /// Adds a key/value pair to the collection if the key does not already exist. Returns the new value, or the existing value if the key exists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="locals"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T? GetOrAddAs<T>(this Locals locals, object key, T value)
    {
        var returned = locals.GetOrAdd(key, value);

        if (returned is null) return default;
        if (returned is T t) return t;

        throw new InvalidCastException($"Value for key '{key}' is of type '{returned.GetType()}', expected type '{typeof(T)}'.");
    }

    /// <summary>
    /// Adds a key/value pair to the collection if the key does not already exist. Returns the new value, or the existing value if the key exists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="locals"></param>
    /// <param name="key"></param>
    /// <param name="valueFactory"></param>
    /// <returns></returns>
    public static T? GetOrAddAs<T>(this Locals locals, object key, Func<object, T> valueFactory)
    {
        var returned = locals.GetOrAdd(key, k => valueFactory(k));

        if (returned is null) return default;
        if (returned is T t) return t;

        throw new InvalidCastException($"Value for key '{key}' is of type '{returned.GetType()}', expected type '{typeof(T)}'.");
    }


    /// <summary>
    /// Sets the value for the specified key in the Locals collection.
    /// </summary>
    /// <param name="locals"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void Set(this Locals locals, object key, object? value)
    {
        if (value is null)
        {
            locals.TryRemove(key, out _);
        }
        else
        {
            locals[key] = value;
        }
    }
}
