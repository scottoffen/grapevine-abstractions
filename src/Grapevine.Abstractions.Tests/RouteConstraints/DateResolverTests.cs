using Grapevine.Abstractions.RouteConstraints;

namespace Grapevine.Abstractions.Tests.RouteConstraints;

public class DateResolverTests
{
    [Theory]
    [InlineData(null, @"(?<date>\d{4}-\d{2}-\d{2})")]
    [InlineData("", @"(?<date>\d{4}-\d{2}-\d{2})")]
    [InlineData(" ", @"(?<date>\d{4}-\d{2}-\d{2})")]
    public void Resolve_ShouldReturnIsoPattern_WhenArgsIsNullOrWhitespace(string? args, string expected)
    {
        var pattern = DateResolver.Resolve("date", args);
        pattern.ShouldBe(expected);
    }

    [Theory]
    [InlineData("iso", @"(?<date>\d{4}-\d{2}-\d{2})")]
    [InlineData("ymd", @"(?<date>\d{4}[-/]\d{2}[-/]\d{2})")]
    [InlineData("mdy", @"(?<date>\d{1,2}[-/]\d{1,2}[-/]\d{4})")]
    [InlineData("dmy", @"(?<date>\d{1,2}[-/]\d{1,2}[-/]\d{4})")]
    [InlineData("basic", @"(?<date>\d{8})")]
    public void Resolve_ShouldReturnExpectedPattern_WhenValidArgsProvided(string args, string expected)
    {
        var pattern = DateResolver.Resolve("date", args);
        pattern.ShouldBe(expected);
    }

    [Theory]
    [InlineData("xyz")]
    [InlineData("date")]
    [InlineData("ISO-8601")]
    public void Resolve_ShouldThrowArgumentException_WhenArgsAreInvalid(string args)
    {
        var ex = Should.Throw<ArgumentException>(() => DateResolver.Resolve("date", args));
        ex.ParamName.ShouldBe("args");
        ex.Message.ShouldContain("does not support the argument");
    }
}
