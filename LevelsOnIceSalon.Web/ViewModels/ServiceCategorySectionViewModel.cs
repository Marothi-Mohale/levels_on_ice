namespace LevelsOnIceSalon.Web.ViewModels;

public class ServiceCategorySectionViewModel
{
    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }

    public IList<ServiceCardViewModel> Services { get; set; } = [];
}
