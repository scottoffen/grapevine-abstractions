namespace Grapevine.Abstractions.Tests;

public class HeaderCollectionTests
{
    [Fact]
    public void Add_HeaderObject_ShouldIncreaseCount()
    {
        var collection = new HeaderCollection<TestHeaders>();
        collection.Add(new Header("X-Test", "value"));
        collection.Count.ShouldBe(1);
    }

    [Fact]
    public void Add_TEnum_ShouldUseMappedName()
    {
        var collection = new HeaderCollection<TestHeaders>();
        collection.Add(TestHeaders.One, "abc");

        collection.Count.ShouldBe(1);
        collection.Contains(TestHeaders.One).ShouldBeTrue();

        var header = collection[TestHeaders.One];
        header.ShouldNotBeNull();
        header.Name.ShouldBe("X-Test-One");
        header.Value.ShouldBe("abc");
    }

    [Fact]
    public void Add_String_ShouldStoreHeader()
    {
        var collection = new HeaderCollection<TestHeaders>();
        collection.Add("x-custom", "abc");

        collection.Count.ShouldBe(1);
        collection["x-custom"]!.Value.ShouldBe("abc");
    }

    [Fact]
    public void AddRange_ShouldAddAllHeaders()
    {
        var collection = new HeaderCollection<TestHeaders>();

        var headers = new[]
        {
            new Header("A", "1"),
            new Header("B", "2")
        };

        collection.AddRange(headers);
        collection.Count.ShouldBe(2);
    }

    [Fact]
    public void Contains_Header_ShouldReturnTrue()
    {
        var header = new Header("A", "value");
        var collection = new HeaderCollection<TestHeaders>();
        collection.Add(header);

        collection.Contains(header).ShouldBeTrue();
    }

    [Fact]
    public void Contains_TEnum_ShouldBeCaseInsensitive()
    {
        var collection = new HeaderCollection<TestHeaders>();
        collection.Add("x-test-two", "foo");

        collection.Contains(TestHeaders.Two).ShouldBeTrue();
    }

    [Fact]
    public void Remove_Header_ShouldWork()
    {
        var header = new Header("X-Remove", "value");
        var collection = new HeaderCollection<TestHeaders>();
        collection.Add(header);

        collection.Remove(header).ShouldBeTrue();
        collection.Count.ShouldBe(0);
    }

    [Fact]
    public void RemoveAll_ByEnum_ShouldRemoveAllMatching()
    {
        var collection = new HeaderCollection<TestHeaders>();
        collection.Add(TestHeaders.Three, "v1");
        collection.Add("X-Test-Three", "v2");

        collection.RemoveAll(TestHeaders.Three).ShouldBeTrue();
        collection.Count.ShouldBe(0);
    }

    [Fact]
    public void GetAll_ShouldReturnAllMatching()
    {
        var collection = new HeaderCollection<TestHeaders>();
        collection.Add(TestHeaders.Two, "one");
        collection.Add("X-Test-Two", "two");

        var all = collection.GetAll(TestHeaders.Two);
        all.Count().ShouldBe(2);

        var allByString = collection.GetAll("X-Test-Two");
        allByString.Count().ShouldBe(2);
    }

    [Fact]
    public void TryGetValue_ShouldReturnFirstMatch()
    {
        var collection = new HeaderCollection<TestHeaders>();
        collection.Add(TestHeaders.One, "a");
        collection.Add(TestHeaders.One, "b");

        collection.TryGetValue(TestHeaders.One, out var result).ShouldBeTrue();
        result!.Value.ShouldBe("a");
    }

    [Fact]
    public void Indexer_ByString_ShouldBeCaseInsensitive()
    {
        var collection = new HeaderCollection<TestHeaders>();
        collection.Add("X-Test-Two", "val");

        collection["x-test-two"]!.Value.ShouldBe("val");
    }

    [Fact]
    public void CopyTo_GenericArray_ShouldWork()
    {
        var collection = new HeaderCollection<TestHeaders>();
        collection.Add("X", "1");

        var array = new Header[1];
        collection.CopyTo(array, 0);

        array[0].Name.ShouldBe("X");
    }

    [Fact]
    public void CopyTo_ObjectArray_InvalidType_ShouldThrow()
    {
        var collection = new HeaderCollection<TestHeaders>();

        var ex = Should.Throw<ArgumentException>(() =>
        {
            collection.CopyTo(new object[1], 0);
        });

        ex.Message.ShouldContain("Header[]");
    }

    [Fact]
    public void Clear_ShouldEmptyCollection()
    {
        var collection = new HeaderCollection<TestHeaders>();
        collection.Add("X", "1");
        collection.Clear();

        collection.Count.ShouldBe(0);
    }

    [Fact]
    public void RequestHeaderEnum_ShouldSupportBasicMapping()
    {
        var collection = new HeaderCollection<RequestHeader>();
        collection.Add(RequestHeader.UserAgent, "MyApp/1.0");
        collection.Add(RequestHeader.AcceptEncoding, "gzip");

        collection.Count.ShouldBe(2);
        collection.Contains(RequestHeader.UserAgent).ShouldBeTrue();
        collection[RequestHeader.AcceptEncoding]!.Name.ShouldBe("Accept-Encoding");
        collection[RequestHeader.AcceptEncoding]!.Value.ShouldBe("gzip");
    }

    [Fact]
    public void ResponseHeaderEnum_ShouldSupportBasicMapping()
    {
        var collection = new HeaderCollection<ResponseHeader>();
        collection.Add(ResponseHeader.Server, "Apache");
        collection.Add(ResponseHeader.ContentType, "text/html");

        collection.Count.ShouldBe(2);
        collection.Contains(ResponseHeader.Server).ShouldBeTrue();
        collection[ResponseHeader.ContentType]!.Name.ShouldBe("Content-Type");
        collection[ResponseHeader.ContentType]!.Value.ShouldBe("text/html");
    }
}

public enum TestHeaders
{
    [ToString("X-Test-One")]
    One,

    [ToString("X-Test-Two")]
    Two,

    [ToString("X-Test-Three")]
    Three
}
