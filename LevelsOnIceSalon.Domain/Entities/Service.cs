using System.ComponentModel.DataAnnotations;
using LevelsOnIceSalon.Domain.Common;
using LevelsOnIceSalon.Domain.Enums;

namespace LevelsOnIceSalon.Domain.Entities;

public class Service : AuditableEntity
{
    [Required]
    public int ServiceCategoryId { get; set; }

    [Required]
    [StringLength(160)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(180)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(300)]
    public string? ShortDescription { get; set; }

    [StringLength(2500)]
    public string? FullDescription { get; set; }

    public decimal? Price { get; set; }

    public ServicePricingType PricingType { get; set; } = ServicePricingType.From;

    public int? DurationMinutes { get; set; }

    public bool IsFeatured { get; set; }

    public bool IsBookableOnline { get; set; } = true;

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public ServiceCategory? ServiceCategory { get; set; }

    public ICollection<GalleryImage> GalleryImages { get; set; } = new List<GalleryImage>();

    public ICollection<AppointmentRequest> AppointmentRequests { get; set; } = new List<AppointmentRequest>();
}
