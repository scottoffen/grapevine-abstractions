using System.Globalization;
using System.Text;

namespace Grapevine.Abstractions.Tests;

public class StringBuilderExtensionsTests
{
    public static IEnumerable<object[]> PrimitiveValues =>
        new List<object[]>
        {
        new object[] { "key", null!, "\"key\":null" },
        new object[] { "key", "value", "\"key\":\"value\"" },
        new object[] { "key", "", "\"key\":\"\"" },
        new object[] { "key", "va\"lue", "\"key\":\"va\\\"lue\"" },
        new object[] { "key", "line\nbreak", "\"key\":\"line\\nbreak\"" },
        new object[] { "key", "tab\tchar", "\"key\":\"tab\\tchar\"" },
        new object[] { "key", true, "\"key\":true" },
        new object[] { "key", false, "\"key\":false" },
        new object[] { "key", 123, "\"key\":123" },
        new object[] { "key", 45.67, "\"key\":45.67" },
        new object[] { "key", 1.2f, "\"key\":1.2" },
        new object[] { "key", 12345678901234L, "\"key\":12345678901234" },
        new object[] { "key", 12.34m, "\"key\":12.34" },
        };

    [Theory]
    [MemberData(nameof(PrimitiveValues))]
    public void AppendJson_ShouldSerializePrimitiveTypes(string key, object? value, string expected)
    {
        var sb = new StringBuilder();

        sb.AppendJson(key, value);

        sb.ToString().ShouldBe(expected);
    }

    [Fact]
    public void AppendJson_ShouldSerializeDateTimeAsIso8601()
    {
        var dt = new DateTime(2023, 5, 1, 13, 45, 30, DateTimeKind.Utc);
        var sb = new StringBuilder();

        sb.AppendJson("date", dt);

        var expected = $"\"date\":\"{dt.ToString("o", CultureInfo.InvariantCulture)}\"";
        sb.ToString().ShouldBe(expected);
    }

    private sealed class CustomObject
    {
        public override string ToString() => "Custom\\\"Text\n";
    }

    [Fact]
    public void AppendJson_ShouldSerializeFallbackAsEscapedString()
    {
        var obj = new CustomObject();
        var sb = new StringBuilder();

        sb.AppendJson("custom", obj);

        sb.ToString().ShouldBe("\"custom\":\"Custom\\\\\\\"Text\\n\"");
    }

    [Fact]
    public void AppendJson_ShouldEscapeJsonKey()
    {
        var sb = new StringBuilder();
        sb.AppendJson("weird\"key", "value");

        sb.ToString().ShouldBe("\"weird\\\"key\":\"value\"");
    }
}
