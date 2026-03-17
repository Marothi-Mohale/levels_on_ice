using System.ComponentModel.DataAnnotations;
using LevelsOnIceSalon.Domain.Common;
using LevelsOnIceSalon.Domain.Enums;

namespace LevelsOnIceSalon.Domain.Entities;

public class Testimonial : AuditableEntity
{
    [Required]
    [StringLength(120)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Quote { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Rating { get; set; } = 5;

    [StringLength(120)]
    public string? ServiceName { get; set; }

    public TestimonialSourceType SourceType { get; set; } = TestimonialSourceType.Direct;

    [StringLength(400)]
    public string? SourceUrl { get; set; }

    public bool IsFeatured { get; set; }

    public bool IsPublished { get; set; } = true;

    public int DisplayOrder { get; set; }
}
