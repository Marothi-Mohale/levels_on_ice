namespace LevelsOnIceSalon.Web.ViewModels;

public class SitemapUrlEntry
{
    public string Url { get; set; } = string.Empty;

    public DateTime LastModifiedUtc { get; set; }

    public string ChangeFrequency { get; set; } = "weekly";

    public decimal Priority { get; set; } = 0.7m;
}
