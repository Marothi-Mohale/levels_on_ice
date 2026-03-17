using LevelsOnIceSalon.Domain.Common;

namespace LevelsOnIceSalon.Domain.Entities;

public class SalonService : AuditableEntity
{
    public int ServiceCategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string? ShortDescription { get; set; }

    public string? FullDescription { get; set; }

    public decimal? PriceFrom { get; set; }

    public int? DurationMinutes { get; set; }

    public bool IsFeatured { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public ServiceCategory? ServiceCategory { get; set; }

    public ICollection<BookingRequest> BookingRequests { get; set; } = new List<BookingRequest>();
}
