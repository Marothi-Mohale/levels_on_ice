using System.ComponentModel.DataAnnotations;

namespace LevelsOnIceSalon.Web.Options;

public class AdminAuthOptions
{
    public const string SectionName = "AdminAuth";

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public bool RequireMfa { get; set; } = true;

    [RegularExpression("^[A-Z2-7=\\s]*$", ErrorMessage = "AdminAuth:MfaSharedKey must be a Base32 value.")]
    public string MfaSharedKey { get; set; } = string.Empty;
}
