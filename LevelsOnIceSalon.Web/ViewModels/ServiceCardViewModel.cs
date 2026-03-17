namespace LevelsOnIceSalon.Web.ViewModels;

public class ServiceCardViewModel
{
    public string Name { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public int? ImageWidth { get; set; }

    public int? ImageHeight { get; set; }

    public string ImageAltText { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string DurationText { get; set; } = string.Empty;

    public string PriceText { get; set; } = string.Empty;

    public bool IsFeatured { get; set; }

    public string BookingUrl { get; set; } = "/book-appointment";
}
