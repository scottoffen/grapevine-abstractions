namespace Grapevine.Abstractions.Tests;

public class HeaderTests
{
    [Fact]
    public void Constructor_WithNullName_ShouldThrow()
    {
        Should.Throw<ArgumentNullException>(() => new Header(null!));
    }

    [Fact]
    public void Constructor_WithNoValue_ShouldInitializeEmpty()
    {
        var header = new Header("X-Test");
        header.Name.ShouldBe("X-Test");
        header.HasValue.ShouldBeFalse();
        header.IsMultipart.ShouldBeFalse();
        header.Values.ShouldBeEmpty();
        header.Value.ShouldBeNull();
    }

    [Theory]
    [InlineData("text/html", 1, "text/html")]
    [InlineData("text/html;q=0.8", 1, "text/html;q=0.8")]
    [InlineData("text/html;q=0.8, application/json;q=0.9", 2, "text/html;q=0.8")]
    public void Constructor_WithValues_ShouldParseCorrectly(string value, int expectedCount, string expectedFirst)
    {
        var header = new Header("Accept", value);
        header.HasValue.ShouldBeTrue();
        header.Values.Count.ShouldBe(expectedCount);
        header.Value.ShouldBe(expectedFirst);
        header.IsMultipart.ShouldBe(expectedCount > 1);
    }

    [Fact]
    public void Constructor_WithMalformedQuality_ShouldFallbackToValueOnly()
    {
        var header = new Header("Accept", "text/html;q=abc");
        header.Values.Count.ShouldBe(1);
        header.Values[0].Value.ShouldBe("text/html");
        header.Values[0].Quality.ShouldBeNull();
    }

    [Fact]
    public void ToString_Should_Format_Correctly()
    {
        var header = new Header("Accept", "text/html;q=0.8, application/json;q=0.9");
        header.ToString().ShouldBe("Accept: text/html;q=0.8, application/json;q=0.9");
    }

    [Fact]
    public void Equality_Should_Compare_Name_And_Values()
    {
        var a = new Header("Accept", "text/html;q=0.8, application/json;q=0.9");
        var b = new Header("ACCEPT", "text/html;q=0.8, application/json;q=0.9");
        var c = new Header("Accept", "application/xml");

        (a == b).ShouldBeTrue();
        a.Equals(b).ShouldBeTrue();
        a.Equals((object)b).ShouldBeTrue();
        a.GetHashCode().ShouldBe(b.GetHashCode());

        (a == c).ShouldBeFalse();
        a.Equals(c).ShouldBeFalse();
    }

    [Fact]
    public void ImplicitOperator_ShouldReturnHeaderString()
    {
        Header header = new Header("X-Foo", "bar");
        string asString = header;
        asString.ShouldBe("X-Foo: bar");
    }

    [Fact]
    public void SequenceEquality_Should_RespectOrder()
    {
        var h1 = new Header("Accept", "application/json;q=0.9, text/html;q=0.8");
        var h2 = new Header("Accept", "text/html;q=0.8, application/json;q=0.9");

        h1.Equals(h2).ShouldBeFalse();
    }
}
