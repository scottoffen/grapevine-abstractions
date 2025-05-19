using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace Grapevine.Abstractions.RouteConstraints;

/// <summary>
/// Represents a method that resolves a route constraint to a regular expression pattern
/// </summary>
/// <param name="name"></param>
/// <param name="value"></param>
/// <returns>A regular expression patterns that matches the route constraint</returns>
public delegate string RouteConstraintResolver(string name, string? value);

public static class ResolverRegistry
{
    private static readonly ConcurrentDictionary<string, RouteConstraintResolver> _resolvers = new(StringComparer.OrdinalIgnoreCase);

    static ResolverRegistry()
    {
        RegisterResolver("alpha", AlphaResolver.Resolve);
        RegisterResolver("bool", BoolResolver.Resolve);
        RegisterResolver("date", DateResolver.Resolve);
        RegisterResolver("datetime", DateTimeResolver.Resolve);
        RegisterResolver("decimal", DecimalResolver.Resolve);
        RegisterResolver("double", DoubleResolver.Resolve);
        RegisterResolver("float", DoubleResolver.Resolve);
        RegisterResolver("guid", GuidResolver.Resolve);
        RegisterResolver("int", IntResolver.Resolve);
        RegisterResolver("len", DefaultResolver.Resolve);
        RegisterResolver("long", IntResolver.Resolve);
        RegisterResolver("regex", RegexResolver.Resolve);
        RegisterResolver("text", DefaultResolver.Resolve);
    }

    /// <summary>
    /// <para>Resolves a route constraint to a regular expression pattern.</para>
    /// Accepted formats:
    /// <list type="bullet">
    /// <item>{name}</item>
    /// <item>{name:constraint}</item>
    /// <item>{name:constraint(args)}</item>
    /// </list>
    /// Where:
    /// <list type="bullet">
    /// <item><term>name</term> The name of the route parameter</item>
    /// <item><term>constraint</term> Optional name of the route constraint</item>
    /// <item><term>args</term> Optional arguments for the route constraint</item>
    /// </list>
    /// If no constraint is specified, the default resolver is used.
    /// </summary>
    /// <param name="segment"></param>
    /// <param name="pattern"></param>
    /// <returns>A regular expression segment for the specified route constraint.</returns>
    public static bool TryResolveSegment(string segment, out string? pattern)
    {
        pattern = null;

        if (TryParseSegment(segment, out var name, out var key, out var args))
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                if (_resolvers.TryGetValue("text", out var defaultResolver))
                {
                    pattern = defaultResolver(name, args);
                    return true;
                }
            }
            else if (_resolvers.TryGetValue(key!, out var resolver))
            {
                pattern = resolver(name, args);
                return true;
            }

            var knownKeys = string.Join(", ", _resolvers.Keys.OrderBy(k => k));
            throw new ArgumentException($"No resolver registered for constraint '{key}'. Known constraints: {knownKeys}.");
        }

        return false;
    }


    /// <summary>
    /// Registers a new route constraint resolver. Throws an exception if the key already exists.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="resolver"></param>
    public static void RegisterResolver(string key, RouteConstraintResolver resolver)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));

        if (resolver == null)
            throw new ArgumentNullException(nameof(resolver));

        if (_resolvers.ContainsKey(key))
            throw new InvalidOperationException($"A resolver is already registered for the key '{key}'.");

        if (!_resolvers.TryAdd(key, resolver))
            throw new InvalidOperationException($"Failed to register resolver for the key '{key}'.");
    }


    /// <summary>
    /// Overrides an existing route constraint resolver. Silently adds the resolver if the key does not exist.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="resolver"></param>
    public static void OverrideResolver(string key, RouteConstraintResolver resolver)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));

        if (resolver == null)
            throw new ArgumentNullException(nameof(resolver));

        _resolvers[key] = resolver;
    }

    /// <summary>
    /// Takes a value in the format of {name}, {name:constraint}, or {name:constraint(args)} and parses it into its components.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="name"></param>
    /// <param name="key"></param>
    /// <param name="args"></param>
    /// <returns>Returns true if the input matches one of the formats.</returns>
    private static bool TryParseSegment(string input, out string name, out string? key, out string? args)
    {
        name = string.Empty;
        key = null;
        args = null;

        if (string.IsNullOrEmpty(input) || input.Length < 3 || input[0] != '{' || input[input.Length - 1] != '}')
            return false;

        var inner = input.Substring(1, input.Length - 2);

        var colonIndex = inner.IndexOf(':');
        if (colonIndex < 0)
        {
            name = inner;
            key = "text"; // fallback
            return !string.IsNullOrWhiteSpace(name);
        }

        name = inner.Substring(0, colonIndex);
        if (string.IsNullOrWhiteSpace(name))
            return false;

        var parenIndex = inner.IndexOf('(', colonIndex + 1);
        if (parenIndex >= 0)
        {
            var closingParenIndex = inner.LastIndexOf(')');
            if (closingParenIndex < parenIndex)
                return false; // unbalanced parens

            key = inner.Substring(colonIndex + 1, parenIndex - colonIndex - 1);
            args = inner.Substring(parenIndex + 1, closingParenIndex - parenIndex - 1);
        }
        else
        {
            key = inner.Substring(colonIndex + 1);
        }

        return !string.IsNullOrWhiteSpace(key);
    }
}
