namespace Grapevine.Abstractions;

/// <summary>
/// A stream wrapper that allows asynchronous observation of read and write operations.
/// </summary>
public class ObservableStream : Stream, IObservableStream
{
    private readonly Stream _inner;
    private readonly List<ReadObserverAsync> _readObservers = new();
    private readonly List<WriteObserverAsync> _writeObservers = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableStream"/> class.
    /// </summary>
    /// <param name="inner">The underlying stream to wrap.</param>
    public ObservableStream(Stream inner)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }

    /// <summary>
    /// Gets the underlying stream that this observable stream wraps.
    /// </summary>
    public Stream BaseStream => _inner;

    /// <inheritdoc />
    public override bool CanRead => _inner.CanRead;

    /// <inheritdoc />
    public override bool CanSeek => _inner.CanSeek;

    /// <inheritdoc />
    public override bool CanWrite => _inner.CanWrite;

    /// <inheritdoc />
    public override long Length => _inner.Length;

    /// <inheritdoc />
    public override long Position
    {
        get => _inner.Position;
        set => _inner.Position = value;
    }

    /// <inheritdoc />
    public override void Flush() => _inner.Flush();

    /// <inheritdoc />
    public override Task FlushAsync(CancellationToken cancellationToken) => _inner.FlushAsync(cancellationToken);

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count)
    {
        var bytesRead = _inner.Read(buffer, offset, count);
        if (bytesRead > 0)
        {
            NotifyReadObserversAsync(buffer, bytesRead, bytesRead < count, CancellationToken.None).GetAwaiter().GetResult();
        }
        return bytesRead;
    }

    /// <inheritdoc />
    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        var bytesRead = await _inner.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
        if (bytesRead > 0)
        {
            await NotifyReadObserversAsync(buffer, bytesRead, bytesRead < count, cancellationToken).ConfigureAwait(false);
        }
        return bytesRead;
    }

    /// <summary>
    /// Registers an asynchronous observer that is called when data is read from the stream.
    /// </summary>
    /// <param name="observer">The observer to register.</param>
    public void RegisterReadObserver(ReadObserverAsync observer)
    {
        if (observer is not null) _readObservers.Add(observer);
    }

    /// <summary>
    /// Registers an asynchronous observer that is called when data is written to the stream.
    /// </summary>
    /// <param name="observer">The observer to register.</param>
    public void RegisterWriteObserver(WriteObserverAsync observer)
    {
        if (observer is not null) _writeObservers.Add(observer);
    }

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin) => _inner.Seek(offset, origin);

    /// <inheritdoc />
    public override void SetLength(long value) => _inner.SetLength(value);

    /// <inheritdoc />
    public override void Write(byte[] buffer, int offset, int count)
    {
        _inner.Write(buffer, offset, count);
        NotifyWriteObserversAsync(buffer, count, false, CancellationToken.None).GetAwaiter().GetResult();
    }

    /// <inheritdoc />
    public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        await _inner.WriteAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
        await NotifyWriteObserversAsync(buffer, count, false, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Unregisters a previously registered read observer.
    /// </summary>
    /// <param name="observer">The observer to unregister.</param>
    public void UnregisterReadObserver(ReadObserverAsync observer)
    {
        _readObservers.Remove(observer);
    }

    /// <summary>
    /// Unregisters a previously registered write observer.
    /// </summary>
    /// <param name="observer">The observer to unregister.</param>
    public void UnregisterWriteObserver(WriteObserverAsync observer)
    {
        _writeObservers.Remove(observer);
    }

    private Task NotifyReadObserversAsync(byte[] buffer, int count, bool isEndOfStream, CancellationToken token)
    {
        var tasks = new List<Task>(_readObservers.Count);
        foreach (var observer in _readObservers)
        {
            tasks.Add(observer(buffer, count, isEndOfStream, token));
        }
        return Task.WhenAll(tasks);
    }

    private Task NotifyWriteObserversAsync(byte[] buffer, int count, bool isFinalChunk, CancellationToken token)
    {
        var tasks = new List<Task>(_writeObservers.Count);
        foreach (var observer in _writeObservers)
        {
            tasks.Add(observer(buffer, count, isFinalChunk, token));
        }
        return Task.WhenAll(tasks);
    }
}
