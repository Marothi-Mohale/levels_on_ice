using System.ComponentModel.DataAnnotations;

namespace LevelsOnIceSalon.Web.Areas.Admin.ViewModels;

public class AdminLoginViewModel
{
    [Required]
    [Display(Name = "Username")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Authenticator Code")]
    [StringLength(12)]
    public string OneTimeCode { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }

    public string? StatusMessage { get; set; }
}
