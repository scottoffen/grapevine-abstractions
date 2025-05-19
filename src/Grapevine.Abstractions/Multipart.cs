namespace Grapevine.Abstractions;

/// <summary>
/// Represents the subtype of multipart message
/// </summary>
/// <remarks>See https://www.w3.org/Protocols/rfc1341/7_2_Multipart.html</remarks>
public enum Multipart
{
    /// <summary>
    /// Default, indicates that the body parts are independent and intended to be displayed serially
    /// </summary>
    Mixed,

    /// <summary>
    /// Indicates that the body parts are alternative versions of the same information
    /// </summary>
    Alternative,

    /// <summary>
    /// Groups multiple message parts
    /// </summary>
    Digest,

    /// <summary>
    /// Contains encrypted data and decryption information
    /// </summary>
    Encrypted,

    /// <summary>
    /// Encoded for submissions
    /// </summary>
    FormData,

    /// <summary>
    /// Contains data that is intended to be processed in parallel
    /// </summary>
    Parallel,

    /// <summary>
    /// Interdependent body parts, such as a message and its attachments
    /// </summary>
    Related,

    /// <summary>
    /// Contains data with a cryptographic signature
    /// </summary>
    Signed
}
