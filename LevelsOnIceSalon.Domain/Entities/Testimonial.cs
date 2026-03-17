using LevelsOnIceSalon.Domain.Common;

namespace LevelsOnIceSalon.Domain.Entities;

public class Testimonial : AuditableEntity
{
    public string CustomerName { get; set; } = string.Empty;

    public string Quote { get; set; } = string.Empty;

    public int Rating { get; set; } = 5;

    public string? ServiceName { get; set; }

    public bool IsFeatured { get; set; }

    public bool IsPublished { get; set; } = true;

    public int DisplayOrder { get; set; }
}
