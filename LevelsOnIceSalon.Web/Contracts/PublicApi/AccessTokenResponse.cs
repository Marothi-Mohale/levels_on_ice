namespace LevelsOnIceSalon.Web.Contracts.PublicApi;

/// <summary>
/// Represents a successful bearer token response.
/// </summary>
public sealed class AccessTokenResponse
{
    /// <summary>
    /// Gets or sets the JWT bearer access token.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the token type. Always <c>Bearer</c>.
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// Gets or sets the access token lifetime in seconds.
    /// </summary>
    public int ExpiresIn { get; set; }
}
