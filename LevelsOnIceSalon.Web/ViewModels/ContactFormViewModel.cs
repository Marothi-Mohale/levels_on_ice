using System.ComponentModel.DataAnnotations;

namespace LevelsOnIceSalon.Web.ViewModels;

public class ContactFormViewModel
{
    [Required]
    [Display(Name = "Full Name")]
    [StringLength(160)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(180)]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [Display(Name = "Phone Number")]
    [StringLength(40)]
    public string? PhoneNumber { get; set; }

    [Required]
    [StringLength(160)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [StringLength(2500)]
    public string Message { get; set; } = string.Empty;

    public string? Website { get; set; }

    public long FormRenderedAtUnix { get; set; }
}
