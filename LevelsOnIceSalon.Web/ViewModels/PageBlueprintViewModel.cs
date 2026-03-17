namespace LevelsOnIceSalon.Web.ViewModels;

public class PageBlueprintViewModel
{
    public string PageTitle { get; set; } = string.Empty;

    public string NavigationTitle { get; set; } = string.Empty;

    public string Intro { get; set; } = string.Empty;

    public string Purpose { get; set; } = string.Empty;

    public IList<ContentBlockViewModel> ContentBlocks { get; set; } = [];

    public CallToActionViewModel PrimaryCta { get; set; } = new();

    public string DataSource { get; set; } = string.Empty;

    public string SeoIntent { get; set; } = string.Empty;
}
