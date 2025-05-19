namespace Grapevine.Abstractions.Tests;

public class ContentTypeTests
{
    [Fact]
    public void Parse_BasicContentType_ShouldSucceed()
    {
        var contentType = ContentType.Parse("application/json");

        contentType.Type.ShouldBe("application");
        contentType.SubType.ShouldBe("json");
        contentType.Charset.ShouldBeEmpty();
        contentType.IsBinary.ShouldBeFalse();
        contentType.Parameters.ShouldBeEmpty();
    }

    [Fact]
    public void Parse_WithCharset_ShouldCaptureCharset()
    {
        var contentType = ContentType.Parse("text/plain; charset=utf-8");

        contentType.Type.ShouldBe("text");
        contentType.SubType.ShouldBe("plain");
        contentType.Charset.ShouldBe("utf-8");
        contentType.IsBinary.ShouldBeFalse();
        contentType.Parameters.ShouldBeEmpty();
    }

    [Fact]
    public void Parse_WithQuotedParameter_ShouldUnquote()
    {
        var contentType = ContentType.Parse("application/octet-stream; filename=\"data with spaces.txt\"");

        contentType.Type.ShouldBe("application");
        contentType.SubType.ShouldBe("octet-stream");
        contentType.IsBinary.ShouldBeTrue();
        contentType.Parameters.ShouldContainKey("filename");
        contentType.Parameters["filename"].ShouldBe("data with spaces.txt");
    }

    [Fact]
    public void ToString_WithComplexParameters_ShouldQuoteProperly()
    {
        var contentType = new ContentType("application", "json");
        contentType.Parameters["name"] = "some name";
        contentType.Parameters["version"] = "1.0";

        var result = contentType.ToString();
        result.ShouldBe("application/json; name=\"some name\"; version=1.0");
    }

    [Fact]
    public void Equality_Comparison_ShouldIgnoreCaseAndIgnoreParameters()
    {
        var ct1 = ContentType.Parse("APPLICATION/JSON; charset=UTF-8");
        var ct2 = ContentType.Parse("application/json; charset=ascii; other=param");

        ct1.ShouldBe(ct2);
        (ct1 == ct2).ShouldBeTrue();
        (ct1 != ct2).ShouldBeFalse();
    }

    [Theory]
    [InlineData("application/json", false)]                // keyword: json
    [InlineData("application/xml", false)]                 // keyword: xml
    [InlineData("application/javascript", false)]          // keyword: javascript
    [InlineData("application/html", false)]                // keyword: html
    [InlineData("application/css", false)]                 // keyword: css
    [InlineData("text/plain", false)]                      // keyword: text
    [InlineData("application/x-www-form-urlencoded", false)] // keyword: form
    [InlineData("application/octet-stream", true)]         // not a keyword
    [InlineData("application/pdf", true)]                  // not a keyword
    [InlineData("video/mp4", true)]                        // not a keyword
    public void IsBinary_ShouldReturnCorrectValue(string contentTypeString, bool expected)
    {
        var contentType = ContentType.Parse(contentTypeString);
        contentType.IsBinary.ShouldBe(expected); // `IsBinary` is true when NOT matched
    }
}
