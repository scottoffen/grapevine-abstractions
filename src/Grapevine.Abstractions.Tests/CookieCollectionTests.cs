namespace Grapevine.Abstractions.Tests;

public class CookieCollectionTests
{
    [Fact]
    public void Add_SingleCookie_ShouldStoreCorrectly()
    {
        var collection = new CookieCollection();
        var cookie = new Cookie("theme", "dark");

        collection.Add(cookie);

        collection.Count.ShouldBe(1);
        collection["theme"].ShouldBe(cookie);
    }

    [Fact]
    public void Add_DuplicateCookieName_ShouldReplaceValue()
    {
        var collection = new CookieCollection();
        collection.Add(new Cookie("lang", "en"));
        collection.Add(new Cookie("lang", "fr"));

        collection.Count.ShouldBe(1);
        collection["lang"]!.Value.ShouldBe("fr");
    }

    [Fact]
    public void AddRange_ShouldAddAllCookies()
    {
        var collection = new CookieCollection();
        var list = new[]
        {
            new Cookie("a", "1"),
            new Cookie("b", "2")
        };

        collection.AddRange(list);

        collection.Count.ShouldBe(2);
        collection["a"]!.Value.ShouldBe("1");
        collection["b"]!.Value.ShouldBe("2");
    }

    [Fact]
    public void Indexer_ByIndex_ShouldReturnCorrectCookie()
    {
        var collection = new CookieCollection();
        collection.Add(new Cookie("one", "1"));
        collection.Add(new Cookie("two", "2"));

        collection[0]!.Name.ShouldBe("one");
        collection[1]!.Name.ShouldBe("two");
    }

    [Fact]
    public void Indexer_ByName_Set_ShouldInsertOrUpdate()
    {
        var collection = new CookieCollection();
        collection["token"] = new Cookie("token", "abc");

        collection.Count.ShouldBe(1);
        collection["token"]!.Value.ShouldBe("abc");

        collection["token"] = new Cookie("token", "xyz");
        collection["token"]!.Value.ShouldBe("xyz");
    }

    [Fact]
    public void Remove_ByName_ShouldWork()
    {
        var collection = new CookieCollection();
        collection.Add(new Cookie("x", "1"));
        collection.Remove("x").ShouldBeTrue();
        collection.Count.ShouldBe(0);
    }

    [Fact]
    public void Remove_ByObject_ShouldWork()
    {
        var cookie = new Cookie("y", "2");
        var collection = new CookieCollection();
        collection.Add(cookie);
        collection.Remove(cookie).ShouldBeTrue();
        collection.Count.ShouldBe(0);
    }

    [Fact]
    public void Clear_ShouldRemoveAll()
    {
        var collection = new CookieCollection();
        collection.Add(new Cookie("one", "1"));
        collection.Add(new Cookie("two", "2"));

        collection.Clear();

        collection.Count.ShouldBe(0);
        collection["one"].ShouldBeNull();
    }

    [Fact]
    public void CopyTo_ShouldCopyOrderedCookies()
    {
        var collection = new CookieCollection();
        collection.Add(new Cookie("first", "1"));
        collection.Add(new Cookie("second", "2"));
        var array = new Cookie[2];

        collection.CopyTo(array, 0);

        array[0].Name.ShouldBe("first");
        array[1].Name.ShouldBe("second");
    }

    [Fact]
    public void Enumerator_ShouldReturnOrderedCookies()
    {
        var collection = new CookieCollection();
        collection.Add(new Cookie("a", "x"));
        collection.Add(new Cookie("b", "y"));

        var names = new List<string>();
        foreach (var cookie in collection)
        {
            names.Add(cookie.Name);
        }

        names.ShouldBe(new[] { "a", "b" });
    }

    [Fact]
    public void GetHeaderString_ShouldMatchToString()
    {
        var collection = new CookieCollection();
        collection.Add(new Cookie("t", "v") { Path = "/", HttpOnly = true });

        var headerLines = collection.GetHeaderString();

        foreach (var line in headerLines)
        {
            line.ShouldContain("t=v");
            line.ShouldContain("Path=/");
            line.ShouldContain("HttpOnly");
        }
    }

    [Fact]
    public void Indexer_SetWithMismatchedName_ShouldThrow()
    {
        var collection = new CookieCollection();

        var ex = Should.Throw<ArgumentException>(() =>
        {
            collection["foo"] = new Cookie("bar", "baz");
        });

        ex.Message.ShouldContain("must match the indexer key");
    }

    [Fact]
    public void Indexer_NegativeIndex_ShouldThrow()
    {
        var collection = new CookieCollection();
        var ex = Should.Throw<ArgumentOutOfRangeException>(() =>
        {
            var _ = collection[-1];
        });

        ex.ParamName.ShouldBe("index");
    }

    [Fact]
    public void Indexer_TooLargeIndex_ShouldThrow()
    {
        var collection = new CookieCollection();
        collection.Add(new Cookie("z", "9"));

        var ex = Should.Throw<ArgumentOutOfRangeException>(() =>
        {
            var _ = collection[1];
        });

        ex.ParamName.ShouldBe("index");
    }
}
