namespace LevelsOnIceSalon.Web.ViewModels;

public class ServicesPageViewModel
{
    public string PageTitle { get; set; } = string.Empty;

    public string NavigationTitle { get; set; } = string.Empty;

    public string BannerTitle { get; set; } = string.Empty;

    public string BannerCopy { get; set; } = string.Empty;

    public CallToActionViewModel PrimaryCta { get; set; } = new();

    public IList<ServiceCardViewModel> FeaturedServices { get; set; } = [];

    public IList<ServiceCategorySectionViewModel> Categories { get; set; } = [];
}
