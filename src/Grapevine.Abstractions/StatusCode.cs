using System.Collections.Concurrent;
using System.Diagnostics;

namespace Grapevine.Abstractions;

/// <summary>
/// Represents an HTTP status code.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public partial class StatusCode : IEquatable<StatusCode>
{
    /// <summary>
    /// Returns the integer value of the StatusCode
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Returns the description of the StatusCode
    /// </summary>
    public string Description { get; }

    public StatusCode(int value, string? description = null)
    {
        if (value < _minValue || value > _maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"Status code must be between {_minValue} and {_maxValue}");
        }

        Value = value;
        Description = description ?? string.Empty;
    }

    /// <summary>
    /// Returns the string representation of the StatusCode
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.IsNullOrWhiteSpace(Description)
            ? Value.ToString()
            : $"{Value} {Description}";
    }

    /// <summary>
    /// Returns true if the StatusCode is equal to the given object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => Equals(obj as StatusCode);

    /// <summary>
    /// Returns true if the StatusCode is equal to the given StatusCode
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(StatusCode? other)
    {
        return other is not null && Value == other.Value;
    }

    /// <summary>
    /// Returns the hash code for the StatusCode
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => Value;
}

public partial class StatusCode
{
    public static bool operator ==(StatusCode? left, StatusCode? right)
    {
        return left?.Equals(right) ?? right is null;
    }

    public static bool operator !=(StatusCode? left, StatusCode? right)
    {
        return !(left == right);
    }

    public static implicit operator int(StatusCode statusCode)
    {
        return statusCode.Value;
    }

    public static implicit operator StatusCode(int value)
    {
        if (TryGetValue(value, out var statusCode))
        {
            return statusCode!;
        }
        else if (TryAddValue(value, null, out var newStatusCode))
        {
            return newStatusCode!;
        }
        else
        {
            throw new InvalidOperationException($"Failed to convert {value} to StatusCode");
        }
    }
}

public partial class StatusCode
{
    private static readonly ConcurrentDictionary<int, StatusCode> _statusCodes = new();

    private static readonly int _maxValue = 599;
    private static readonly int _minValue = 100;

    public static bool TryAddValue(int value, string? description, out StatusCode? statusCode)
    {
        if (TryGetValue(value, out statusCode))
        {
            return false;
        }

        statusCode = new StatusCode(value, description);
        return _statusCodes.TryAdd(value, statusCode);
    }

    private static StatusCode AddInternal(int value, string description)
    {
        if (TryAddValue(value, description, out var statusCode))
        {
            return statusCode!;
        }
        else if (TryGetValue(value, out var existingStatusCode))
        {
            return existingStatusCode!;
        }
        else
        {
            throw new InvalidOperationException($"Failed to add status code {value} with description '{description}'");
        }
    }

    public static bool TryGetValue(int value, out StatusCode? statusCode)
    {
        return _statusCodes.TryGetValue(value, out statusCode);
    }

    #region 1xx Informational

    /// <summary>
    /// The server has received the request headers and the client should proceed to send the request body (in the case of a request for which a body needs to be sent; for example, a POST request). Sending a large request body to a server after a request has been rejected for inappropriate headers would be inefficient. To have a server check the request's headers, a client must send Expect: 100-continue as a header in its initial request and receive a 100 Continue status code in response before sending the body. The response 417 Expectation Failed indicates the request should not be continued.
    /// </summary>
    public static readonly StatusCode Continue = AddInternal(100, "Continue");

    /// <summary>
    /// The requester has asked the server to switch protocols and the server has agreed to do so.
    /// </summary>
    public static readonly StatusCode SwitchingProtocols = AddInternal(101, "Switching Protocols");

    /// <summary>
    /// A WebDAV request may contain many sub-requests involving file operations, requiring a long time to complete the request. This code indicates that the server has received and is processing the request, but no response is available yet. This prevents the client from timing out and assuming the request was lost.
    /// </summary>
    public static readonly StatusCode Processing = AddInternal(102, "Processing");

    #endregion

