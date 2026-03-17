namespace LevelsOnIceSalon.Web.ViewModels;

public class HomePageViewModel : PageBlueprintViewModel
{
    public string SalonName { get; set; } = string.Empty;

    public string Tagline { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public IList<HomeSectionPlanViewModel> Sections { get; set; } = [];
}
