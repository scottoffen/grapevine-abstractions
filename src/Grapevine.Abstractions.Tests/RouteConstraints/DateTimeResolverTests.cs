using Grapevine.Abstractions.RouteConstraints;

namespace Grapevine.Abstractions.Tests.RouteConstraints;

public class DateTimeResolverTests
{
    [Theory]
    [InlineData(null, @"(?<timestamp>\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2})")]
    [InlineData("", @"(?<timestamp>\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2})")]
    [InlineData(" ", @"(?<timestamp>\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2})")]
    public void Resolve_ShouldReturnIsoPattern_WhenArgsIsNullOrWhitespace(string? args, string expected)
    {
        var pattern = DateTimeResolver.Resolve("timestamp", args);
        pattern.ShouldBe(expected);
    }

    [Theory]
    [InlineData("iso", @"(?<timestamp>\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2})")]
    [InlineData("date", @"(?<timestamp>\d{4}-\d{2}-\d{2})")]
    [InlineData("time", @"(?<timestamp>\d{2}:\d{2}:\d{2})")]
    [InlineData("basic", @"(?<timestamp>\d{8})")]
    [InlineData("rfc", @"(?<timestamp>[A-Za-z]{3}, \d{2} [A-Za-z]{3} \d{4} \d{2}:\d{2}:\d{2} GMT)")]
    public void Resolve_ShouldReturnExpectedPattern_WhenValidArgsProvided(string args, string expected)
    {
        var pattern = DateTimeResolver.Resolve("timestamp", args);
        pattern.ShouldBe(expected);
    }

    [Theory]
    [InlineData("xyz")]
    [InlineData("datetime")]
    [InlineData("iso8601")]
    public void Resolve_ShouldThrowArgumentException_WhenArgsAreInvalid(string args)
    {
        var ex = Should.Throw<ArgumentException>(() => DateTimeResolver.Resolve("timestamp", args));
        ex.ParamName.ShouldBe("args");
        ex.Message.ShouldContain("does not support the argument");
    }
}