    #region 2xx Success

    /// <summary>
    /// Standard response for successful HTTP requests. The actual response will depend on the request method used. In a GET request, the response will contain an entity corresponding to the requested resource. In a POST request, the response will contain an entity describing or containing the result of the action.
    /// </summary>
    public static readonly StatusCode Ok = AddInternal(200, "Ok");

    /// <summary>
    /// The request has been fulfilled, resulting in the creation of a new resource.
    /// </summary>
    public static readonly StatusCode Created = AddInternal(201, "Created");

    /// <summary>
    /// The request has been accepted for processing, but the processing has not been completed. The request might or might not be eventually acted upon, and may be disallowed when processing occurs.
    /// </summary>
    public static readonly StatusCode Accepted = AddInternal(202, "Accepted");

    /// <summary>
    /// The server is a transforming proxy (e.g. a Web accelerator) that received a 200 OK from its origin, but is returning a modified version of the origin's response.
    /// </summary>
    public static readonly StatusCode NonAuthoritativeInformation = AddInternal(203, "Non Authoritative Information");

    /// <summary>
    /// The server successfully processed the request and is not returning any content.
    /// </summary>
    public static readonly StatusCode NoContent = AddInternal(204, "No Content");

    /// <summary>
    /// The server successfully processed the request, but is not returning any content. Unlike a 204 response, this response requires that the requester reset the document view.
    /// </summary>
    public static readonly StatusCode ResetContent = AddInternal(205, "Reset Content");

    /// <summary>
    /// The server is delivering only part of the resource (byte serving) due to a range header sent by the client. The range header is used by HTTP clients to enable resuming of interrupted downloads, or split a download into multiple simultaneous streams.
    /// </summary>
    public static readonly StatusCode PartialContent = AddInternal(206, "Partial Content");

    /// <summary>
    /// The message body that follows is an XML message and can contain a number of separate response codes, depending on how many sub-requests were made.
    /// </summary>
    public static readonly StatusCode MultiStatus = AddInternal(207, "Multi Status");

    /// <summary>
    /// The members of a DAV binding have already been enumerated in a previous reply to this request, and are not being included again.
    /// </summary>
    public static readonly StatusCode AlreadyReported = AddInternal(208, "Already Reported");

    /// <summary>
    /// The server has fulfilled a request for the resource, and the response is a representation of the result of one or more instance-manipulations applied to the current instance.
    /// </summary>
    public static readonly StatusCode IMUsed = AddInternal(226, "IMUsed");

    #endregion

    #region 3xx Redirection

    /// <summary>
    /// Indicates multiple options for the resource from which the client may choose. For example, this code could be used to present multiple video format options, to list files with different extensions, or to suggest word sense disambiguation.
    /// </summary>
    public static readonly StatusCode MultipleChoices = AddInternal(300, "Multiple Choices");

    /// <summary>
    /// This and all future requests should be directed to the given URI.
    /// </summary>
    public static readonly StatusCode MovedPermanently = AddInternal(301, "Moved Permanently");

    /// <summary>
    /// This is an example of industry practice contradicting the standard. The HTTP/1.0 specification (RFC 1945) required the client to perform a temporary redirect (the original describing phrase was "Moved Temporarily"), but popular browsers implemented 302 with the functionality of a 303 See Other. Therefore, HTTP/1.1 added status codes 303 and 307 to distinguish between the two behaviors. However, some Web applications and frameworks use the 302 status code as if it were the 303.
    /// </summary>
    public static readonly StatusCode Found = AddInternal(302, "Found");

    /// <summary>
    /// The response to the request can be found under another URI using a GET method.When received in response to a POST (or PUT/DELETE), the client should presume that the server has received the data and should issue a redirect with a separate GET message.
    /// </summary>
    public static readonly StatusCode SeeOther = AddInternal(303, "See Other");

    /// <summary>
    /// Indicates that the resource has not been modified since the version specified by the request headers If-Modified-Since or If-None-Match.In such case, there is no need to retransmit the resource since the client still has a previously-downloaded copy.
    /// </summary>
    public static readonly StatusCode NotModified = AddInternal(304, "Not Modified");

