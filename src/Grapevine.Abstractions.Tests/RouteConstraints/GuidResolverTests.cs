using Grapevine.Abstractions.RouteConstraints;

namespace Grapevine.Abstractions.Tests.RouteConstraints;

public class GuidResolverTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Resolve_ShouldReturnExpectedPattern_WhenArgsIsNullOrWhitespace(string? args)
    {
        var expected = @"(?<id>[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})";
        var pattern = GuidResolver.Resolve("id", args);
        pattern.ShouldBe(expected);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("123")]
    [InlineData("guid")]
    public void Resolve_ShouldThrowArgumentException_WhenArgsAreProvided(string args)
    {
        var ex = Should.Throw<ArgumentException>(() => GuidResolver.Resolve("id", args));
        ex.ParamName.ShouldBe("args");
        ex.Message.ShouldContain("does not accept any arguments");
    }
}
