namespace Grapevine.Abstractions;

/// <summary>
/// Enum representing standard HTTP request headers.
/// </summary>
public enum RequestHeader
{
    /// <summary>
    /// Media types acceptable for the response.
    /// </summary>
    [ToString("Accept")]
    Accept,

    /// <summary>
    /// Character sets acceptable for the response.
    /// </summary>
    [ToString("Accept-Charset")]
    [Obsolete("The Accept-Charset header is deprecated. Modern browsers ignore it.")]
    AcceptCharset,

    /// <summary>
    /// Content-encoding algorithms acceptable for the response.
    /// </summary>
    [ToString("Accept-Encoding")]
    AcceptEncoding,

    /// <summary>
    /// Languages acceptable for the response.
    /// </summary>
    [ToString("Accept-Language")]
    AcceptLanguage,

    /// <summary>
    /// Authentication credentials for the request.
    /// </summary>
    [ToString("Authorization")]
    Authorization,

    /// <summary>
    /// Directives for caching mechanisms.
    /// </summary>
    [ToString("Cache-Control")]
    CacheControl,

    /// <summary>
    /// Controls whether the network connection should remain open.
    /// </summary>
    [ToString("Connection")]
    Connection,

    /// <summary>
    /// The encoding of the request body.
    /// </summary>
    [ToString("Content-Encoding")]
    ContentEncoding,

    /// <summary>
    /// The size of the request body in bytes.
    /// </summary>
    [ToString("Content-Length")]
    ContentLength,

    /// <summary>
    /// The media type of the request body.
    /// </summary>
    [ToString("Content-Type")]
    ContentType,

    /// <summary>
    /// Cookies sent with the request.
    /// </summary>
    [ToString("Cookie")]
    Cookie,

    /// <summary>
    /// The date and time the request was originated.
    /// </summary>
    [ToString("Date")]
    [Obsolete("The Date header is rarely used in requests and may be ignored.")]
    Date,

    /// <summary>
    /// Indicates the client expects specific behaviors from the server.
    /// </summary>
    [ToString("Expect")]
    Expect,

    /// <summary>
    /// Disclose original client IP and other proxies in the chain.
    /// </summary>
    [ToString("Forwarded")]
    Forwarded,

    /// <summary>
    /// Email address of the user making the request (deprecated).
    /// </summary>
    [ToString("From")]
    [Obsolete("The From header is deprecated for privacy reasons.")]
    From,

    /// <summary>
    /// The domain name or IP of the server being requested.
    /// </summary>
    [ToString("Host")]
    Host,

    /// <summary>
    /// Conditionally matches resources based on an ETag.
    /// </summary>
    [ToString("If-Match")]
    IfMatch,

    /// <summary>
    /// Only perform the request if the resource was modified since a specific date.
    /// </summary>
    [ToString("If-Modified-Since")]
    IfModifiedSince,

    /// <summary>
    /// Conditionally matches resources if the ETag does not match.
    /// </summary>
    [ToString("If-None-Match")]
    IfNoneMatch,

    /// <summary>
    /// Request a range of the resource if unchanged since a specific date.
    /// </summary>
    [ToString("If-Range")]
    IfRange,

    /// <summary>
    /// Only perform the request if the resource is unchanged since a specific date.
    /// </summary>
    [ToString("If-Unmodified-Since")]
    IfUnmodifiedSince,

    /// <summary>
    /// Indicates the origin of a cross-site request.
    /// </summary>
    [ToString("Origin")]
    Origin,

    /// <summary>
    /// Implementation-specific directives for backward compatibility.
    /// </summary>
    [ToString("Pragma")]
    [Obsolete("The Pragma header is obsolete; use Cache-Control instead.")]
    Pragma,

    /// <summary>
    /// Authentication credentials for a proxy server.
    /// </summary>
    [ToString("Proxy-Authorization")]
    ProxyAuthorization,

    /// <summary>
    /// Request only a specific range of the resource.
    /// </summary>
    [ToString("Range")]
    Range,

    /// <summary>
    /// The address of the previous web page linked to the resource.
    /// </summary>
    [ToString("Referer")]
    Referer,

    /// <summary>
    /// Indicates acceptable transfer encodings for the response.
    /// </summary>
    [ToString("TE")]
    TE,

    /// <summary>
    /// Headers to be sent after the message body (chunked transfer encoding).
    /// </summary>
    [ToString("Trailer")]
    Trailer,

    /// <summary>
    /// The transfer encodings applied to the message body.
    /// </summary>
    [ToString("Transfer-Encoding")]
    TransferEncoding,

    /// <summary>
    /// Indicates a protocol the client wants to switch to.
    /// </summary>
    [ToString("Upgrade")]
    Upgrade,

    /// <summary>
    /// Identifies the client software initiating the request.
    /// </summary>
    [ToString("User-Agent")]
    UserAgent,

    /// <summary>
    /// Indicates intermediate proxies handling the request.
    /// </summary>
    [ToString("Via")]
    Via,

    /// <summary>
    /// Additional information about the status or transformation of the request.
    /// </summary>
    [ToString("Warning")]
    [Obsolete("The Warning header is deprecated in HTTP/2 and later.")]
    Warning
}
