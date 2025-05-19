using Grapevine.Abstractions.RouteConstraints;

namespace Grapevine.Abstractions.Tests.RouteConstraints;

public class AlphaResolverTests
{
    [Theory]
    [InlineData(null, "(?<letters>[a-zA-Z]+)")]
    [InlineData("", "(?<letters>[a-zA-Z]+)")]
    [InlineData(" ", "(?<letters>[a-zA-Z]+)")]
    public void Resolve_ShouldReturnUnboundedPattern_WhenArgsIsNullOrWhitespace(string? args, string expected)
    {
        var pattern = AlphaResolver.Resolve("letters", args);
        pattern.ShouldBe(expected);
    }

    [Theory]
    [InlineData("3", "(?<letters>[a-zA-Z]{3})")]
    [InlineData("1,", "(?<letters>[a-zA-Z]{1,})")]
    [InlineData(",5", "(?<letters>[a-zA-Z]{1,5})")]
    [InlineData("1,5", "(?<letters>[a-zA-Z]{1,5})")]
    public void Resolve_ShouldReturnBoundedPattern_WhenArgsAreValid(string args, string expected)
    {
        var pattern = AlphaResolver.Resolve("letters", args);
        pattern.ShouldBe(expected);
    }
}
