using System.ComponentModel.DataAnnotations;
using LevelsOnIceSalon.Domain.Common;

namespace LevelsOnIceSalon.Domain.Entities;

public class FAQ : AuditableEntity
{
    [Required]
    [StringLength(250)]
    public string Question { get; set; } = string.Empty;

    [Required]
    [StringLength(2500)]
    public string Answer { get; set; } = string.Empty;

    [StringLength(80)]
    public string? Category { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsPublished { get; set; } = true;
}
