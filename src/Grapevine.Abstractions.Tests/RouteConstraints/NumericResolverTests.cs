using Grapevine.Abstractions.RouteConstraints;

namespace Grapevine.Abstractions.Tests.RouteConstraints;

public class NumericResolverTests
{
    [Theory]
    [InlineData(null, @"(?<digits>\d+)")]
    [InlineData("", @"(?<digits>\d+)")]
    [InlineData(" ", @"(?<digits>\d+)")]
    public void Resolve_ShouldReturnUnboundedPattern_WhenArgsIsNullOrWhitespace(string? args, string expected)
    {
        var pattern = NumericResolver.Resolve("digits", args);
        pattern.ShouldBe(expected);
    }

    [Theory]
    [InlineData("3", @"(?<digits>\d{3})")]
    [InlineData("1,", @"(?<digits>\d{1,})")]
    [InlineData(",5", @"(?<digits>\d{1,5})")]
    [InlineData("1,5", @"(?<digits>\d{1,5})")]
    public void Resolve_ShouldReturnBoundedPattern_WhenArgsAreValid(string args, string expected)
    {
        var pattern = NumericResolver.Resolve("digits", args);
        pattern.ShouldBe(expected);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("1.2")]
    [InlineData(",,")]
    [InlineData("1,2,3")]
    public void Resolve_ShouldThrowArgumentException_WhenArgsAreInvalid(string args)
    {
        var ex = Should.Throw<ArgumentException>(() => NumericResolver.Resolve("digits", args));
        ex.Message.ShouldContain("Invalid argument");
    }
}
