namespace Grapevine.Abstractions.Tests;

public class UnitTest1
{
    [Theory]
    [InlineData("hello world", "world", true)]
    [InlineData("hello world", "WORLD", true)]
    [InlineData("hello world", "hello", true)]
    [InlineData("hello world", "HELLO", true)]
    [InlineData("hello world", "test", false)]
    [InlineData("hello world", "TEST", false)]
    [InlineData("hello world", " ", true)]
    [InlineData("hello world", "", false)]
    [InlineData("", "hello", false)]
    [InlineData(null, "hello", false)]
    [InlineData(null, null, false)]
    [InlineData("", "", false)]
    [InlineData("hello world", null, false)]
    public void ContainsIgnoreCase_AccuratelyDetectsMatches(string? source, string? value, bool expected)
    {
        source.ContainsIgnoreCase(value).ShouldBe(expected);
    }

    [Theory]
    [InlineData("hello world", new[] { "world" }, true)]
    [InlineData("hello world", new[] { "WORLD" }, true)]
    [InlineData("hello world", new[] { "foo", "bar", "world" }, true)]
    [InlineData("hello world", new[] { "foo", "bar" }, false)]
    [InlineData("", new[] { "foo" }, false)]
    [InlineData(null, new[] { "foo" }, false)]
    [InlineData("hello", new string[0], false)]
    [InlineData("hello", null, false)]
    public void ContainsAny_WithVariousInputs_ReturnsExpected(string? source, string[]? values, bool expected)
    {
        source.ContainsAny(values!).ShouldBe(expected);
    }

    [Theory]
    [InlineData("TestString", new[] { "teststring" }, StringComparison.OrdinalIgnoreCase, true)]
    [InlineData("TestString", new[] { "teststring" }, StringComparison.Ordinal, false)]
    [InlineData("TestString", new[] { "Test" }, StringComparison.CurrentCulture, true)]
    [InlineData("TestString", new[] { "notfound" }, StringComparison.CurrentCulture, false)]
    public void ContainsAny_RespectsComparisonMode(string source, string[] values, StringComparison comparison, bool expected)
    {
        source.ContainsAny(values, comparison).ShouldBe(expected);
    }

    [Theory]
    [InlineData("hello world", StringComparison.CurrentCultureIgnoreCase, true, "world")]
    [InlineData("hello world", StringComparison.CurrentCultureIgnoreCase, true, "WORLD")]
    [InlineData("hello world", StringComparison.CurrentCultureIgnoreCase, true, "foo", "bar", "world")]
    [InlineData("hello world", StringComparison.CurrentCultureIgnoreCase, false, "foo", "bar")]
    [InlineData("", StringComparison.CurrentCultureIgnoreCase, false, "foo")]
    [InlineData(null, StringComparison.CurrentCultureIgnoreCase, false, "foo")]
    [InlineData("hello", StringComparison.CurrentCultureIgnoreCase, false)] // No values
    public void ContainsAny_Params_Overload_ReturnsExpected(string? source, StringComparison comparison, bool expected, params string[] values)
    {
        source.ContainsAny(comparison, values).ShouldBe(expected);
    }

    [Theory]
    [InlineData("TestString", StringComparison.OrdinalIgnoreCase, true, "teststring")]
    [InlineData("TestString", StringComparison.Ordinal, false, "teststring")]
    [InlineData("TestString", StringComparison.CurrentCulture, true, "Test")]
    [InlineData("TestString", StringComparison.CurrentCulture, false, "notfound")]
    public void ContainsAny_Params_RespectsComparisonMode(string source, StringComparison comparison, bool expected, params string[] values)
    {
        source.ContainsAny(comparison, values).ShouldBe(expected);
    }

    [Theory]
    [InlineData("HelloWorld", new[] { "world" }, true)]
    [InlineData("HelloWorld", new[] { "WORLD", "notmatch" }, true)]
    [InlineData("HelloWorld", new[] { "not", "match" }, false)]
    [InlineData("HelloWorld", new[] { "" }, true)] // Empty suffix always matches
    [InlineData("", new[] { "" }, true)] // Empty string ends with empty suffix
    [InlineData("", new[] { "d" }, false)]
    public void EndsWithAny_ReturnsExpected(string input, string[] suffixes, bool expected)
    {
        input.EndsWithAny(suffixes).ShouldBe(expected);
    }

    [Theory]
    [InlineData("CaseSensitive", new[] { "SENSITIVE" }, true)]
    [InlineData("CaseSensitive", new[] { "Sensitive" }, true)]
    [InlineData("CaseSensitive", new[] { "case", "sensitive" }, true)]
    [InlineData("CaseSensitive", new[] { "Case" }, false)]
    public void EndsWithAny_IsCaseInsensitive(string input, string[] suffixes, bool expected)
    {
        input.EndsWithAny(suffixes).ShouldBe(expected);
    }

    [Fact]
    public void EndsWithAny_WithNoSuffixes_ReturnsFalse()
    {
        "HelloWorld".EndsWithAny().ShouldBeFalse();
    }

    [Theory]
    [InlineData("HelloWorld", new[] { "hello" }, true)]
    [InlineData("HelloWorld", new[] { "HELLO", "notmatch" }, true)]
    [InlineData("HelloWorld", new[] { "not", "match" }, false)]
    [InlineData("HelloWorld", new[] { "" }, true)] // Empty prefix always matches
    [InlineData("", new[] { "" }, true)] // Empty string starts with empty prefix
    [InlineData("", new[] { "H" }, false)]
    public void StartsWithAny_ReturnsExpected(string input, string[] prefixes, bool expected)
    {
        input.StartsWithAny(prefixes).ShouldBe(expected);
    }

    [Theory]
    [InlineData("CaseSensitive", new[] { "casesensitive" }, true)]
    [InlineData("CaseSensitive", new[] { "Case" }, true)]
    [InlineData("CaseSensitive", new[] { "case", "sensitive" }, true)]
    [InlineData("CaseSensitive", new[] { "Sensitive" }, false)]
    public void StartsWithAny_IsCaseInsensitive(string input, string[] prefixes, bool expected)
    {
        input.StartsWithAny(prefixes).ShouldBe(expected);
    }

    [Fact]
    public void StartsWithAny_WithNoPrefixes_ReturnsFalse()
    {
        "HelloWorld".StartsWithAny().ShouldBeFalse();
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("   ", "")]
    [InlineData("/", "")]
    [InlineData("///", "")]
    [InlineData(" / ", "")]
    [InlineData("///api//v1///", "api/v1")]
    [InlineData("/api/v1/", "api/v1")]
    [InlineData("   /api/v1/   ", "api/v1")]
    [InlineData("/api///v1///users/", "api/v1/users")]
    [InlineData("path/with//multiple///slashes", "path/with/multiple/slashes")]
    [InlineData("///one/two//three////", "one/two/three")]
    public void TrimPath_ShouldReturnExpectedResult(string? input, string expected)
    {
        input.TrimPath().ShouldBe(expected);
    }
}
