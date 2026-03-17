namespace LevelsOnIceSalon.Web.ViewModels;

public class FaqsPageViewModel
{
    public string PageTitle { get; set; } = string.Empty;

    public string MetaDescription { get; set; } = string.Empty;

    public string BannerTitle { get; set; } = string.Empty;

    public string BannerCopy { get; set; } = string.Empty;

    public IList<FaqCategoryViewModel> Categories { get; set; } = [];

    public CallToActionViewModel PrimaryCta { get; set; } = new();
}
