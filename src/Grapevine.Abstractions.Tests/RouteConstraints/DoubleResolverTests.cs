using Grapevine.Abstractions.RouteConstraints;

namespace Grapevine.Abstractions.Tests.RouteConstraints;

public class DoubleResolverTests
{
    [Theory]
    [InlineData(null, @"(?<value>[-]?\d+(?:\.\d+)?(?:[eE][-+]?\d+)?)")]
    [InlineData("", @"(?<value>[-]?\d+(?:\.\d+)?(?:[eE][-+]?\d+)?)")]
    [InlineData(" ", @"(?<value>[-]?\d+(?:\.\d+)?(?:[eE][-+]?\d+)?)")]
    public void Resolve_ShouldReturnUnboundedPattern_WhenArgsIsNullOrWhitespace(string? args, string expected)
    {
        var pattern = DoubleResolver.Resolve("value", args);
        pattern.ShouldBe(expected);
    }

    [Theory]
    [InlineData("2", @"(?<value>[-]?\d+(?:\.\d{2})?(?:[eE][-+]?\d+)?)")]
    [InlineData("1,", @"(?<value>[-]?\d+(?:\.\d{1,})?(?:[eE][-+]?\d+)?)")]
    [InlineData(",3", @"(?<value>[-]?\d+(?:\.\d{0,3})?(?:[eE][-+]?\d+)?)")]
    [InlineData("1,3", @"(?<value>[-]?\d+(?:\.\d{1,3})?(?:[eE][-+]?\d+)?)")]
    public void Resolve_ShouldReturnBoundedPattern_WhenArgsAreValid(string args, string expected)
    {
        var pattern = DoubleResolver.Resolve("value", args);
        pattern.ShouldBe(expected);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("1.2")]
    [InlineData(",")]
    [InlineData("1,2,3")]
    public void Resolve_ShouldThrowArgumentException_WhenArgsAreInvalid(string args)
    {
        var ex = Should.Throw<ArgumentException>(() => DoubleResolver.Resolve("value", args));
        ex.Message.ShouldContain("Invalid argument");
    }
}
