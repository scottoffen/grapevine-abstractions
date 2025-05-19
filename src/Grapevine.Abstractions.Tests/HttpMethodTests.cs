namespace Grapevine.Abstractions.Tests;

public class HttpMethodTests
{
    [Fact]
    public void Parse_ShouldReturnSameInstance_ForKnownMethods()
    {
        var parsed = HttpMethod.Parse("GET");
        parsed.ShouldBeSameAs(HttpMethod.Get);
    }

    [Theory]
    [InlineData("GET", "GET")]
    [InlineData("get", "GET")]
    [InlineData("  post  ", "POST")]
    public void ImplicitCast_FromString_ShouldReturnInternedInstance(string input, string expectedName)
    {
        HttpMethod method = input;
        method.Name.ShouldBe(expectedName);
        HttpMethod.Known.ShouldContain(method);
    }

    [Fact]
    public void ImplicitCast_ToString_ShouldReturnName()
    {
        string name = HttpMethod.Put;
        name.ShouldBe("PUT");
    }

    [Fact]
    public void EqualityOperator_ShouldReturnTrue_ForEqualInstances()
    {
        var method = HttpMethod.Parse("GET");
        (HttpMethod.Get == method).ShouldBeTrue();
    }

    [Fact]
    public void InequalityOperator_ShouldReturnTrue_ForDifferentInstances()
    {
        (HttpMethod.Get != HttpMethod.Post).ShouldBeTrue();
    }

    [Theory]
    [InlineData("get", "GET")]
    [InlineData("PoSt", "POST")]
    public void Equals_ShouldBeCaseInsensitive(string input, string reference)
    {
        var method = HttpMethod.Parse(input);
        method.Equals(HttpMethod.Parse(reference)).ShouldBeTrue();
    }

    [Fact]
    public void Matches_ShouldReturnTrue_WhenSame()
    {
        var method = HttpMethod.Parse("HEAD");
        HttpMethod.Head.Matches(method).ShouldBeTrue();
    }

    [Fact]
    public void Matches_ShouldReturnTrue_WhenEitherIsAny()
    {
        HttpMethod.Any.Matches(HttpMethod.Delete).ShouldBeTrue();
        HttpMethod.Get.Matches(HttpMethod.Any).ShouldBeTrue();
    }

    [Fact]
    public void Known_ShouldContainPredefinedMethods()
    {
        HttpMethod.Known.ShouldContain(HttpMethod.Get);
        HttpMethod.Known.ShouldContain(HttpMethod.Post);
        HttpMethod.Known.ShouldContain(HttpMethod.Put);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ImplicitCast_FromInvalidString_ShouldThrow(string? input)
    {
        Should.Throw<ArgumentNullException>(() =>
        {
            HttpMethod method = input!;
        });
    }
}
