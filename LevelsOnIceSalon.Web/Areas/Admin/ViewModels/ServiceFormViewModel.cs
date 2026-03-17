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
    public string Slug { get; set; } = string.Empty;

    [StringLength(300)]
    [Display(Name = "Short Description")]
    public string? ShortDescription { get; set; }

    [StringLength(2500)]
    [Display(Name = "Full Description")]
    public string? FullDescription { get; set; }

    [Display(Name = "Starting Price")]
    public decimal? Price { get; set; }

    [Display(Name = "Pricing Type")]
    public ServicePricingType PricingType { get; set; } = ServicePricingType.From;

    [Display(Name = "Duration (Minutes)")]
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
