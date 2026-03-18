using System.ComponentModel.DataAnnotations;

namespace LevelsOnIceSalon.Web.Contracts.PublicApi.Requests;

/// <summary>
/// Represents the credentials required to mint an API access token for an admin user.
/// </summary>
public sealed class CreateAccessTokenRequest
{
    /// <summary>
    /// Gets or sets the configured admin username.
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the configured admin password.
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current six-digit authenticator code.
    /// </summary>
    [Required]
    [RegularExpression("^\\d{6}$", ErrorMessage = "The oneTimeCode field must be a six-digit TOTP code.")]
    public string OneTimeCode { get; set; } = string.Empty;
}
