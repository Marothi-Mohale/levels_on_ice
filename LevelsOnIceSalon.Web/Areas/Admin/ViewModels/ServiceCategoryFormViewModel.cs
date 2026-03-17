using System.ComponentModel.DataAnnotations;

namespace LevelsOnIceSalon.Web.Areas.Admin.ViewModels;

public class ServiceCategoryFormViewModel
{
    public int? Id { get; set; }

    [Required]
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(160)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Display(Name = "Sort Order")]
    public int DisplayOrder { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
}
