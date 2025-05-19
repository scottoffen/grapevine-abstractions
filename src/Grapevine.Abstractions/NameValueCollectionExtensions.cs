using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Grapevine.Abstractions;

public static class NameValueCollectionExtensions
{
    private static readonly ConcurrentDictionary<Type, TypeConverter> _cache = new();

    /// <summary>
    /// Retrieves a value from the <see cref="NameValueCollection"/> and converts it to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to convert the value to.</typeparam>
    /// <param name="collection">The source name-value collection.</param>
    /// <param name="key">The key whose value should be retrieved.</param>
    /// <returns>
    /// The value converted to type <typeparamref name="T"/> if found and convertible; otherwise <c>default</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="collection"/> or <paramref name="key"/> is null or empty.</exception>
    public static T? GetValue<T>(this NameValueCollection collection, string key)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        if (collection.TryGetValue(key, out var value))
        {
            try
            {
                var converter = GetTypeConverter(typeof(T));
                return (T)converter.ConvertFrom(value);
            }
            catch (Exception)
            {
                // Silently ignore conversion errors
                // and return default value for the type.
            }
        }

        return default;
    }

    /// <summary>
    /// Retrieves a value from the <see cref="NameValueCollection"/>, converts it to the specified type, or returns a default value if conversion fails.
    /// </summary>
    /// <typeparam name="T">The type to convert the value to.</typeparam>
    /// <param name="collection">The source name-value collection.</param>
    /// <param name="key">The key whose value should be retrieved.</param>
    /// <param name="defaultValue">The value to return if the key is not found or conversion fails.</param>
    /// <returns>
    /// The value converted to type <typeparamref name="T"/> if found and convertible; otherwise <paramref name="defaultValue"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="collection"/> or <paramref name="key"/> is null or empty.</exception>
    public static T? GetValue<T>(this NameValueCollection collection, string key, T defaultValue)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        if (collection.TryGetValue(key, out var value))
        {
            try
            {
                var converter = GetTypeConverter(typeof(T));
                return (T)converter.ConvertFrom(value);
            }
            catch (Exception)
            {
                // Silently ignore conversion errors
                // and return default value for the type.
            }
        }

        return defaultValue;
    }

    /// <summary>
    /// Attempts to retrieve a value from the <see cref="NameValueCollection"/> by key.
    /// </summary>
    /// <param name="collection">The source name-value collection.</param>
    /// <param name="key">The key to look up in the collection.</param>
    /// <param name="value">The value associated with the specified key, or <c>null</c> if not found.</param>
    /// <returns><c>true</c> if the key exists and the value is not <c>null</c>; otherwise <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="collection"/> or <paramref name="key"/> is null or empty.</exception>
    public static bool TryGetValue(this NameValueCollection collection, string key, out string? value)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        value = collection[key];
        return value != null;
    }

    private static TypeConverter GetTypeConverter(Type type)
    {
        return _cache.GetOrAdd(type, t =>
        {
            var converter = TypeDescriptor.GetConverter(t);
            if (converter.CanConvertFrom(typeof(string)))
            {
                return converter;
            }

            throw new InvalidOperationException($"No type converter found for type {t.FullName}");
        });
    }
}