    /// <summary>
    /// The requested resource is available only through a proxy, the address for which is provided in the response.Many HTTP clients (such as Mozilla and Internet Explorer) do not correctly handle responses with this status code, primarily for security reasons.
    /// </summary>
    public static readonly StatusCode UseProxy = AddInternal(305, "Use Proxy");

    /// <summary>
    /// No longer used.Originally meant "Subsequent requests should use the specified proxy."
    /// </summary>
    public static readonly StatusCode SwitchProxy = AddInternal(306, "Switch Proxy");

    /// <summary>
    /// In this case, the request should be repeated with another URI; however, future requests should still use the original URI. In contrast to how 302 was historically implemented, the request method is not allowed to be changed when reissuing the original request. For example, a POST request should be repeated using another POST request.
    /// </summary>
    public static readonly StatusCode TemporaryRedirect = AddInternal(307, "Temporary Redirect");

    /// <summary>
    /// The request and all future requests should be repeated using another URI. 307 and 308 parallel the behaviors of 302 and 301, but do not allow the HTTP method to change.So, for example, submitting a form to a permanently redirected resource may continue smoothly.
    /// </summary>
    public static readonly StatusCode PermanentRedirect = AddInternal(308, "Permanent Redirect");

    #endregion

    #region 4xx Client Error

    /// <summary>
    /// The server cannot or will not process the request due to an apparent client error (e.g., malformed request syntax, invalid request message framing, or deceptive request routing).
    /// </summary>
    public static readonly StatusCode BadRequest = AddInternal(400, "Bad Request");

    /// <summary>
    /// Similar to 403 Forbidden, but specifically for use when authentication is required and has failed or has not yet been provided. The response must include a WWW-Authenticate header field containing a challenge applicable to the requested resource. See Basic access authentication and Digest access authentication. 401 semantically means "unauthenticated", i.e. the user does not have the necessary credentials. Note: Some sites issue HTTP 401 when an IP address is banned from the website (usually the website domain) and that specific address is refused permission to access a website.
    /// </summary>
    public static readonly StatusCode Unauthorized = AddInternal(401, "Unauthorized");

    /// <summary>
    /// Reserved for future use. The original intention was that this code might be used as part of some form of digital cash or micropayment scheme, but that has not happened, and this code is not usually used. Google Developers API uses this status if a particular developer has exceeded the daily limit on requests.
    /// </summary>
    public static readonly StatusCode PaymentRequired = AddInternal(402, "Payment Required");

    /// <summary>
    /// The request was a valid request, but the server is refusing to respond to it. 403 error semantically means "unauthorized", i.e. the user does not have the necessary permissions for the resource.
    /// </summary>
    public static readonly StatusCode Forbidden = AddInternal(403, "Forbidden");

    /// <summary>
    /// The requested resource could not be found but may be available in the future. Subsequent requests by the client are permissible.
    /// </summary>
    public static readonly StatusCode NotFound = AddInternal(404, "Not Found");

    /// <summary>
    /// A request method is not supported for the requested resource; for example, a GET request on a form which requires data to be presented via POST, or a PUT request on a read-only resource.
    /// </summary>
    public static readonly StatusCode MethodNotAllowed = AddInternal(405, "Method Not Allowed");

    /// <summary>
    /// The requested resource is capable of generating only content not acceptable according to the Accept headers sent in the request.
    /// </summary>
    public static readonly StatusCode NotAcceptable = AddInternal(406, "Not Acceptable");

    /// <summary>
    /// The client must first authenticate itself with the proxy.
    /// </summary>
    public static readonly StatusCode ProxyAuthenticationRequired = AddInternal(407, "Proxy Authentication Required");

    /// <summary>
    /// The server timed out waiting for the request. According to HTTP specifications: "The client did not produce a request within the time that the server was prepared to wait. The client MAY repeat the request without modifications at any later time."
    /// </summary>
    public static readonly StatusCode RequestTimeout = AddInternal(408, "Request Timeout");

