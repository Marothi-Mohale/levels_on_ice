namespace LevelsOnIceSalon.Web.Options;

public class SiteOptions
{
    public const string SectionName = "Site";

    public string BaseUrl { get; set; } = string.Empty;

    public string Name { get; set; } = "Levels On Ice Salon";

    public string DefaultSocialImage { get; set; } = "/images/salon/hair-featured-01.jpg";

    public string PhoneNumber { get; set; } = "081 390 6634";

    public string Email { get; set; } = "levelsonicegroup@gmail.com";

    public string StreetAddress { get; set; } = "102 Main Road";

    public string AddressLocality { get; set; } = "Mowbray";

    public string AddressRegion { get; set; } = "Cape Town";

    public string PostalCode { get; set; } = string.Empty;

    public string AddressCountry { get; set; } = "ZA";

    public string PriceRange { get; set; } = "$$";

    public string InstagramUrl { get; set; } = string.Empty;

    public string FacebookUrl { get; set; } = string.Empty;

    public string TikTokUrl { get; set; } = string.Empty;
}
