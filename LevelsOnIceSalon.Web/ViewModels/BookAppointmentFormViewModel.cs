using System.ComponentModel.DataAnnotations;

namespace LevelsOnIceSalon.Web.ViewModels;

public class BookAppointmentFormViewModel
{
    [Required(ErrorMessage = "Please enter your full name.")]
    [StringLength(160)]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your phone number.")]
    [StringLength(40)]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(180)]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Please choose a preferred service.")]
    [Display(Name = "Preferred Service")]
    public int? PreferredServiceId { get; set; }

    [Required(ErrorMessage = "Please choose a preferred date.")]
    [Display(Name = "Preferred Date")]
    public DateOnly? PreferredDate { get; set; }

    [Required(ErrorMessage = "Please choose a preferred time.")]
    [Display(Name = "Preferred Time")]
    public TimeOnly? PreferredTime { get; set; }

    [StringLength(1500)]
    [Display(Name = "Notes")]
    public string? Notes { get; set; }

    [StringLength(120)]
    public string? Website { get; set; }

    public long FormRenderedAtUnix { get; set; }

    public string? CaptchaToken { get; set; }
}
