namespace Grapevine.Abstractions.Tests;

public class ConnectionInfoTests
{
    [Theory]
    [InlineData("127.0.0.1", 80)]
    [InlineData("::1", 8080)]
    [InlineData("[::1]", 443)]
    [InlineData("localhost", 1234)]
    [InlineData("example.com", 65535)]
    [InlineData("127.0.0.1", 0)]
    public void Constructor_WithValidInputs_CreatesInstance(string address, int port)
    {
        var connectionInfo = new ConnectionInfo(address, port);
        connectionInfo.Address.ShouldBe(address);
        connectionInfo.Port.ShouldBe(port);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithNullOrWhitespaceAddress_Throws(string? address)
    {
        var exception = Should.Throw<ArgumentException>(() => new ConnectionInfo(address!, 80));
        exception.ParamName.ShouldBe("address");
    }

    [Theory]
    [InlineData("bad!address")]
    [InlineData("some@host")]
    [InlineData("name#with$")]
    public void Constructor_WithInvalidCharactersInAddress_Throws(string address)
    {
        var exception = Should.Throw<ArgumentException>(() => new ConnectionInfo(address, 80));
        exception.ParamName.ShouldBe("address");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(65536)]
    public void Constructor_WithOutOfRangePort_Throws(int port)
    {
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => new ConnectionInfo("127.0.0.1", port));
        exception.ParamName.ShouldBe("port");
    }

    [Fact]
    public void ToString_ShouldUseInvariantCulture()
    {
        var connectionInfo = new ConnectionInfo("127.0.0.1", 8080);

        var originalCulture = System.Globalization.CultureInfo.CurrentCulture;
        try
        {
            System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("fa-IR");

            connectionInfo.ToString().ShouldBe("127.0.0.1:8080");
        }
        finally
        {
            System.Globalization.CultureInfo.CurrentCulture = originalCulture;
        }
    }
}
