namespace LevelsOnIceSalon.Web.ViewModels;

public class FaqCategoryViewModel
{
    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public IList<FaqItemViewModel> Items { get; set; } = [];
}
