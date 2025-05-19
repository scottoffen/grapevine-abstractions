namespace Grapevine.Abstractions.Tests;

public class HeaderValueTests
{
    [Fact]
    public void Constructor_Should_Trim_Value_And_Set_Quality()
    {
        var header = new HeaderValue(" text/html ", 0.8);
        header.Value.ShouldBe("text/html");
        header.Quality.ShouldBe(0.8);
    }

    [Fact]
    public void Constructor_With_Null_Value_Should_Throw()
    {
        Should.Throw<ArgumentNullException>(() => new HeaderValue(null!));
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    public void Constructor_With_Invalid_Quality_Should_Throw(double quality)
    {
        Should.Throw<ArgumentOutOfRangeException>(() => new HeaderValue("text/html", quality));
    }

    [Fact]
    public void ToString_With_Quality_Should_Format_Correctly()
    {
        var header = new HeaderValue("application/json", 0.95);
        header.ToString().ShouldBe("application/json;q=0.95");
    }

    [Fact]
    public void ToString_Without_Quality_Should_Return_Value()
    {
        var header = new HeaderValue("application/xml");
        header.ToString().ShouldBe("application/xml");
    }

    [Fact]
    public void Equals_Should_Be_Case_Insensitive_And_Compare_Quality()
    {
        var h1 = new HeaderValue("application/json", 0.8);
        var h2 = new HeaderValue("Application/Json", 0.8);
        h1.ShouldBe(h2);
    }

    [Fact]
    public void CompareTo_Should_Sort_By_Quality_Descending()
    {
        var lower = new HeaderValue("text/html", 0.8);
        var higher = new HeaderValue("application/json", 0.9);
        lower.CompareTo(higher).ShouldBeGreaterThan(0);
        higher.CompareTo(lower).ShouldBeLessThan(0);
    }

    [Fact]
    public void CompareTo_Should_Treat_Null_Quality_As_One()
    {
        var defaultQuality = new HeaderValue("text/html");
        var lower = new HeaderValue("text/html", 0.9);
        defaultQuality.CompareTo(lower).ShouldBeLessThan(0);
        lower.CompareTo(defaultQuality).ShouldBeGreaterThan(0);
    }

    [Fact]
    public void CompareTo_Should_Tiebreak_Using_Value()
    {
        var h1 = new HeaderValue("application/json", 0.9);
        var h2 = new HeaderValue("text/html", 0.9);
        h1.CompareTo(h2).ShouldBeLessThan(0); // because "application/json" < "text/html"
    }

    [Fact]
    public void Operator_Equality_Should_Behave_Like_Equals()
    {
        var h1 = new HeaderValue("text/plain", 0.5);
        var h2 = new HeaderValue("TEXT/PLAIN", 0.5);
        (h1 == h2).ShouldBeTrue();
        (h1 != h2).ShouldBeFalse();
    }
}
