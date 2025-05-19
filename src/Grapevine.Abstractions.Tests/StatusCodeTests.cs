namespace Grapevine.Abstractions.Tests;

public class StatusCodeTests
{
    [Fact]
    public void PredefinedStatusCode_ShouldHaveExpectedValueAndDescription()
    {
        StatusCode.Ok.Value.ShouldBe(200);
        StatusCode.Ok.Description.ShouldBe("Ok");

        StatusCode.NotFound.Value.ShouldBe(404);
        StatusCode.NotFound.Description.ShouldBe("Not Found");
    }

    [Fact]
    public void ToString_ShouldReturnExpectedFormat()
    {
        StatusCode.BadRequest.ToString().ShouldBe("400 Bad Request");
        var code = new StatusCode(499);
        code.ToString().ShouldBe("499");
    }

    [Fact]
    public void Equals_ShouldReturnTrue_ForSameValue()
    {
        var a = new StatusCode(250, "Custom");
        var b = new StatusCode(250, "Something else");
        a.Equals(b).ShouldBeTrue();
        (a == b).ShouldBeTrue();
        (a != b).ShouldBeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_ForDifferentValues()
    {
        var a = new StatusCode(201, "Created");
        var b = new StatusCode(202, "Accepted");
        a.Equals(b).ShouldBeFalse();
    }

    [Fact]
    public void GetHashCode_ShouldBeSame_ForSameValue()
    {
        var a = new StatusCode(250, "A");
        var b = new StatusCode(250, "B");
        a.GetHashCode().ShouldBe(b.GetHashCode());
    }

    [Fact]
    public void ImplicitConversion_FromStatusCodeToInt_ShouldReturnValue()
    {
        int code = StatusCode.Ok;
        code.ShouldBe(200);
    }

    [Fact]
    public void ImplicitConversion_FromIntToStatusCode_ShouldReturnKnownInstance()
    {
        StatusCode code = 200;
        code.ShouldBe(StatusCode.Ok);
    }

    [Fact]
    public void ImplicitConversion_FromIntToUnknownStatusCode_ShouldThrow_WhenOutOfRange()
    {
        var ex = Should.Throw<ArgumentOutOfRangeException>(() =>
        {
            StatusCode _ = 999;
        });

        ex.ParamName.ShouldBe("value");
    }

    [Fact]
    public void TryAdd_ShouldAddCustomStatusCode_WhenNotAlreadyPresent()
    {
        var success = StatusCode.TryAddValue(599, "Custom Code", out var custom);
        success.ShouldBeTrue();
        custom!.Value.ShouldBe(599);
        custom.Description.ShouldBe("Custom Code");
    }

    [Fact]
    public void TryAdd_ShouldFail_WhenCodeAlreadyExists()
    {
        StatusCode.TryAddValue(598, "First", out _).ShouldBeTrue();
        var result = StatusCode.TryAddValue(598, "Second", out var duplicate);
        result.ShouldBeFalse();
        duplicate!.Description.ShouldBe("First"); // Should return the first one
    }

    [Fact]
    public void TryGetValue_ShouldReturnTrue_WhenCodeExists()
    {
        StatusCode.TryAddValue(597, "Something", out _);
        StatusCode.TryGetValue(597, out var value).ShouldBeTrue();
        value!.Description.ShouldBe("Something");
    }

    [Fact]
    public void TryGetValue_ShouldReturnFalse_WhenCodeDoesNotExist()
    {
        StatusCode.TryGetValue(596, out var value).ShouldBeFalse();
        value.ShouldBeNull();
    }

    [Fact]
    public void CreatingStatusCode_WithInvalidValue_ShouldThrow()
    {
        Should.Throw<ArgumentOutOfRangeException>(() => new StatusCode(99));
        Should.Throw<ArgumentOutOfRangeException>(() => new StatusCode(600));
    }
}
