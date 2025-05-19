namespace Grapevine.Abstractions;

/// <summary>
/// Represents an asynchronous callback that is invoked when data is read from a stream.
/// </summary>
/// <param name="buffer">The buffer containing the bytes that were read.</param>
/// <param name="bytesRead">The number of bytes that were read into the buffer.</param>
/// <param name="isEndOfStream">Indicates whether the end of the stream has been reached.</param>
/// <param name="cancellationToken"></param>
/// <returns>A task that completes when the observation logic has finished executing.</returns>
public delegate Task ReadObserverAsync(byte[] buffer, int bytesRead, bool isEndOfStream, CancellationToken? cancellationToken = null);

/// <summary>
/// Represents an asynchronous callback that is invoked when data is written to a stream.
/// </summary>
/// <param name="buffer">The buffer containing the bytes that were written.</param>
/// <param name="bytesWritten">The number of bytes that were written from the buffer.</param>
/// <param name="isFinalChunk">Indicates whether this is the final chunk of data being written.</param>
/// <param name="cancellationToken"></param>
/// <returns>A task that completes when the observation logic has finished executing.</returns>
public delegate Task WriteObserverAsync(byte[] buffer, int bytesWritten, bool isFinalChunk, CancellationToken? cancellationToken = null);

/// <summary>
/// Provides a contract for registering and unregistering asynchronous callbacks
/// to observe data being read from or written to a stream.
/// </summary>
public interface IObservableStream
{
    /// <summary>
    /// Registers an asynchronous callback to be invoked each time data is read from the underlying stream.
    /// </summary>
    /// <param name="observer">The delegate to invoke after each read operation.</param>
    void RegisterReadObserver(ReadObserverAsync observer);

    /// <summary>
    /// Registers an asynchronous callback to be invoked each time data is written to the underlying stream.
    /// </summary>
    /// <param name="observer">The delegate to invoke after each write operation.</param>
    void RegisterWriteObserver(WriteObserverAsync observer);

    /// <summary>
    /// Unregisters a previously registered read observer.
    /// </summary>
    /// <param name="observer">The delegate to remove from the read observer list.</param>
    void UnregisterReadObserver(ReadObserverAsync observer);

    /// <summary>
    /// Unregisters a previously registered write observer.
    /// </summary>
    /// <param name="observer">The delegate to remove from the write observer list.</param>
    void UnregisterWriteObserver(WriteObserverAsync observer);
}
