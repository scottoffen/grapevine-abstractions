using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Grapevine.Abstractions.RouteConstraints;

namespace Grapevine.Abstractions;

[DebuggerDisplay("{Template}")]
public class RouteTemplate : IComparable<RouteTemplate>
{
    private readonly string[] _groupNames;

    /// <summary>
    /// Gets the names of the named parameters in the route template.
    /// </summary>
    public IReadOnlyList<string> GroupNames => _groupNames;

    /// <summary>
    /// Returns <c>true</c> if the specified path matches the route template pattern.
    /// </summary>
    /// <param name="path">The request path to test against the route pattern.</param>
    /// <returns><c>true</c> if the path matches the pattern; otherwise <c>false</c>.</returns>
    public bool Matches(string path) => Pattern.IsMatch(path);

    /// <summary>
    /// Gets the number of named parameters in the route template.
    /// </summary>
    public int Parameters { get; }

    /// <summary>
    /// Gets the compiled regular expression pattern used to match the route path.
    /// </summary>
    public Regex Pattern { get; }

    /// <summary>
    /// Gets the number of segments in the route template.
    /// </summary>
    public int Segments { get; }

    /// <summary>
    /// Gets the original route template string used to generate the pattern.
    /// </summary>
    public string Template { get; }

    public RouteTemplate(string template)
    {
        var pattern = ParseTemplate(template, out var segmentCount);

        Template = template;
        Pattern = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        _groupNames = Pattern.GetGroupNames().Where(name => !int.TryParse(name, out _)).ToArray();

        Parameters = _groupNames.Length;
        Segments = segmentCount;
    }

    /// <summary>
    /// Compares this instance with another <see cref="RouteTemplate"/> object.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(RouteTemplate? other)
    {
        if (other == null) return -1;

        var segmentCompare = other.Segments.CompareTo(Segments);
        if (segmentCompare != 0) return segmentCompare;

        return Parameters.CompareTo(other.Parameters);
    }

    /// <summary>
    /// Extracts route parameters from the specified path if it matches the route pattern.
    /// </summary>
    /// <param name="path">The request path to extract route parameters from.</param>
    /// <returns>
    /// A <see cref="RouteParams"/> collection containing key-value pairs for each named capture group in the pattern.
    /// If the path does not match, the collection is empty.
    /// </returns>
    public RouteParams GetRouteParams(string path)
    {
        var routeParams = new RouteParams();

        var match = Pattern.Match(path);
        if (!match.Success) return routeParams;

        foreach (var groupName in _groupNames)
        {
            routeParams.Add(groupName, match.Groups[groupName].Value);
        }

        return routeParams;
    }

    /// <summary>
    /// Converts a route template string into a regular expression pattern.
    /// </summary>
    /// <param name="template">The route template to parse.</param>
    /// <param name="segmentCount"></param>
    /// <returns>A string containing the regular expression pattern that matches the route structure.</returns>
    private string ParseTemplate(string template, out int segmentCount)
    {
        segmentCount = 0;

        if (string.IsNullOrWhiteSpace(template) || template.Trim() == "*")
            return "^.*$";

        template = template.Trim('/');

        var segments = new List<string>();
        var start = 0;
        var depth = 0;

        for (var i = 0; i < template.Length; i++)
        {
            var currentChar = template[i];

            if (currentChar == '{')
            {
                if (depth == 0) start = i;
                depth++;
            }
            else if (currentChar == '}')
            {
                depth--;
                if (depth == 0)
                {
                    segments.Add(template.Substring(start, i - start + 1));
                    start = i + 1;
                }
            }
            else if (currentChar == '/' && depth == 0)
            {
                if (i > start)
                    segments.Add(template.Substring(start, i - start));
                start = i + 1;
            }
        }

        if (start < template.Length)
            segments.Add(template.Substring(start));

        segmentCount = segments.Count;

        var patternBuilder = new StringBuilder("^");

        for (var i = 0; i < segments.Count; i++)
        {
            if (i > 0)
                patternBuilder.Append("/");

            var segment = segments[i];
            if (ResolverRegistry.TryResolveSegment(segment, out var regexSegment) && regexSegment != null)
            {
                patternBuilder.Append(regexSegment);
            }
            else
            {
                patternBuilder.Append(Regex.Escape(segment));
            }
        }

        patternBuilder.Append("$");
        return patternBuilder.ToString();
    }
}
