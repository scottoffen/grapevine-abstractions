using Grapevine.Abstractions.RouteConstraints;

namespace Grapevine.Abstractions.Tests.RouteConstraints;

public class BoolResolverTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Resolve_ShouldReturnExpectedPattern_WhenArgsAreNullOrWhitespace(string? args)
    {
        var pattern = BoolResolver.Resolve("flag", args);
        pattern.ShouldBe("(?<flag>true|false)");
    }

    [Theory]
    [InlineData("true")]
    [InlineData("1")]
    [InlineData("any")]
    public void Resolve_ShouldThrowArgumentException_WhenArgsAreProvided(string args)
    {
        var ex = Should.Throw<ArgumentException>(() => BoolResolver.Resolve("flag", args));
        ex.ParamName.ShouldBe("args");
        ex.Message.ShouldContain("does not accept arguments");
    }
}
