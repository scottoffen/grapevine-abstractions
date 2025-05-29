﻿using System.Text;

namespace Grapevine.Abstractions;

public interface IHttpRequest
{
    /// <summary>
    /// Gets the MIME types accepted by the client.
    /// </summary>
    string[]? AcceptTypes { get; }

    /// <summary>
    /// Gets the content encoding that can be used with data sent with the request.
    /// </summary>
    Encoding ContentEncoding { get; }

    /// <summary>
    /// Gets the length of the body data included in the request.
    /// </summary>
    long ContentLength64 { get; }

    /// <summary>
    /// Gets the MIME type of the body data included in the request.
    /// </summary>
    string? ContentType { get; }

    /// <summary>
    /// Gets the cookies sent with the request.
    /// </summary>
    CookieCollection Cookies { get; }

    /// <summary>
    /// Gets the URL information (without the host and port) requested by the client.
    /// </summary>
    string Endpoint { get; }

    /// <summary>
    /// Gets a boolean value that indicates whether the request has associated body data.
    /// </summary>
    bool HasEntityBody { get; }

    /// <summary>
    /// Gets the collection of header name/value pairs sent in the request.
    /// </summary>
    HeaderCollection<RequestHeader> Headers { get; }

    /// <summary>
    /// Gets the scheme, host, and port portion of the request URI (e.g., "http://example.com:8080").
    /// </summary>
    string HostPrefix { get; }

    /// <summary>
    /// Gets the HTTPMethod specified by the client.
    /// </summary>
    HttpMethod HttpMethod { get; }

    /// <summary>
    /// Gets the input stream for the request.
    /// </summary>
    IObservableStream InputStream { get; }

    /// <summary>
    /// Gets the multipart boundary, returns empty string if not available.
    /// </summary>
    string MultipartBoundary { get; }

    /// <summary>
    /// Gets or sets a dictionary of parameters provided in the Endpoint as identified by the processing route.
    /// </summary>
    IDictionary<string, string> PathParameters { get; set; }

    /// <summary>
    /// Gets the query string included in the request.
    /// </summary>
    QueryParams QueryString { get; }

    /// <summary>
    /// Gets the URL information (without the host and port) requested by the client.
    /// </summary>
    string? RawUrl { get; }

    /// <summary>
    /// Gets the client IP address and port number from which the request originated.
    /// </summary>
    ConnectionInfo RemoteEndPoint { get; }

    /// <summary>
    /// Gets a representation of the HttpMethod and Endpoint of the request.
    /// </summary>
    string Signature { get; }

    /// <summary>
    /// Gets the Uri object requested by the client.
    /// </summary>
    Uri? Url { get; }

    /// <summary>
    /// Gets the Uniform Resource Identifier (URI) of the resource that referred the client to the server.
    /// </summary>
    Uri? UrlReferrer { get; }

    /// <summary>
    /// Gets the user agent presented by the client.
    /// </summary>
    string UserAgent { get; }

    /// <summary>
    /// Gets the server IP address and port number to which the request is directed.
    /// </summary>
    string UserHostAddress { get; }

    /// <summary>
    /// Gets the DNS name and, if provided, the port number specified by the client.
    /// </summary>
    string UserHostname { get; }

    /// <summary>
    /// Gets the natural languages that are preferred for the response.
    /// </summary>
    string[]? UserLanguages { get; }
}
