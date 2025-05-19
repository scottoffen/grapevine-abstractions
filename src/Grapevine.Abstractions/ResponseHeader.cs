namespace Grapevine.Abstractions;

/// <summary>
/// Enum representing standard HTTP response headers.
/// </summary>
public enum ResponseHeader
{
    /// <summary>
    /// Specifies the range units supported by the server.
    /// </summary>
    [ToString("Accept-Ranges")]
    AcceptRanges,

    /// <summary>
    /// Indicates the age of the response, in seconds, since it was generated.
    /// </summary>
    [ToString("Age")]
    Age,

    /// <summary>
    /// Lists the allowed HTTP methods for the requested resource.
    /// </summary>
    [ToString("Allow")]
    Allow,

    /// <summary>
    /// Specifies alternative services that can be used to access the resource.
    /// </summary>
    [ToString("Alt-Svc")]
    AltSvc,

    /// <summary>
    /// Directs caching behavior for the response, including expiration and revalidation.
    /// </summary>
    [ToString("Cache-Control")]
    CacheControl,

    /// <summary>
    /// Indicates whether the network connection should be kept open after the response.
    /// </summary>
    [ToString("Connection")]
    Connection,

    /// <summary>
    /// Provides directives for how the content should be displayed (e.g., attachment or inline).
    /// </summary>
    [ToString("Content-Disposition")]
    ContentDisposition,

    /// <summary>
    /// Specifies the encoding transformation applied to the response body (e.g., gzip).
    /// </summary>
    [ToString("Content-Encoding")]
    ContentEncoding,

    /// <summary>
    /// Specifies the natural language(s) of the response content.
    /// </summary>
    [ToString("Content-Language")]
    ContentLanguage,

    /// <summary>
    /// Specifies the size of the response body in bytes.
    /// </summary>
    [ToString("Content-Length")]
    ContentLength,

    /// <summary>
    /// Indicates a URL where the response content can be found.
    /// </summary>
    [ToString("Content-Location")]
    ContentLocation,

    /// <summary>
    /// Specifies the range of bytes being returned for a partial content request.
    /// </summary>
    [ToString("Content-Range")]
    ContentRange,

    /// <summary>
    /// Specifies the media type of the response body (e.g., text/html, application/json).
    /// </summary>
    [ToString("Content-Type")]
    ContentType,

    /// <summary>
    /// Specifies the date and time when the response was generated.
    /// </summary>
    [ToString("Date")]
    Date,

    /// <summary>
    /// Provides a unique identifier for the version of the resource, used for caching.
    /// </summary>
    [ToString("ETag")]
    ETag,

    /// <summary>
    /// Specifies the date and time after which the response is considered stale.
    /// </summary>
    [ToString("Expires")]
    Expires,

    /// <summary>
    /// Contains configuration settings for the HTTP/2 connection.
    /// </summary>
    [ToString("HTTP2-Settings")]
    HTTP2Settings,

    /// <summary>
    /// Indicates the date and time when the resource was last modified.
    /// </summary>
    [ToString("Last-Modified")]
    LastModified,

    /// <summary>
    /// Specifies relationships between the current response and other resources.
    /// </summary>
    [ToString("Link")]
    Link,

    /// <summary>
    /// Used in redirection responses to indicate the URL to redirect to.
    /// </summary>
    [ToString("Location")]
    Location,

    /// <summary>
    /// Includes implementation-specific directives (e.g., for caching).
    /// </summary>
    [ToString("Pragma")]
    [Obsolete("The Pragma header is deprecated in HTTP/1.1. Use Cache-Control instead.")]
    Pragma,

    /// <summary>
    /// Indicates the authentication method required by a proxy server.
    /// </summary>
    [ToString("Proxy-Authenticate")]
    ProxyAuthenticate,

    /// <summary>
    /// Specifies how long to wait before making another request (e.g., after a rate limit is exceeded).
    /// </summary>
    [ToString("Retry-After")]
    RetryAfter,

    /// <summary>
    /// Identifies the software used by the origin server to handle the request.
    /// </summary>
    [ToString("Server")]
    Server,

    /// <summary>
    /// Sends cookies from the server to the client for storage and later use.
    /// </summary>
    [ToString("Set-Cookie")]
    SetCookie,

    /// <summary>
    /// Instructs browsers to always access the site using HTTPS.
    /// </summary>
    [ToString("Strict-Transport-Security")]
    StrictTransportSecurity,

    /// <summary>
    /// Specifies the transfer mechanism used for the response body (e.g., chunked).
    /// </summary>
    [ToString("Transfer-Encoding")]
    TransferEncoding,

    /// <summary>
    /// Requests the client to upgrade to a different protocol (e.g., HTTP/2).
    /// </summary>
    [ToString("Upgrade")]
    Upgrade,

    /// <summary>
    /// Specifies which request headers the server used to determine the response.
    /// </summary>
    [ToString("Vary")]
    Vary,

    /// <summary>
    /// Records intermediate proxies or gateways that have handled the request.
    /// </summary>
    [ToString("Via")]
    Via,

    /// <summary>
    /// Specifies the authentication method required to access the resource.
    /// </summary>
    [ToString("WWW-Authenticate")]
    WWWAuthenticate
}
