namespace LevelsOnIceSalon.Web.ViewModels;

public class AboutPageViewModel
{
    public string PageTitle { get; set; } = string.Empty;

    public string NavigationTitle { get; set; } = string.Empty;

    public string Intro { get; set; } = string.Empty;

    public string BrandStory { get; set; } = string.Empty;

    public string AudienceFit { get; set; } = string.Empty;

    public string Atmosphere { get; set; } = string.Empty;

    public IList<ContentBlockViewModel> DifferencePoints { get; set; } = [];

    public IList<TeamMemberProfileViewModel> TeamMembers { get; set; } = [];

    public CallToActionViewModel PrimaryCta { get; set; } = new();
}
