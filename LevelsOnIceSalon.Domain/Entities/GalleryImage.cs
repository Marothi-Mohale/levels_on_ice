using LevelsOnIceSalon.Domain.Common;

namespace LevelsOnIceSalon.Domain.Entities;

public class GalleryImage : AuditableEntity
{
    public string Title { get; set; } = string.Empty;

    public string ImagePath { get; set; } = string.Empty;

    public string? ThumbnailPath { get; set; }

    public string AltText { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string? Caption { get; set; }

    public bool IsFeatured { get; set; }

    public int DisplayOrder { get; set; }

    public string? SourceType { get; set; }

    public string? SourceUrl { get; set; }

    public bool IsPublished { get; set; } = true;
}
