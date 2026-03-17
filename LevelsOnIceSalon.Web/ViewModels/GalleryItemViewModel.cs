namespace LevelsOnIceSalon.Web.ViewModels;

public class GalleryItemViewModel
{
    public string Title { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public string? ThumbnailUrl { get; set; }

    public string AltText { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string? Caption { get; set; }

    public bool IsFeatured { get; set; }

    public int DisplayOrder { get; set; }
}
