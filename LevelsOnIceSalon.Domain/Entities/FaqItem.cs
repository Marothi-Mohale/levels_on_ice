using LevelsOnIceSalon.Domain.Common;

namespace LevelsOnIceSalon.Domain.Entities;

public class FaqItem : AuditableEntity
{
    public string Question { get; set; } = string.Empty;

    public string Answer { get; set; } = string.Empty;

    public string? Category { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsPublished { get; set; } = true;
}
