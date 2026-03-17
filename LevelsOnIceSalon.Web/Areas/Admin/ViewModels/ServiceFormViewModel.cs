using System.ComponentModel.DataAnnotations;
using LevelsOnIceSalon.Domain.Enums;

namespace LevelsOnIceSalon.Web.Areas.Admin.ViewModels;

public class ServiceFormViewModel
{
    public int? Id { get; set; }

    [Required]
    [Display(Name = "Category")]
    public int ServiceCategoryId { get; set; }

    [Required]
    [StringLength(160)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(180)]
    [RegularExpression("^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Use lowercase letters, numbers, and hyphens only.")]
    public string Slug { get; set; } = string.Empty;

    [StringLength(300)]
    [Display(Name = "Short Description")]
    public string? ShortDescription { get; set; }

    [StringLength(2500)]
    [Display(Name = "Full Description")]
    public string? FullDescription { get; set; }

    [Display(Name = "Starting Price")]
    [Range(typeof(decimal), "0", "999999", ErrorMessage = "Enter a valid non-negative price.")]
    public decimal? Price { get; set; }

    [Display(Name = "Pricing Type")]
    public ServicePricingType PricingType { get; set; } = ServicePricingType.From;

    [Display(Name = "Duration (Minutes)")]
    [Range(1, 1440, ErrorMessage = "Enter a duration between 1 and 1440 minutes.")]
    public int? DurationMinutes { get; set; }

    [Display(Name = "Featured")]
    public bool IsFeatured { get; set; }

    [Display(Name = "Bookable Online")]
    public bool IsBookableOnline { get; set; } = true;

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Sort Order")]
    public int DisplayOrder { get; set; }
}
