namespace Grapevine.Abstractions.Tests;

public class PrefixCollectionTests
{
    [Fact]
    public void Add_ValidPrefix_ShouldSucceed()
    {
        var collection = new PrefixCollection();
        collection.Add("http://localhost:1234/");

        collection.Count.ShouldBe(1);
        collection.Contains("http://localhost:1234/").ShouldBeTrue();
    }

    [Fact]
    public void Add_InvalidPrefix_ShouldThrowArgumentException()
    {
        var collection = new PrefixCollection();

        var ex = Should.Throw<ArgumentException>(() =>
            collection.Add("invalid-prefix"));

        ex.Message.ShouldContain("invalid according to the configured evaluation rules");
    }

    [Fact]
    public void TryAdd_ValidPrefix_ShouldReturnTrue_AndAddToCollection()
    {
        var collection = new PrefixCollection();
        var result = collection.TryAdd("https://example.com/");

        result.ShouldBeTrue();
        collection.Count.ShouldBe(1);
    }

    [Fact]
    public void TryAdd_InvalidPrefix_ShouldReturnFalse_AndNotAddToCollection()
    {
        var collection = new PrefixCollection();
        var result = collection.TryAdd("ftp://example.com");

        result.ShouldBeFalse();
        collection.Count.ShouldBe(0);
    }

    [Fact]
    public void Contains_ShouldReturnTrueForExistingPrefix()
    {
        var collection = new PrefixCollection();
        collection.Add("http://test/");

        collection.Contains("http://test/").ShouldBeTrue();
    }

    [Fact]
    public void Remove_ShouldDeletePrefix()
    {
        var collection = new PrefixCollection();
        collection.Add("http://remove-me/");

        collection.Remove("http://remove-me/").ShouldBeTrue();
        collection.Contains("http://remove-me/").ShouldBeFalse();
    }

    [Fact]
    public void Clear_ShouldRemoveAllPrefixes()
    {
        var collection = new PrefixCollection();
        collection.Add("http://one/");
        collection.Add("http://two/");

        collection.Clear();

        collection.Count.ShouldBe(0);
    }

    [Fact]
    public void CopyTo_ShouldCopyContentsToArray()
    {
        var collection = new PrefixCollection();
        collection.Add("http://copy/");
        var array = new string[1];

        collection.CopyTo(array, 0);

        array[0].ShouldBe("http://copy/");
    }

    [Fact]
    public void Enumerator_ShouldReturnAllPrefixes()
    {
        var collection = new PrefixCollection();
        collection.Add("http://a/");
        collection.Add("http://b/");

        var items = collection.ToList();

        items.ShouldContain("http://a/");
        items.ShouldContain("http://b/");
        items.Count.ShouldBe(2);
    }

    [Fact]
    public void SettingPrefixEvaluatorToNull_ShouldThrow()
    {
        var collection = new PrefixCollection();

        Should.Throw<ArgumentNullException>(() => collection.PrefixEvaluator = null!);
    }

    [Fact]
    public void CustomPrefixEvaluator_ShouldOverrideDefaultValidation()
    {
        var collection = new PrefixCollection
        {
            PrefixEvaluator = prefix => prefix == "custom://pass/"
        };

        collection.TryAdd("custom://pass/").ShouldBeTrue();
        collection.TryAdd("http://fail/").ShouldBeFalse();
        collection.Count.ShouldBe(1);
    }

    [Theory]
    [InlineData("http://localhost/")]
    [InlineData("https://example.com/")]
    [InlineData("https://127.0.0.1:5000/")]
    public void Add_WithValidPrefixes_ShouldSucceed(string validPrefix)
    {
        var collection = new PrefixCollection();
        collection.Add(validPrefix);

        collection.Contains(validPrefix).ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("http://no-trailing-slash")]
    [InlineData("https://")]
    [InlineData("ftp://example.com/")]
    [InlineData("invalid-prefix")]
    public void Add_WithInvalidPrefixes_ShouldThrow(string? invalidPrefix)
    {
        var collection = new PrefixCollection();

        Should.Throw<ArgumentException>(() => collection.Add(invalidPrefix!));
        collection.Count.ShouldBe(0);
    }

    [Theory]
    [InlineData("http://localhost/")]
    [InlineData("https://secure/")]
    public void TryAdd_WithValidPrefixes_ShouldReturnTrue(string validPrefix)
    {
        var collection = new PrefixCollection();

        collection.TryAdd(validPrefix).ShouldBeTrue();
        collection.Contains(validPrefix).ShouldBeTrue();
    }

    [Theory]
    [InlineData("not-a-url")]
    [InlineData("ftp://unsupported/")]
    [InlineData("")]
    [InlineData(null)]
    public void TryAdd_WithInvalidPrefixes_ShouldReturnFalse(string? invalidPrefix)
    {
        var collection = new PrefixCollection();

        collection.TryAdd(invalidPrefix!).ShouldBeFalse();
        collection.Count.ShouldBe(0);
    }

    [Theory]
    [InlineData("http://localhost/", true)]
    [InlineData("ftp://invalid/", false)]
    public void CustomPrefixEvaluator_ShouldRespectCustomLogic(string prefix, bool expected)
    {
        var collection = new PrefixCollection
        {
            PrefixEvaluator = p => p.StartsWith("http://")
        };

        collection.TryAdd(prefix).ShouldBe(expected);
    }
}
