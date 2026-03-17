using System.ComponentModel.DataAnnotations;

namespace LevelsOnIceSalon.Web.Options;

public class AdminAuthOptions
{
    public const string SectionName = "AdminAuth";

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
