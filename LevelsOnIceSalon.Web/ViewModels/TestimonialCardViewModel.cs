namespace LevelsOnIceSalon.Web.ViewModels;

public class TestimonialCardViewModel
{
    public string CustomerName { get; set; } = string.Empty;

    public string Quote { get; set; } = string.Empty;

    public int Rating { get; set; }

    public string? ServiceName { get; set; }

    public bool IsFeatured { get; set; }

    public string? SourceLabel { get; set; }
}