    /// <summary>
    /// Indicates that the request could not be processed because of conflict in the request, such as an edit conflict between multiple simultaneous updates.
    /// </summary>
    public static readonly StatusCode Conflict = AddInternal(409, "Conflict");

    /// <summary>
    /// Indicates that the resource requested is no longer available and will not be available again. This should be used when a resource has been intentionally removed and the resource should be purged. Upon receiving a 410 status code, the client should not request the resource in the future. Clients such as search engines should remove the resource from their indices. Most use cases do not require clients and search engines to purge the resource, and a "404 Not Found" may be used instead.
    /// </summary>
    public static readonly StatusCode Gone = AddInternal(410, "Gone");

    /// <summary>
    /// The request did not specify the length of its content, which is required by the requested resource.
    /// </summary>
    public static readonly StatusCode LengthRequired = AddInternal(411, "Length Required");

    /// <summary>
    /// The server does not meet one of the preconditions that the requester put on the request.
    /// </summary>
    public static readonly StatusCode PreconditionFailed = AddInternal(412, "Precondition Failed");

    /// <summary>
    /// The request is larger than the server is willing or able to process. Previously called "Request Entity Too Large".
    /// </summary>
    public static readonly StatusCode PayloadTooLarge = AddInternal(413, "Payload Too Large");

    /// <summary>
    /// The URI provided was too long for the server to process. Often the result of too much data being encoded as a query-string of a GET request, in which case it should be converted to a POST request. Called "Request-URI Too Long" previously.
    /// </summary>
    public static readonly StatusCode URITooLong = AddInternal(414, "URI Too Long");

    /// <summary>
    /// The request entity has a media type which the server or resource does not support. For example, the client uploads an image as image/svg+xml, but the server requires that images use a different format.
    /// </summary>
    public static readonly StatusCode UnsupportedMediaType = AddInternal(415, "Unsupported Media Type");

    /// <summary>
    /// The client has asked for a portion of the file (byte serving), but the server cannot supply that portion. For example, if the client asked for a part of the file that lies beyond the end of the file. Called "Requested Range Not Satisfiable" previously.
    /// </summary>
    public static readonly StatusCode RangeNotSatisfiable = AddInternal(416, "Range Not Satisfiable");

    /// <summary>
    /// The server cannot meet the requirements of the Expect request-header field.
    /// </summary>
    public static readonly StatusCode ExpectationFailed = AddInternal(417, "Expectation Failed");

    /// <summary>
    /// This code was defined in 1998 as one of the traditional IETF April Fools' jokes, in RFC 2324, Hyper Text Coffee Pot Control Protocol, and is not expected to be implemented by actual HTTP servers. The RFC specifies this code should be returned by tea pots requested to brew coffee. This HTTP status is used as an easter egg in some websites, including Google.com.
    /// </summary>
    public static readonly StatusCode ImATeapot = AddInternal(418, "I'm a Teapot");

    /// <summary>
    /// Returned by version 1 of the Twitter Search and Trends API when the client is being rate limited; versions 1.1 and later use the 429 Too Many Requests response code instead.
    /// </summary>
    public static readonly StatusCode EnhanceYourCalm = AddInternal(420, "Enhance Your Calm");

    /// <summary>
    /// The request was directed at a server that is not able to produce a response (for example because a connection reuse).
    /// </summary>
    public static readonly StatusCode MisdirectedRequest = AddInternal(421, "Misdirected Request");

    /// <summary>
    /// The request was well-formed but was unable to be followed due to semantic errors.
    /// </summary>
    public static readonly StatusCode UnprocessableEntity = AddInternal(422, "Unprocessable Entity");

    /// <summary>
    /// The resource that is being accessed is locked.
    /// </summary>
    public static readonly StatusCode Locked = AddInternal(423, "Locked");

    /// <summary>
    /// The request failed due to failure of a previous request (e.g., a PROPPATCH).
    /// </summary>
    public static readonly StatusCode FailedDependency = AddInternal(424, "Failed Dependency");

