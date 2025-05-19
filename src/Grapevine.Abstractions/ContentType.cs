using System.Diagnostics;
using System.Text;

namespace Grapevine.Abstractions;

/// <summary>
/// Represents a content type for HTTP requests and responses.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public partial class ContentType : IEquatable<ContentType>
{
    private static readonly IEnumerable<string> _keywords = new[]
    {
        "form",
        "json",
        "xml",
        "javascript",
        "html",
        "css",
        "txt"
    };

    /// <summary>
    /// The primary type of the content type
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// The subtype of the content type
    /// </summary>
    public string SubType { get; }

    /// <summary>
    /// The character set of the content type
    /// </summary>
    public string? Charset { get; }

    /// <summary>
    /// The boundary of the content type
    /// </summary>
    public string? Boundary { get; }

    /// <summary>
    /// Additional parameters of the content type
    /// </summary>
    public Dictionary<string, string> Parameters { get; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Indicates whether the content type is non-text-based
    /// </summary>
    /// <remarks>
    /// This value is auto-set by the constructor if the type starts with text or if the combined type contains form, json, xml, javascript, html, css or txt. It should be manually overridden only as needed.
    /// </remarks>
    public bool IsBinary { get; set; } = true;

    public ContentType(string type, string subtype, string? charset = null, string? boundary = null)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        SubType = subtype ?? string.Empty;
        Charset = charset;
        Boundary = boundary;

        var contentType = string.Join("/", Type, SubType);
        if (!string.IsNullOrEmpty(Charset) || contentType.StartsWith("text", StringComparison.InvariantCultureIgnoreCase) || contentType.ContainsAny(_keywords))
        {
            IsBinary = false;
        }
    }

    /// <summary>
    /// Compares this instance with another instance of ContentType for equality.
    /// </summary>
    /// <remarks>
    /// This method compares the Type and SubType properties of both instances, ignoring case. It does not compare the Charset, Boundary, or Parameters properties.
    /// </remarks>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ContentType? other)
    {
        if (other is null) return false;
        return string.Equals(Type, other.Type, StringComparison.OrdinalIgnoreCase)
            && string.Equals(SubType, other.SubType, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Type)
            ^ StringComparer.OrdinalIgnoreCase.GetHashCode(SubType);
    }

    /// <summary>
    /// Compares this instance with another object for equality.
    /// </summary>
    /// <remarks>
    /// This method compares the Type and SubType properties of both instances, ignoring case. It does not compare the Charset, Boundary, or Parameters properties.
    /// </remarks>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        return Equals(obj as ContentType);
    }

    public override string ToString()
    {
        var sb = new StringBuilder(string.Join("/", Type, SubType));

        if (!string.IsNullOrWhiteSpace(Charset))
        {
            sb.Append("; charset=");
            sb.Append(Charset);
        }

        if (!string.IsNullOrWhiteSpace(Boundary))
        {
            sb.Append("; boundary=");
            sb.Append(Boundary);
        }

        foreach (var param in Parameters)
        {
            sb.Append("; ");
            sb.Append(param.Key);
            sb.Append('=');
            sb.Append(FormatParameterValue(param.Value));
        }

        return sb.ToString();
    }

    private static string FormatParameterValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return "\"\"";

        var needsQuoting = value.Any(c => char.IsWhiteSpace(c) || c == ';' || c == '=' || c == '"');

        // Escape quotes if present (e.g. boundary="abc\"123")
        var escaped = value.Replace("\"", "\\\"");

        return needsQuoting ? $"\"{escaped}\"" : escaped;
    }
}

public partial class ContentType
{
    /// <summary>
    /// Compares two instances of ContentType for equality.
    /// </summary>
    /// <remarks>
    /// This operator compares the Type and SubType properties of both instances, ignoring case. It does not compare the Charset, Boundary, or Parameters properties.
    /// </remarks>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(ContentType? left, ContentType? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    /// <summary>
    /// Compares two instances of ContentType for inequality.
    /// </summary>
    /// <remarks>
    /// This operator compares the Type and SubType properties of both instances, ignoring case. It does not compare the Charset, Boundary, or Parameters properties.
    /// </remarks>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(ContentType? left, ContentType? right)
    {
        return !(left == right);
    }

    public static implicit operator string(ContentType contentType)
    {
        return contentType.ToString();
    }

    public static implicit operator ContentType(string contentType)
    {
        return Parse(contentType);
    }
}

public partial class ContentType
{
    private const string CharsetTag = "charset";
    private const string BoundaryTag = "boundary";

