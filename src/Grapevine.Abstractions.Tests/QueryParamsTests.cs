using System.Collections.Specialized;

namespace Grapevine.Abstractions.Tests;

public class QueryParamsTests
{
    [Fact]
    public void DefaultConstructor_InitializesEmptyCollection()
    {
        var queryParams = new QueryParams();
        queryParams.Count.ShouldBe(0);
    }

    [Fact]
    public void Constructor_WithNameValueCollection_CopiesValues()
    {
        var collection = new NameValueCollection
        {
            { "key1", "value1" },
            { "key2", "value2" }
        };

        var queryParams = new QueryParams(collection);

        queryParams.Count.ShouldBe(2);
        queryParams["key1"].ShouldBe("value1");
        queryParams["key2"].ShouldBe("value2");
    }

    [Fact]
    public void Constructor_WithEnumerable_CopiesValues()
    {
        var items = new List<KeyValuePair<string, string?>>
        {
            new KeyValuePair<string, string?>("key1", "value1"),
            new KeyValuePair<string, string?>("key2", "value2")
        };

        var queryParams = new QueryParams(items);

        queryParams.Count.ShouldBe(2);
        queryParams["key1"].ShouldBe("value1");
        queryParams["key2"].ShouldBe("value2");
    }

    [Fact]
    public void Parse_EmptyString_ReturnsEmptyCollection()
    {
        var result = QueryParams.Parse("");
        result.Count.ShouldBe(0);
    }

    [Fact]
    public void Parse_LeadingQuestionMark_ParsesCorrectly()
    {
        var result = QueryParams.Parse("?foo=bar&baz=qux");

        result.Count.ShouldBe(2);
        result["foo"].ShouldBe("bar");
        result["baz"].ShouldBe("qux");
    }

    [Fact]
    public void Parse_UnescapedCharacters_AreDecoded()
    {
        var result = QueryParams.Parse("name=John%20Doe&city=New%20York");

        result["name"].ShouldBe("John Doe");
        result["city"].ShouldBe("New York");
    }

    [Fact]
    public void Parse_MissingValue_IsStoredAsNull()
    {
        var result = QueryParams.Parse("key1=&key2");

        result["key1"].ShouldBe(string.Empty);
        result["key2"].ShouldBeNull();
    }

    [Fact]
    public void Parse_MultipleValuesWithSameKey_ArePreserved()
    {
        var result = QueryParams.Parse("tag=one&tag=two&tag=three");

        result.GetValues("tag").ShouldBe(new[] { "one", "two", "three" });
    }

    [Fact]
    public void ToString_ReturnsFormattedQueryString()
    {
        var queryParams = new QueryParams
        {
            { "key1", "value1" },
            { "key2", "value2" }
        };

        var output = queryParams.ToString();

        output.ShouldContain("key1=value1");
        output.ShouldContain("key2=value2");
        output.ShouldContain("&");
    }

    [Fact]
    public void ToString_HandlesMultipleValues()
    {
        var queryParams = new QueryParams();
        queryParams.Add("key", "a");
        queryParams.Add("key", "b");

        var result = queryParams.ToString();

        result.ShouldBe("key=a&key=b");
    }

    [Fact]
    public void Parse_IgnoresEmptyKeys()
    {
        var result = QueryParams.Parse("=value&key=value2");

        result.Count.ShouldBe(1);
        result["key"].ShouldBe("value2");
    }
}
