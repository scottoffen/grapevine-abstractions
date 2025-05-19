using System.Text;

namespace Grapevine.Abstractions.Tests;

public class ObservableStreamTests
{
    private static ObservableStream CreateTestStream(out MemoryStream inner)
    {
        inner = new MemoryStream();
        return new ObservableStream(inner);
    }

    [Fact]
    public async Task ReadAsync_InvokesObserver_WithCancellation()
    {
        var stream = CreateTestStream(out var inner);
        var invoked = false;

        ReadObserverAsync observer = (_, __, ___, _) =>
        {
            invoked = true;
            return Task.CompletedTask;
        };

        stream.RegisterReadObserver(observer);

        var buffer = Encoding.UTF8.GetBytes("test");
        await inner.WriteAsync(buffer, CancellationToken.None);
        await inner.FlushAsync();
        stream.Position = 0;

        var readBuffer = new byte[buffer.Length];
        var bytesRead = await stream.ReadAsync(readBuffer, CancellationToken.None);

        bytesRead.ShouldBe(buffer.Length);
        invoked.ShouldBeTrue();
    }

    [Fact]
    public async Task UnregisterReadObserver_RemovesObserver_WithCancellation()
    {
        var stream = CreateTestStream(out var inner);
        var invoked = false;

        ReadObserverAsync observer = (_, __, ___, _) =>
        {
            invoked = true;
            return Task.CompletedTask;
        };

        stream.RegisterReadObserver(observer);
        stream.UnregisterReadObserver(observer);

        var buffer = Encoding.UTF8.GetBytes("test");
        await inner.WriteAsync(buffer, CancellationToken.None);
        await inner.FlushAsync();
        stream.Position = 0;

        var readBuffer = new byte[buffer.Length];
        var bytesRead = await stream.ReadAsync(readBuffer, CancellationToken.None);

        bytesRead.ShouldBe(buffer.Length);
        invoked.ShouldBeFalse();
    }

    [Fact]
    public async Task WriteAsync_InvokesObserver_WithCancellation()
    {
        var stream = CreateTestStream(out _);
        var invoked = false;

        WriteObserverAsync observer = (_, __, ___, _) =>
        {
            invoked = true;
            return Task.CompletedTask;
        };

        stream.RegisterWriteObserver(observer);

        var buffer = Encoding.UTF8.GetBytes("test");
        await stream.WriteAsync(buffer, CancellationToken.None);

        invoked.ShouldBeTrue();
    }

    [Fact]
    public async Task UnregisterWriteObserver_RemovesObserver_WithCancellation()
    {
        var stream = CreateTestStream(out _);
        var invoked = false;

        WriteObserverAsync observer = (_, __, ___, _) =>
        {
            invoked = true;
            return Task.CompletedTask;
        };

        stream.RegisterWriteObserver(observer);
        stream.UnregisterWriteObserver(observer);

        var buffer = Encoding.UTF8.GetBytes("test");
        await stream.WriteAsync(buffer, CancellationToken.None);

        invoked.ShouldBeFalse();
    }
}
