using LevelsOnIceSalon.Domain.Common;

namespace LevelsOnIceSalon.Domain.Entities;

public class ServiceCategory : AuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<SalonService> Services { get; set; } = new List<SalonService>();
}
