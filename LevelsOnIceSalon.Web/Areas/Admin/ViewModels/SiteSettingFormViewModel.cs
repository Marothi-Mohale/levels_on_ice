using System.ComponentModel.DataAnnotations;

namespace LevelsOnIceSalon.Web.Areas.Admin.ViewModels;

public class SiteSettingFormViewModel
{
    public int? Id { get; set; }

    [Required]
    [StringLength(120)]
    public string Key { get; set; } = string.Empty;

    [Required]
    [StringLength(2000)]
    public string Value { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string Group { get; set; } = "General";

    [StringLength(500)]
    public string? Description { get; set; }
}
