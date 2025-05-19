namespace Grapevine.Abstractions.Tests;

public class RemoteAddressTests
{
    [Fact]
    public void Constructor_Should_Set_Address_And_Port()
    {
        var address = new RemoteAddress("127.0.0.1", 8080);
        address.Address.ShouldBe("127.0.0.1");
        address.Port.ShouldBe(8080);
    }

    [Fact]
    public void ToString_Should_Return_Address_And_Port()
    {
        var address = new RemoteAddress("192.168.1.1", 443);
        address.ToString().ShouldBe("192.168.1.1:443");
    }

    [Fact]
    public void Parse_Should_Return_RemoteAddress_When_Valid()
    {
        var result = RemoteAddress.Parse("10.0.0.5:5000");
        result.Address.ShouldBe("10.0.0.5");
        result.Port.ShouldBe(5000);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("192.168.1.1")]
    [InlineData("localhost:abc")]
    [InlineData("host:123:extra")]
    [InlineData(":1234")]
    [InlineData("192.168.1.1:")]
    public void Parse_Should_Throw_FormatException_When_Invalid(string input)
    {
        var exception = Should.Throw<FormatException>(() => RemoteAddress.Parse(input));
        exception.Message.ShouldBe($"Invalid remote address format: {input}");
    }
}
