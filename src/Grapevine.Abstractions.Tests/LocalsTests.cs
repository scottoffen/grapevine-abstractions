namespace Grapevine.Abstractions.Tests;

public class LocalsExtensionsTests
{
    [Fact]
    public void Get_ReturnsValue_WhenKeyExists()
    {
        var locals = new Locals();
        locals["key"] = "value";

        var result = locals.Get("key");

        result.ShouldBe("value");
    }

    [Fact]
    public void Get_ReturnsNull_WhenKeyDoesNotExist()
    {
        var locals = new Locals();

        var result = locals.Get("missing");

        result.ShouldBeNull();
    }

    [Fact]
    public void GetAs_ReturnsTypedValue_WhenKeyExistsAndTypeMatches()
    {
        var locals = new Locals();
        locals["key"] = 42;

        var result = locals.GetAs<int>("key");

        result.ShouldBe(42);
    }

    [Fact]
    public void GetAs_ReturnsDefault_WhenKeyDoesNotExist()
    {
        var locals = new Locals();

        var result = locals.GetAs<int>("missing");

        result.ShouldBe(0);
    }

    [Fact]
    public void GetAs_ReturnsDefault_WhenTypeMismatch()
    {
        var locals = new Locals();
        locals["key"] = "not an int";

        var result = locals.GetAs<int?>("key");

        result.ShouldBeNull(); // default of nullable int (int?) is null
    }

    [Fact]
    public void GetOrAddAs_WithValue_AddsAndReturnsTypedValue()
    {
        var locals = new Locals();

        var result = locals.GetOrAddAs("key", 123);

        result.ShouldBe(123);
        locals["key"].ShouldBe(123);
    }

    [Fact]
    public void GetOrAddAs_WithValue_ReturnsExistingValue_WhenKeyExists()
    {
        var locals = new Locals();
        locals["key"] = 456;

        var result = locals.GetOrAddAs("key", 123);

        result.ShouldBe(456);
    }

    [Fact]
    public void GetOrAddAs_WithValue_Throws_WhenTypeMismatch()
    {
        var locals = new Locals();
        locals["key"] = "wrong";

        Should.Throw<InvalidCastException>(() => locals.GetOrAddAs("key", 123));
    }

    [Fact]
    public void GetOrAddAs_WithFactory_AddsAndReturnsTypedValue()
    {
        var locals = new Locals();

        var result = locals.GetOrAddAs("key", _ => 789);

        result.ShouldBe(789);
        locals["key"].ShouldBe(789);
    }

    [Fact]
    public void GetOrAddAs_WithFactory_UsesExisting_WhenKeyExists()
    {
        var locals = new Locals();
        locals["key"] = 999;

        var result = locals.GetOrAddAs("key", _ => 0);

        result.ShouldBe(999);
    }

    [Fact]
    public void GetOrAddAs_WithFactory_Throws_WhenTypeMismatch()
    {
        var locals = new Locals();
        locals["key"] = "not an int";

        Should.Throw<InvalidCastException>(() => locals.GetOrAddAs("key", _ => 100));
    }

    [Fact]
    public void Set_AddsOrUpdatesKey_WhenValueIsNotNull()
    {
        var locals = new Locals();

        locals.Set("key", "value");

        locals["key"].ShouldBe("value");

        locals.Set("key", "newvalue");

        locals["key"].ShouldBe("newvalue");
    }

    [Fact]
    public void Set_RemovesKey_WhenValueIsNull()
    {
        var locals = new Locals();
        locals["key"] = "value";

        locals.Set("key", null);

        locals.ContainsKey("key").ShouldBeFalse();
    }
}
