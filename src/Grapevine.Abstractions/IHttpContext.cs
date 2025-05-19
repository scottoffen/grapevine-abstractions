namespace Grapevine.Abstractions;

public interface IHttpContext
{
    /// <summary>
    /// Gets the CancellationToken object that will be used to signal the cancellation of the request
    /// </summary>
    CancellationToken CancellationToken { get; }

    /// <summary>
    /// Gets the unique identifier for this request
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets a key/value collection for storing per-request contextual data that can be shared across middleware and handlers.
    /// </summary>
    Locals Locals { get; }

    /// <summary>
    /// Gets the IHttpRequest that represents a client's request for a resource
    /// </summary>
    IHttpRequest Request { get; }

    /// <summary>
    /// Gets the IHttpResponse object that will be sent to the client in response to the client's request
    /// </summary>
    IHttpResponse Response { get; }

    /// <summary>
    /// Gets or sets the IServiceProvider object for dependency injection
    /// </summary>
    IServiceProvider Services { get; set; }
}
