using Grapevine.Abstractions.RouteConstraints;

namespace Grapevine.Abstractions.Tests.RouteConstraints;

public class LengthPatternResolverTests
{
    [Theory]
    [InlineData(null, false, "+")]
    [InlineData("", false, "+")]
    [InlineData(" ", false, "+")]
    public void Resolve_WithNullOrEmptyOrWhitespace_ShouldReturnPlus(string? input, bool allowZeroMin, string expected)
    {
        LengthPatternResolver.Resolve(input, allowZeroMin).ShouldBe(expected);
    }

    [Theory]
    [InlineData("1", false, "{1}")]
    [InlineData("3", false, "{3}")]
    [InlineData("5", false, "{5}")]
    public void Resolve_WithExactDigitCount_ShouldReturnExactQuantifier(string input, bool allowZeroMin, string expected)
    {
        LengthPatternResolver.Resolve(input, allowZeroMin).ShouldBe(expected);
    }

    [Theory]
    [InlineData("1,", false, "{1,}")]
    [InlineData("4,", false, "{4,}")]
    public void Resolve_WithLowerBoundOnly_ShouldReturnMinOnlyQuantifier(string input, bool allowZeroMin, string expected)
    {
        LengthPatternResolver.Resolve(input, allowZeroMin).ShouldBe(expected);
    }

    [Theory]
    [InlineData(",3", false, "{1,3}")]
    [InlineData(",5", false, "{1,5}")]
    [InlineData(",3", true, "{0,3}")]
    [InlineData(",5", true, "{0,5}")]
    public void Resolve_WithUpperBoundOnly_ShouldApplyDefaultOrZeroMin(string input, bool allowZeroMin, string expected)
    {
        LengthPatternResolver.Resolve(input, allowZeroMin).ShouldBe(expected);
    }

    [Theory]
    [InlineData("1,3", false, "{1,3}")]
    [InlineData("2,5", false, "{2,5}")]
    [InlineData("0,3", true, "{0,3}")]
    public void Resolve_WithMinAndMax_ShouldReturnBoundedQuantifier(string input, bool allowZeroMin, string expected)
    {
        LengthPatternResolver.Resolve(input, allowZeroMin).ShouldBe(expected);
    }

    [Theory]
    [InlineData(" 3 ", false, "{3}")]
    [InlineData(" 1 , 4 ", false, "{1,4}")]
    [InlineData(" , 5 ", true, "{0,5}")]
    public void Resolve_WithInputContainingSpaces_ShouldReturnTrimmedQuantifier(string input, bool allowZeroMin, string expected)
    {
        LengthPatternResolver.Resolve(input, allowZeroMin).ShouldBe(expected);
    }

    [Theory]
    [InlineData("0,", true, "{0,}")]
    [InlineData("0,2", true, "{0,2}")]
    public void Resolve_WithAllowZeroMin_ShouldAcceptZeroMinimum(string input, bool allowZeroMin, string expected)
    {
        LengthPatternResolver.Resolve(input, allowZeroMin).ShouldBe(expected);
    }

    [Theory]
    [InlineData("abc", false)]
    [InlineData("1,,5", false)]
    [InlineData(",,", false)]
    [InlineData("-1", false)]
    [InlineData("2,-5", false)]
    [InlineData("-1,5", false)]
    [InlineData("5,2", false)]
    [InlineData("0", false)] // zero min disallowed
    [InlineData("0,2", false)]
    public void Resolve_WithInvalidInput_ShouldThrowArgumentException(string input, bool allowZeroMin)
    {
        Should.Throw<ArgumentException>(() => LengthPatternResolver.Resolve(input, allowZeroMin));
    }
}