    /// <summary>
    /// Parse a string representation of a content type into a ContentType object
    /// </summary>
    /// <param name="contentType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static ContentType Parse(string contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentException("Missing or invalid content type.", nameof(contentType));

        var parts = contentType.Trim().Split(';');
        var typeParts = parts[0].Trim().Split('/');

        var type = typeParts[0];
        var subtype = typeParts.Length > 1
            ? typeParts[1]
            : string.Empty;

        var charset = string.Empty;
        var boundary = string.Empty;
        var @params = new Dictionary<string, string>();

        foreach (var part in parts.Skip(1))
        {
            var paramParts = part.Split(new[] { '=' }, 2, StringSplitOptions.None);
            var key = paramParts[0].Trim().ToLowerInvariant();
            var value = paramParts.Length > 1
                ? paramParts[1].Trim().Trim('"')
                : string.Empty;

            switch (key)
            {
                case CharsetTag:
                    charset = value;
                    break;
                case BoundaryTag:
                    boundary = value;
                    break;
                default:
                    @params.Add(key, value);
                    break;
            }
        }

        var result = new ContentType(type, subtype, charset, boundary);
        foreach (var param in @params)
        {
            result.Parameters.Add(param.Key, param.Value);
        }

        return result;
    }

    /// <summary>
    /// A content type representing any binary data
    /// </summary>
    public static readonly ContentType Binary = Parse("application/octet-stream");

    /// <summary>
    /// A content type representing Cascading Style Sheets
    /// </summary>
    public static readonly ContentType Css = Parse("text/css; charset=UTF-8");

    /// <summary>
    /// A content type representing comma-separated values
    /// </summary>
    /// <remarks>For comparing with incoming requests only</remarks>
    // Remember: Servers don't return FormUrlEncoded data
    public static readonly ContentType FormUrlEncoded = Parse("application/x-www-form-urlencoded");

    /// <summary>
    /// A content type representing Graphics Interchange Format images
    /// </summary>
    public static readonly ContentType Gif = Parse("image/gif");

    /// <summary>
    /// A content type representing HyperText Markup Language
    /// </summary>
    public static readonly ContentType Html = Parse("text/html; charset=UTF-8");

    /// <summary>
    /// A content type representing Icon images
    /// </summary>
    public static readonly ContentType Icon = Parse("image/x-icon");

    /// <summary>
    /// A content type representing JavaScript
    /// </summary>
    public static readonly ContentType JavaScript = Parse("application/javascript; charset=UTF-8");

    /// <summary>
    /// A content type representing a JSON object
    /// </summary>
    public static readonly ContentType Json = Parse("application/json; charset=UTF-8");

    /// <summary>
    /// A content type representing Joint Photographic Experts Group images
    /// </summary>
    public static readonly ContentType Jpg = Parse("image/jpeg");

    /// <summary>
    /// A content type representing an MPEG encoded audio file
    /// </summary>
    public static readonly ContentType Mp3 = Parse("audio/mpeg");

    /// <summary>
    /// A content type representing an MP4 encoded video file
    /// </summary>
    public static readonly ContentType Mp4 = Parse("video/mp4");

    /// <summary>
    /// A content type representing a form that contains files
    /// </summary>
    /// <remarks>For comparing with incoming requests only</remarks>
    // Remember: Servers don't return MultipartFormData
    public static readonly ContentType MultipartFormData = Parse("multipart/form-data");

    /// <summary>
    /// A content type representing Portable Document Format files
    /// </summary>
    public static readonly ContentType Pdf = Parse("application/pdf");

    /// <summary>
    /// A content type representing Portable Network Graphics images
    /// </summary>
    public static readonly ContentType Png = Parse("image/png");

    /// <summary>
    /// A content type representing a Problem Details JSON object
    /// </summary>
    public static readonly ContentType ProblemDetailsJson = Parse("application/problem+json; charset=UTF-8");

    /// <summary>
    /// A content type representing a Problem Details XML document
    /// </summary>
    public static readonly ContentType ProblemDetailsXml = Parse("application/problem+xml; charset=UTF-8");

    /// <summary>
    /// A content type representing SVG graphics images
    /// </summary>
    public static readonly ContentType Svg = Parse("image/svg+xml; charset=UTF-8");

    /// <summary>
    /// A content type representing plain text
    /// </summary>
    public static readonly ContentType Text = Parse("text/plain; charset=UTF-8");

    /// <summary>
    /// A content type representing an XML document
    /// </summary>
    public static readonly ContentType Xml = Parse("application/xml; charset=UTF-8");

    /// <summary>
    /// A content type representing a ZIP archive
    /// </summary>
    public static readonly ContentType Zip = Parse("application/zip");
}
