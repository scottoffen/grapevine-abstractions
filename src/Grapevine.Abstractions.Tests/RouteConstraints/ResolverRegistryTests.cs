using Grapevine.Abstractions.RouteConstraints;

namespace Grapevine.Abstractions.Tests.RouteConstraints;

public class ResolverRegistryTests
{
    [Theory]
    [InlineData("{id}", @"(?<id>[^/]+)")]
    [InlineData("{id:int}", @"(?<id>-?\d+)")]
    [InlineData("{id:int(3)}", @"(?<id>-?\d{3})")]
    [InlineData("{val:alpha(2)}", @"(?<val>[a-zA-Z]{2})")]
    [InlineData("{when:date(basic)}", @"(?<when>\d{8})")]
    [InlineData("{v:double(1,3)}", @"(?<v>[-]?\d+(?:\.\d{1,3})?(?:[eE][-+]?\d+)?)")]
    public void TryResolveSegment_ShouldReturnExpectedPattern_WhenFormatIsValid(string segment, string expected)
    {
        var success = ResolverRegistry.TryResolveSegment(segment, out var pattern);
        success.ShouldBeTrue();
        pattern.ShouldBe(expected);
    }

    [Theory]
    [InlineData("id:int")]     // no braces
    [InlineData("{:int}")]     // missing name
    [InlineData("{id:int(3}")] // unbalanced
    public void TryResolveSegment_ShouldReturnFalse_WhenFormatIsInvalid(string segment)
    {
        var success = ResolverRegistry.TryResolveSegment(segment, out var pattern);
        success.ShouldBeFalse();
        pattern.ShouldBeNull();
    }

    [Fact]
    public void TryResolveSegment_ShouldThrow_WhenConstraintIsUnknown()
    {
        var ex = Should.Throw<ArgumentException>(() =>
        {
            ResolverRegistry.TryResolveSegment("{id:unknown}", out _);
        });

        ex.Message.ShouldContain("No resolver registered for constraint 'unknown'");
        ex.Message.ShouldContain("Known constraints");
    }

    [Fact]
    public void RegisterResolver_ShouldAddNewResolver_WhenKeyIsUnique()
    {
        var key = "custom-key";
        var resolver = new RouteConstraintResolver((name, arg) => $"(?<{name}>custom)");
        ResolverRegistry.RegisterResolver(key, resolver);

        var success = ResolverRegistry.TryResolveSegment("{x:custom-key}", out var pattern);
        success.ShouldBeTrue();
        pattern.ShouldBe("(?<x>custom)");
    }

    [Fact]
    public void RegisterResolver_ShouldThrow_WhenKeyIsAlreadyRegistered()
    {
        var ex = Should.Throw<InvalidOperationException>(() =>
        {
            ResolverRegistry.RegisterResolver("int", (name, arg) => $"(?<{name}>override)");
        });

        ex.Message.ShouldContain("already registered");
    }

    [Fact]
    public void OverrideResolver_ShouldReplaceOrAddResolver()
    {
        var key = "custom-override";
        var first = new RouteConstraintResolver((name, arg) => $"(?<{name}>first)");
        var second = new RouteConstraintResolver((name, arg) => $"(?<{name}>second)");

        ResolverRegistry.OverrideResolver(key, first);
        ResolverRegistry.TryResolveSegment("{val:custom-override}", out var pattern1);
        pattern1.ShouldBe("(?<val>first)");

        ResolverRegistry.OverrideResolver(key, second);
        ResolverRegistry.TryResolveSegment("{val:custom-override}", out var pattern2);
        pattern2.ShouldBe("(?<val>second)");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void RegisterResolver_ShouldThrow_WhenKeyIsNullOrEmpty(string? key)
    {
        var resolver = new RouteConstraintResolver((name, arg) => $"(?<{name}>x)");
        var ex = Should.Throw<ArgumentException>(() => ResolverRegistry.RegisterResolver(key!, resolver));
        ex.ParamName.ShouldBe("key");
    }

    [Fact]
    public void RegisterResolver_ShouldThrow_WhenResolverIsNull()
    {
        var ex = Should.Throw<ArgumentNullException>(() => ResolverRegistry.RegisterResolver("test-null", null!));
        ex.ParamName.ShouldBe("resolver");
    }

    [Fact]
    public void OverrideResolver_ShouldThrow_WhenKeyIsNullOrWhitespace()
    {
        var resolver = new RouteConstraintResolver((name, arg) => $"(?<{name}>x)");
        var ex = Should.Throw<ArgumentException>(() => ResolverRegistry.OverrideResolver(" ", resolver));
        ex.ParamName.ShouldBe("key");
    }

    [Fact]
    public void OverrideResolver_ShouldThrow_WhenResolverIsNull()
    {
        var ex = Should.Throw<ArgumentNullException>(() => ResolverRegistry.OverrideResolver("some-key", null!));
        ex.ParamName.ShouldBe("resolver");
    }
}
