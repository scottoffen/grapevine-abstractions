namespace Grapevine.Abstractions.Tests;

using Grapevine.Abstractions;

public class RouteTemplateTests
{
    [Theory]
    [InlineData("api/users", "^api/users$")]
    [InlineData("api/{id}", @"^api/(?<id>[^/]+)$")]
    [InlineData("api/{id:int}", @"^api/(?<id>-?\d+)$")]
    [InlineData("api/{name:alpha(2)}", @"^api/(?<name>[a-zA-Z]{2})$")]
    [InlineData("*", "^.*$")]
    public void Constructor_ShouldCreateExpectedRegexPattern(string template, string expectedPattern)
    {
        var routeTemplate = new RouteTemplate(template);
        routeTemplate.Pattern.ToString().ShouldBe(expectedPattern);
    }

    [Fact]
    public void Parameters_ShouldReturnExpectedCount()
    {
        var routeTemplate = new RouteTemplate("api/{id}/{name:alpha}");
        routeTemplate.Parameters.ShouldBe(2);
    }

    [Fact]
    public void Segments_ShouldReturnExpectedCount()
    {
        var routeTemplate = new RouteTemplate("api/v1/{id:int}");
        routeTemplate.Segments.ShouldBe(3);
    }

    [Fact]
    public void Matches_ShouldReturnTrue_WhenPathIsValid()
    {
        var routeTemplate = new RouteTemplate("api/{id:int}");
        routeTemplate.Matches("api/42").ShouldBeTrue();
    }

    [Fact]
    public void Matches_ShouldReturnFalse_WhenPathIsInvalid()
    {
        var routeTemplate = new RouteTemplate("api/{id:int}");
        routeTemplate.Matches("api/foo").ShouldBeFalse();
    }

    [Fact]
    public void GetRouteParams_ShouldReturnCorrectParameters()
    {
        var routeTemplate = new RouteTemplate("api/{id:int}/{name:alpha}");
        var result = routeTemplate.GetRouteParams("api/123/John");

        result.ShouldNotBeNull();
        result.AllKeys.ShouldContain("id");
        result["id"].ShouldBe("123");
        result.AllKeys.ShouldContain("name");
        result["name"].ShouldBe("John");
    }

    [Fact]
    public void CompareTo_ShouldPreferTemplateWithMoreSegments()
    {
        var a = new RouteTemplate("api/{id}");
        var b = new RouteTemplate("api/v1/{id}");

        a.CompareTo(b).ShouldBeGreaterThan(0);
        b.CompareTo(a).ShouldBeLessThan(0);
    }

    [Fact]
    public void CompareTo_ShouldPreferTemplateWithFewerParameters_WhenSegmentsEqual()
    {
        var a = new RouteTemplate("api/{id}/{name}");
        var b = new RouteTemplate("api/{id}/users");

        a.CompareTo(b).ShouldBeGreaterThan(0);
        b.CompareTo(a).ShouldBeLessThan(0);
    }
}