    /// <summary>
    /// The client should switch to a different protocol such as TLS/1.0, given in the Upgrade header field.
    /// </summary>
    public static readonly StatusCode UpgradeRequired = AddInternal(426, "Upgrade Required");

    /// <summary>
    /// The origin server requires the request to be conditional. Intended to prevent "the 'lost update' problem, where a client GETs a resource's state, modifies it, and PUTs it back to the server, when meanwhile a third party has modified the state on the server, leading to a conflict."
    /// </summary>
    public static readonly StatusCode PreconditionRequired = AddInternal(428, "Precondition Required");

    /// <summary>
    /// The user has sent too many requests in a given amount of time. Intended for use with rate limiting schemes.
    /// </summary>
    public static readonly StatusCode TooManyRequests = AddInternal(429, "Too Many Requests");

    /// <summary>
    /// The server is unwilling to process the request because either an individual header field, or all the header fields collectively, are too large.
    /// </summary>
    public static readonly StatusCode RequestHeaderFieldsTooLarge = AddInternal(431, "Request Header Fields Too Large");

    /// <summary>
    /// A server operator has received a legal demand to deny access to a resource or to a set of resources that includes the requested resource. The code 451 was chosen as a reference to the novel Fahrenheit 451.
    /// </summary>
    public static readonly StatusCode UnavailableForLegalReasons = AddInternal(451, "Unavailable For Legal Reasons");

    #endregion

    #region 5xx Server Error

    /// <summary>
    /// A generic error message, given when an unexpected condition was encountered and no more specific message is suitable.
    /// </summary>
    public static readonly StatusCode InternalServerError = AddInternal(500, "Internal Server Error");

    /// <summary>
    /// The server either does not recognize the request method, or it lacks the ability to fulfill the request. Usually this implies future availability (e.g., a new feature of a web-service API).
    /// </summary>
    public static readonly StatusCode NotImplemented = AddInternal(501, "Not Implemented");

    /// <summary>
    /// The server was acting as a gateway or proxy and received an invalid response from the upstream server.
    /// </summary>
    public static readonly StatusCode BadGateway = AddInternal(502, "Bad Gateway");

    /// <summary>
    /// The server is currently unavailable(because it is overloaded or down for maintenance). Generally, this is a temporary state.
    /// </summary>
    public static readonly StatusCode ServiceUnavailable = AddInternal(503, "Service Unavailable");

    /// <summary>
    /// The server was acting as a gateway or proxy and did not receive a timely response from the upstream server.
    /// </summary>
    public static readonly StatusCode GatewayTimeout = AddInternal(504, "Gateway Timeout");

    /// <summary>
    /// The server does not support the HTTP protocol version used in the request.
    /// </summary>
    public static readonly StatusCode HTTPVersionNotSupported = AddInternal(505, "HTTP Version Not Supported");

    /// <summary>
    /// Transparent content negotiation for the request results in a circular reference.
    /// </summary>
    public static readonly StatusCode VariantAlsoNegotiates = AddInternal(506, "Variant Also Negotiates");

    /// <summary>
    /// The server is unable to store the representation needed to complete the request.
    /// </summary>
    public static readonly StatusCode InsufficientStorage = AddInternal(507, "Insufficient Storage");

    /// <summary>
    /// The server detected an infinite loop while processing the request (sent in lieu of 208 Already Reported).
    /// </summary>
    public static readonly StatusCode LoopDetected = AddInternal(508, "Loop Detected");

    /// <summary>
    /// Further extensions to the request are required for the server to fulfill it.
    /// </summary>
    public static readonly StatusCode NotExtended = AddInternal(510, "Not Extended");

    /// <summary>
    /// The client needs to authenticate to gain network access. Intended for use by intercepting proxies used to control access to the network (e.g., "captive portals" used to require agreement to Terms of Service before granting full Internet access via a Wi-Fi hotspot).
    /// </summary>
    public static readonly StatusCode NetworkAuthenticationRequired = AddInternal(511, "Network Authentication Required");

    #endregion
}
