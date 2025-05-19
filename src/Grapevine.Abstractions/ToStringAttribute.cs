namespace Grapevine.Abstractions;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
internal class ToStringAttribute : Attribute
{
    /// <summary>
    /// Gets the value of the attribute
    /// </summary>
    public string Value { get; }

    public ToStringAttribute(string value)
    {
        Value = value;
    }
}
