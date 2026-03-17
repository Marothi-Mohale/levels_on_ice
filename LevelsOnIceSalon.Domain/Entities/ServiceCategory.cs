using System.ComponentModel.DataAnnotations;
using LevelsOnIceSalon.Domain.Common;

namespace LevelsOnIceSalon.Domain.Entities;

public class ServiceCategory : AuditableEntity
{
    [Required]
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(160)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Service> Services { get; set; } = new List<Service>();
}
