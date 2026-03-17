using System.ComponentModel.DataAnnotations;

namespace LevelsOnIceSalon.Web.Areas.Admin.ViewModels;

public class FaqFormViewModel
{
    public int? Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Question { get; set; } = string.Empty;

    [Required]
    [StringLength(2500)]
    public string Answer { get; set; } = string.Empty;

    [StringLength(80)]
    public string? Category { get; set; }

    [Display(Name = "Sort Order")]
    public int DisplayOrder { get; set; }

    [Display(Name = "Published")]
    public bool IsPublished { get; set; } = true;
}
