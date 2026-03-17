namespace LevelsOnIceSalon.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAtUtc { get; set; }

    public DateTime? PublishedAtUtc { get; set; }

    public bool IsDeleted { get; set; }
}
