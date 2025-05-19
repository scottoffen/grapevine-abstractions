using System.Globalization;
using System.Text;

namespace Grapevine.Abstractions;

internal static class StringBuilderExtensions
{
    /// <summary>
    /// Appends a JSON key-value pair to the StringBuilder.
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <exception cref="NotImplementedException"></exception>
    public static void AppendJson(this StringBuilder sb, string key, object? value)
    {
        sb.Append("\"");
        sb.Append(EscapeJson(key));
        sb.Append("\":");

        if (value == null)
        {
            sb.Append("null");
        }
        else if (value is string s)
        {
            sb.Append("\"");
            sb.Append(EscapeJson(s));
            sb.Append("\"");
        }
        else if (value is bool b)
        {
            sb.Append(b ? "true" : "false");
        }
        else if (value is int || value is float || value is double || value is long || value is decimal)
        {
            sb.Append(Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
        }
        else if (value is DateTime dt)
        {
            sb.Append("\"");
            sb.Append(dt.ToString("o", CultureInfo.InvariantCulture)); // ISO 8601
            sb.Append("\"");
        }
        else
        {
            // fallback to quoted .ToString()
            sb.Append("\"");
            sb.Append(EscapeJson(value.ToString()));
            sb.Append("\"");
        }
    }

    private static string EscapeJson(string input)
    {
        return input
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t");
    }
}
