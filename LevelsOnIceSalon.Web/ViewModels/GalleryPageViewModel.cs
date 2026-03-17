namespace LevelsOnIceSalon.Web.ViewModels;

public class GalleryPageViewModel
{
    public string PageTitle { get; set; } = string.Empty;

    public string BannerTitle { get; set; } = string.Empty;

    public string BannerCopy { get; set; } = string.Empty;

    public CallToActionViewModel PrimaryCta { get; set; } = new();

    public bool IsUsingDatabaseImages { get; set; }

    public IList<string> Categories { get; set; } = [];

    public IList<GalleryItemViewModel> Items { get; set; } = [];
}
