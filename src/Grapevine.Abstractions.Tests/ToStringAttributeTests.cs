using System.Reflection;

namespace Grapevine.Abstractions.Tests;

public class ToStringAttributeTests
{
    [Fact]
    public void Constructor_Should_Set_Value_Property()
    {
        var expectedValue = "TestValue";
        var attribute = new ToStringAttribute(expectedValue);
        attribute.Value.ShouldBe(expectedValue);
    }

    [Fact]
    public void Value_Property_Should_Not_Be_Null_When_Set()
    {
        var attribute = new ToStringAttribute("NonNull");
        attribute.Value.ShouldNotBeNull();
    }

    [Fact]
    public void Can_Be_Applied_To_Field_And_Reflected()
    {
        var fieldInfo = typeof(SampleWithAttribute).GetField(nameof(SampleWithAttribute.Foo), BindingFlags.Public | BindingFlags.Static);
        var attr = fieldInfo?.GetCustomAttribute<ToStringAttribute>();

        attr.ShouldNotBeNull();
        attr!.Value.ShouldBe("FieldValue");
    }

    private class SampleWithAttribute
    {
        [ToString("FieldValue")]
        public static string Foo = "Hello";
    }
}
