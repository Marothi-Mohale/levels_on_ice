using System.ComponentModel.DataAnnotations;
using LevelsOnIceSalon.Domain.Common;
using LevelsOnIceSalon.Domain.Enums;

namespace LevelsOnIceSalon.Domain.Entities;

public class GalleryImage : AuditableEntity
{
    public int? ServiceId { get; set; }

    [Required]
    [StringLength(160)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(400)]
    public string ImagePath { get; set; } = string.Empty;

    [StringLength(400)]
    public string? ThumbnailPath { get; set; }

    [Required]
    [StringLength(200)]
    public string AltText { get; set; } = string.Empty;

    public GalleryImageType ImageType { get; set; } = GalleryImageType.Nails;

    [StringLength(500)]
    public string? Caption { get; set; }

    public bool IsFeatured { get; set; }

    public int DisplayOrder { get; set; }

    [StringLength(40)]
    public string? SourceType { get; set; }

    [StringLength(400)]
    public string? SourceUrl { get; set; }

    public bool IsPublished { get; set; } = true;

    public Service? Service { get; set; }
}
