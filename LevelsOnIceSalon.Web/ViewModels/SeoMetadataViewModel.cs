namespace LevelsOnIceSalon.Web.ViewModels;

public class SeoMetadataViewModel
{
    public string Title { get; set; } = "Levels On Ice Salon";

    public string Description { get; set; } = "Premium beauty salon services in Mowbray, Cape Town.";

    public string CanonicalUrl { get; set; } = string.Empty;

    public string Robots { get; set; } = "index,follow,max-image-preview:large";

    public string OpenGraphType { get; set; } = "website";

    public string OpenGraphTitle { get; set; } = string.Empty;

    public string OpenGraphDescription { get; set; } = string.Empty;

    public string OpenGraphUrl { get; set; } = string.Empty;

    public string OpenGraphImageUrl { get; set; } = string.Empty;

    public string OpenGraphImageAlt { get; set; } = string.Empty;

    public string TwitterCard { get; set; } = "summary_large_image";

    public string TwitterTitle { get; set; } = string.Empty;

    public string TwitterDescription { get; set; } = string.Empty;

    public string TwitterImageUrl { get; set; } = string.Empty;

    public string JsonLd { get; set; } = string.Empty;
}
