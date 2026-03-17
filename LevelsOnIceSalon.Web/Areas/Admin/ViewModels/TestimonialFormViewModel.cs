using System.ComponentModel.DataAnnotations;
using LevelsOnIceSalon.Domain.Enums;

namespace LevelsOnIceSalon.Web.Areas.Admin.ViewModels;

public class TestimonialFormViewModel
{
    public int? Id { get; set; }

    [Required]
    [StringLength(120)]
    [Display(Name = "Customer Name")]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Quote { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Rating { get; set; } = 5;

    [StringLength(120)]
    [Display(Name = "Service Name")]
    public string? ServiceName { get; set; }

    [Display(Name = "Source Type")]
    public TestimonialSourceType SourceType { get; set; } = TestimonialSourceType.Direct;

    [StringLength(400)]
    [Url]
    [Display(Name = "Source URL")]
    public string? SourceUrl { get; set; }

    [Display(Name = "Featured")]
    public bool IsFeatured { get; set; }

    [Display(Name = "Published")]
    public bool IsPublished { get; set; } = true;

    [Display(Name = "Sort Order")]
    public int DisplayOrder { get; set; }
}
