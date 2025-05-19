namespace Grapevine.Abstractions;

public enum SameSiteMode
{
    /// <summary>
    /// The cookie is sent with requests originating from the same site.
    /// </summary>
    None,

    /// <summary>
    /// The cookie is sent with requests originating from the same site and cross-site top-level navigations.
    /// </summary>
    Lax,

    /// <summary>
    /// The cookie is sent with requests originating from the same site and all cross-site requests.
    /// </summary>
    Strict
}
