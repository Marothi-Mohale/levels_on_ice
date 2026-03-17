namespace LevelsOnIceSalon.Web.ViewModels;

public class HomeSectionPlanViewModel
{
    public string SectionId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Purpose { get; set; } = string.Empty;

    public IList<ContentBlockViewModel> ContentBlocks { get; set; } = [];

    public CallToActionViewModel PrimaryCta { get; set; } = new();

    public string DataSource { get; set; } = string.Empty;

    public string SeoIntent { get; set; } = string.Empty;
}
