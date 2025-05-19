namespace Grapevine.Abstractions.Tests;

public class ProblemDetailsExtensionsTests
{
    [Fact]
    public void Add_ValidKey_ShouldStoreValue()
    {
        var extensions = new ProblemDetailsExtensions();
        extensions.Add("foo", 42);

        extensions["foo"].ShouldBe(42);
    }

    [Theory]
    [InlineData("title")]
    [InlineData("status")]
    [InlineData("detail")]
    [InlineData("instance")]
    [InlineData("type")]
    [InlineData(" Title ")]
    [InlineData("STATUS")]
    public void Add_RestrictedKey_ShouldThrow(string key)
    {
        var extensions = new ProblemDetailsExtensions();

        Should.Throw<ArgumentException>(() => extensions.Add(key, "value"))
              .Message.ShouldContain("conflicts with a reserved field name");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Add_NullOrWhitespaceKey_ShouldThrow(string? key)
    {
        var extensions = new ProblemDetailsExtensions();

        Should.Throw<ArgumentException>(() => extensions.Add(key!, "value"))
              .Message.ShouldContain("cannot be null, empty, or whitespace");
    }

    [Fact]
    public void Indexer_SetValidKey_ShouldStoreValue()
    {
        var extensions = new ProblemDetailsExtensions();
        extensions["user"] = "admin";

        extensions["user"].ShouldBe("admin");
    }

    [Fact]
    public void Indexer_SetRestrictedKey_ShouldThrow()
    {
        var extensions = new ProblemDetailsExtensions();

        Should.Throw<ArgumentException>(() => extensions["type"] = "application/problem+json")
              .Message.ShouldContain("conflicts with a reserved field name");
    }

    [Fact]
    public void AddRange_ShouldAddAllValidEntries()
    {
        var extensions = new ProblemDetailsExtensions();

        extensions.AddRange(new[]
        {
            new KeyValuePair<string, object>("one", 1),
            new KeyValuePair<string, object>("two", 2)
        });

        extensions["one"].ShouldBe(1);
        extensions["two"].ShouldBe(2);
    }

    [Fact]
    public void AddRange_WithNull_ShouldThrow()
    {
        var extensions = new ProblemDetailsExtensions();
        Should.Throw<ArgumentNullException>(() => extensions.AddRange(null!));
    }

    [Fact]
    public void TryAdd_WhenKeyDoesNotExist_ShouldAdd()
    {
        var extensions = new ProblemDetailsExtensions();
        extensions.TryAdd("foo", "bar");

        extensions["foo"].ShouldBe("bar");
    }

    [Fact]
    public void TryAdd_WhenKeyExists_ShouldNotOverwrite()
    {
        var extensions = new ProblemDetailsExtensions
        {
            ["foo"] = "bar"
        };

        extensions.TryAdd("foo", "baz");

        extensions["foo"].ShouldBe("bar");
    }

    [Fact]
    public void ToString_ShouldFormatKeyValuePairs()
    {
        var extensions = new ProblemDetailsExtensions
        {
            ["a"] = 1,
            ["b"] = true
        };

        var result = extensions.ToString();

        result.ShouldContain("a=1");
        result.ShouldContain("b=True");
    }
}
