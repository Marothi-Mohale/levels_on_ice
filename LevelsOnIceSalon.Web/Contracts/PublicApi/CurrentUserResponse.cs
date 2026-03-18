namespace LevelsOnIceSalon.Web.Contracts.PublicApi;

/// <summary>
/// Represents the authenticated API principal.
/// </summary>
public sealed class CurrentUserResponse
{
    /// <summary>
    /// Gets or sets the authenticated username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the authentication scheme used by the principal.
    /// </summary>
    public string AuthenticationScheme { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the roles assigned to the principal.
    /// </summary>
    public IReadOnlyList<string> Roles { get; set; } = [];

    /// <summary>
    /// Gets or sets the authentication methods recorded for the token.
    /// </summary>
    public IReadOnlyList<string> AuthenticationMethods { get; set; } = [];
}
