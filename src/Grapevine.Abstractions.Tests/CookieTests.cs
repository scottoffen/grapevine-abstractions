namespace Grapevine.Abstractions.Tests;

public class CookieTests
{
    [Fact]
    public void Constructor_ShouldSetNameAndValue()
    {
        var cookie = new Cookie("session", "abc123");
        cookie.Name.ShouldBe("session");
        cookie.Value.ShouldBe("abc123");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Name_SetInvalidValue_ShouldThrow(string? invalidName)
    {
        var cookie = new Cookie("valid", "value");
        Should.Throw<ArgumentException>(() => cookie.Name = invalidName!);
    }

    [Theory]
    [InlineData("user name")]
    [InlineData("user;id")]
    [InlineData("user@id")]
    public void Name_SetInvalidCharacters_ShouldThrow(string invalidName)
    {
        var cookie = new Cookie("valid", "value");
        Should.Throw<ArgumentException>(() => cookie.Name = invalidName);
    }

    [Fact]
    public void Value_SetNull_ShouldThrow()
    {
        var cookie = new Cookie("session", "abc123");
        Should.Throw<ArgumentNullException>(() => cookie.Value = null!);
    }

    [Fact]
    public void Value_SetWithSemicolon_ShouldThrow()
    {
        var cookie = new Cookie("session", "abc123");
        Should.Throw<ArgumentException>(() => cookie.Value = "abc;123");
    }

    [Theory]
    [InlineData("example.com")]
    [InlineData("sub.example.com")]
    [InlineData("localhost")]
    public void Domain_SetValidDomain_ShouldSucceed(string validDomain)
    {
        var cookie = new Cookie("session", "abc123") { Domain = validDomain };
        cookie.Domain.ShouldBe(validDomain);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("not a domain")]
    public void Domain_SetInvalidDomain_ShouldThrow(string? invalidDomain)
    {
        var cookie = new Cookie("session", "abc123");
        Should.Throw<ArgumentException>(() => cookie.Domain = invalidDomain!);
    }

    [Fact]
    public void Path_SetValidPath_ShouldSucceed()
    {
        var cookie = new Cookie("session", "abc123") { Path = "/account" };
        cookie.Path.ShouldBe("/account");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("account")]
    public void Path_SetInvalid_ShouldThrow(string? invalidPath)
    {
        var cookie = new Cookie("session", "abc123");
        Should.Throw<ArgumentException>(() => cookie.Path = invalidPath!);
    }

    [Fact]
    public void Expires_SetUtcValue_ShouldSucceed()
    {
        var utc = DateTime.UtcNow.AddDays(1);
        var cookie = new Cookie("session", "abc123") { Expires = utc };
        cookie.Expires.ShouldBe(utc);
    }

    [Fact]
    public void Expires_SetNonUtcValue_ShouldThrow()
    {
        var nonUtc = DateTime.Now.AddDays(1);
        var cookie = new Cookie("session", "abc123");
        Should.Throw<ArgumentException>(() => cookie.Expires = nonUtc);
    }

    [Fact]
    public void ToString_ShouldOutputBasicFormat()
    {
        var cookie = new Cookie("token", "abc123");
        cookie.ToString().ShouldBe("token=abc123; Path=/");
    }

    [Fact]
    public void ToString_ShouldIncludeAllFields()
    {
        var cookie = new Cookie("token", "abc123")
        {
            Domain = "example.com",
            Path = "/secure",
            Expires = new DateTime(2030, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.Strict
        };

        var result = cookie.ToString();

        result.ShouldContain("token=abc123");
        result.ShouldContain("Domain=example.com");
        result.ShouldContain("Path=/secure");
        result.ShouldContain("Expires=Tue, 01 Jan 2030");
        result.ShouldContain("Secure");
        result.ShouldContain("HttpOnly");
        result.ShouldContain("SameSite=Strict");
    }
}
