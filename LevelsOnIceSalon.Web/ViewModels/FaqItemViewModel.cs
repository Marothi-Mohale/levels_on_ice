namespace LevelsOnIceSalon.Web.ViewModels;

public class FaqItemViewModel
{
    public int Id { get; set; }

    public string Question { get; set; } = string.Empty;

    public string Answer { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;
}
