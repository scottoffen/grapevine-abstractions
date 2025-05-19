using System.Collections.Specialized;

namespace Grapevine.Abstractions.Tests;

public sealed class NameValueCollectionExtensionsTests
{
    [Fact]
    public void GetValue_WithValidConvertibleValue_ReturnsConvertedValue()
    {
        var collection = new NameValueCollection { { "key", "123" } };

        int result = collection.GetValue<int>("key");

        result.ShouldBe(123);
    }

    [Fact]
    public void GetValue_WithInvalidConvertibleValue_ReturnsDefault()
    {
        var collection = new NameValueCollection { { "key", "notanumber" } };

        int result = collection.GetValue<int>("key");

        result.ShouldBe(default);
    }

    [Fact]
    public void GetValue_WithMissingKey_ReturnsDefault()
    {
        var collection = new NameValueCollection();

        bool result = collection.GetValue<bool>("missing");

        result.ShouldBe(default);
    }

    [Fact]
    public void GetValue_WithDefaultValue_ReturnsConvertedValueIfPresent()
    {
        var collection = new NameValueCollection { { "key", "true" } };

        bool result = collection.GetValue("key", false);

        result.ShouldBeTrue();
    }

    [Fact]
    public void GetValue_WithDefaultValue_ReturnsDefaultIfMissingOrInvalid()
    {
        var collection = new NameValueCollection();

        int result = collection.GetValue("missing", 42);

        result.ShouldBe(42);
    }

    [Fact]
    public void TryGetValue_WithPresentKey_ReturnsTrueAndValue()
    {
        var collection = new NameValueCollection { { "key", "value" } };

        var result = collection.TryGetValue("key", out var value);

        result.ShouldBeTrue();
        value.ShouldBe("value");
    }

    [Fact]
    public void TryGetValue_WithMissingKey_ReturnsFalseAndNull()
    {
        var collection = new NameValueCollection();

        var result = collection.TryGetValue("missing", out var value);

        result.ShouldBeFalse();
        value.ShouldBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetValue_WithInvalidKey_ThrowsArgumentNullException(string? invalidKey)
    {
        var collection = new NameValueCollection();

        Should.Throw<ArgumentNullException>(() => collection.GetValue<int>(invalidKey!));
    }

    [Fact]
    public void GetValue_WithNullCollection_ThrowsArgumentNullException()
    {
        NameValueCollection? collection = null;

        Should.Throw<ArgumentNullException>(() => collection!.GetValue<int>("key"));
    }

    [Fact]
    public void TryGetValue_WithNullCollection_ThrowsArgumentNullException()
    {
        NameValueCollection? collection = null;

        Should.Throw<ArgumentNullException>(() => collection!.TryGetValue("key", out _));
    }

    [Fact]
    public void TryGetValue_WithWhitespaceKey_ThrowsArgumentNullException()
    {
        var collection = new NameValueCollection();

        Should.Throw<ArgumentNullException>(() => collection.TryGetValue("   ", out _));
    }

    [Fact]
    public void GetValue_CustomTypeWithoutConverter_ReturnsDefault()
    {
        var collection = new NameValueCollection { { "key", "test" } };

        var result = collection.GetValue<CustomType>("key");

        result.ShouldBeNull();
    }

    private class CustomType { }
}
