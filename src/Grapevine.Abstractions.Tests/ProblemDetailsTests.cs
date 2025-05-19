namespace Grapevine.Abstractions.Tests;

public class ProblemDetailsTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeWithDefaults()
    {
        var pd = new ProblemDetails();

        pd.Type.ShouldBe("about:blank");
        pd.Title.ShouldBe(string.Empty);
        pd.Status.ShouldBe(0);
        pd.Detail.ShouldBe(string.Empty);
        pd.Instance.ShouldBe(string.Empty);
        pd.Extensions.ShouldNotBeNull();
        pd.Extensions.ShouldBeEmpty();
    }

    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        var pd = new ProblemDetails(404, "Not Found", "The requested resource was not found.", "/errors/404", "/foo/bar");

        pd.Status.ShouldBe(404);
        pd.Title.ShouldBe("Not Found");
        pd.Detail.ShouldBe("The requested resource was not found.");
        pd.Type.ShouldBe("/errors/404");
        pd.Instance.ShouldBe("/foo/bar");
    }

    [Fact]
    public void ToString_ShouldReturnFormattedMessage()
    {
        var pd = new ProblemDetails(500, "Server Error", "Unexpected failure occurred.");
        pd.ToString().ShouldBe("Server Error (500): Unexpected failure occurred.");
    }

    [Fact]
    public void ToJson_ShouldIncludeStandardProperties()
    {
        var pd = new ProblemDetails(400, "Bad Request", "Invalid input.", "/errors/400", "/api/submit");

        var json = pd.ToJson();

        json.ShouldContain("\"status\":400");
        json.ShouldContain("\"title\":\"Bad Request\"");
        json.ShouldContain("\"detail\":\"Invalid input.\"");
        json.ShouldContain("\"type\":\"/errors/400\"");
        json.ShouldContain("\"instance\":\"/api/submit\"");
    }

    [Fact]
    public void ToJson_ShouldIncludeExtensions()
    {
        var pd = new ProblemDetails(403, "Forbidden");
        pd.Extensions.Add("correlationId", "abc-123");
        pd.Extensions.Add("timestamp", new DateTime(2024, 01, 01, 12, 0, 0, DateTimeKind.Utc));

        var json = pd.ToJson();

        json.ShouldContain("\"correlationId\":\"abc-123\"");
        json.ShouldContain("\"timestamp\":\"2024-01-01T12:00:00.0000000Z\"");
    }

    [Fact]
    public void ToJson_ShouldEscapeStringsProperly()
    {
        var pd = new ProblemDetails(418, "I'm a teapot", "Line\nbreak\t\"quote\"");
        var json = pd.ToJson();

        json.ShouldContain("\"detail\":\"Line\\nbreak\\t\\\"quote\\\"\"");
    }

    [Fact]
    public void Extensions_ShouldRejectReservedKeys()
    {
        var pd = new ProblemDetails();

        Should.Throw<ArgumentException>(() => pd.Extensions.Add("title", "duplicate"))
            .Message.ShouldContain("conflicts with a reserved field name");
    }
}
