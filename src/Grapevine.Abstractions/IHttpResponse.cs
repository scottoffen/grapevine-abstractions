﻿using System.Text;

namespace Grapevine.Abstractions;

public interface IHttpResponse
{
    /// <summary>
    /// Gets or sets the Encoding for this response's OutputStream
    /// </summary>
    Encoding? ContentEncoding { get; set; }

    /// <summary>
    /// Gets or sets the number of bytes in the body data included in the response
    /// </summary>
    long ContentLength64 { get; set; }

    /// <summary>
    /// Gets or sets an integer to indicate the minimum number of bytes after which content will potentially be compressed before being returned to the client
    /// </summary>
    TimeSpan ContentExpiresDuration { get; set; }

    /// <summary>
    /// Gets or sets the MIME type of the content returned
    /// </summary>
    string? ContentType { get; set; }

    /// <summary>
    /// Gets or sets the collection of cookies returned with the response
    /// </summary>
    CookieCollection Cookies { get; set; }

    /// <summary>
    /// Gets or sets the collection of header name/value pairs returned by the server
    /// </summary>
    HeaderCollection<ResponseHeader> Headers { get; set; }

    /// <summary>
    /// Gets a value indicating whether headers have been sent to the client.
    /// </summary>
    bool HasSentHeaders { get; }

    /// <summary>
    /// Gets a value indicating whether a response has been sent to this request
    /// </summary>
    bool HasSentResponse { get; }

    /// <summary>
    /// Gets the stream used to write response data. Writing to this stream will send data to the client.
    /// </summary>
    IObservableStream OutputStream { get; }

    /// <summary>
    /// Gets or sets the redirect location for this response
    /// </summary>
    string? RedirectLocation { get; set; }

    /// <summary>
    /// Gets or sets whether the response uses chunked transfer encoding
    /// </summary>
    bool SendChunked { get; set; }

    /// <summary>
    /// Gets or sets the HTTP status code to be returned to the client
    /// </summary>
    int StatusCode { get; set; }

    /// <summary>
    /// Gets or sets a text description of the HTTP status code returned to the client
    /// </summary>
    string StatusDescription { get; set; }

    /// <summary>
    /// Closes the connection to the client without sending a response.
    /// </summary>
    void Abort();

    /// <summary>
    /// Adds or replaces the specified header and value to the HTTP headers for this response
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    void AddHeader(string name, string value);

    /// <summary>
    /// Adds the specified Cookie to the collection of cookies for this response
    /// </summary>
    /// <param name="cookie"></param>
    void AppendCookie(Cookie cookie);

    /// <summary>
    /// Appends a value to the specified header. If the header already exists, the new value is appended using a comma separator.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    void AppendHeader(string name, string value);

    /// <summary>
    /// Flushes the response to the client without closing the connection.
    /// </summary>
    Task FlushAsync();

    /// <summary>
    /// Configures the response to redirect the client to the specified URL
    /// </summary>
    /// <param name="url"></param>
    void Redirect(string url);

    /// <summary>
    /// Write the contents of the buffer to and then closes the OutputStream, followed by closing the Response
    /// </summary>
    /// <param name="contents"></param>
    Task SendResponseAsync(byte[] contents);

    /// <summary>
    /// Adds or updates a Cookie in the collection of cookies sent with this response
    /// </summary>
    /// <param name="cookie"></param>
    void SetCookie(Cookie cookie);
}
