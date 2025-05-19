using Grapevine.Abstractions.RouteConstraints;

namespace Grapevine.Abstractions.Tests.RouteConstraints;

public class RegexResolverTests
{
    [Theory]
    [InlineData("abc", @"(?<x>abc)")]
    [InlineData(@"\d+", @"(?<x>\d+)")]
    [InlineData(@"[a-zA-Z0-9_-]+", @"(?<x>[a-zA-Z0-9_-]+)")]
    [InlineData(@"[a-z]{3}\d{2}", @"(?<x>[a-z]{3}\d{2})")]
    public void Resolve_ShouldReturnExpectedPattern_WhenArgsAreValid(string args, string expected)
    {
        var result = RegexResolver.Resolve("x", args);
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Resolve_ShouldThrowArgumentException_WhenArgsIsEmpty(string? args)
    {
        var ex = Should.Throw<ArgumentException>(() =>
        {
            RegexResolver.Resolve("x", args);
        });

        ex.ParamName.ShouldBe("args");
        ex.Message.ShouldContain("non-empty pattern");
    }

    [Theory]
    [InlineData("^\\d+$")]
    [InlineData("abc$")]
    [InlineData("^hello")]
    public void Resolve_ShouldThrowArgumentException_WhenPatternIsAnchored(string args)
    {
        var ex = Should.Throw<ArgumentException>(() =>
        {
            RegexResolver.Resolve("x", args);
        });

        ex.ParamName.ShouldBe("args");
        ex.Message.ShouldContain("must not start with ^ or end with $");
    }

    [Theory]
    [InlineData("abc(def)")]
    [InlineData("pre(?<group>abc)post")]
    [InlineData("([a-z]{2})(\\d{2})")]
    public void Resolve_ShouldThrowArgumentException_WhenPatternContainsCaptureGroups(string args)
    {
        var ex = Should.Throw<ArgumentException>(() =>
        {
            RegexResolver.Resolve("x", args);
        });

        ex.ParamName.ShouldBe("args");
        ex.Message.ShouldContain("must not contain any capture groups");
    }

    [Theory]
    [InlineData("[a-")]   // invalid range
    [InlineData("*")]     // quantifier with no expression
    [InlineData("abc(")]  // unbalanced parens
    public void Resolve_ShouldThrowArgumentException_WhenPatternIsInvalidRegex(string args)
    {
        var ex = Should.Throw<ArgumentException>(() =>
        {
            RegexResolver.Resolve("x", args);
        });

        ex.ParamName.ShouldBe("args");
        ex.Message.ShouldContain("Invalid regular expression pattern");
    }
}
