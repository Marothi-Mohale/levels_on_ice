namespace LevelsOnIceSalon.Web.Contracts.PublicApi;

/// <summary>
/// Represents the detailed service resource returned by a single-item lookup.
/// </summary>
public sealed class ServiceDetailResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceDetailResponse"/> class.
    /// </summary>
    public ServiceDetailResponse(
        int id,
        string name,
        string slug,
        string? shortDescription,
        string? fullDescription,
        decimal? price,
        string pricingType,
        int? durationMinutes,
        bool isFeatured,
        bool isBookableOnline,
        int displayOrder,
        ServiceCategoryReferenceResponse? category)
    {
        Id = id;
        Name = name;
        Slug = slug;
        ShortDescription = shortDescription;
        FullDescription = fullDescription;
        Price = price;
        PricingType = pricingType;
        DurationMinutes = durationMinutes;
        IsFeatured = isFeatured;
        IsBookableOnline = isBookableOnline;
        DisplayOrder = displayOrder;
        Category = category;
    }

    /// <summary>
    /// Gets the service identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the service display name.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets the stable slug used in URLs and client lookups.
    /// </summary>
    public string Slug { get; init; }

    /// <summary>
    /// Gets the short marketing summary for the service.
    /// </summary>
    public string? ShortDescription { get; init; }

    /// <summary>
    /// Gets the full detailed description for the service.
    /// </summary>
    public string? FullDescription { get; init; }

    /// <summary>
    /// Gets the configured price when a numeric price is available.
    /// </summary>
    public decimal? Price { get; init; }

    /// <summary>
    /// Gets the pricing mode, for example <c>From</c> or <c>Fixed</c>.
    /// </summary>
    public string PricingType { get; init; }

    /// <summary>
    /// Gets the expected appointment duration in minutes when known.
    /// </summary>
    public int? DurationMinutes { get; init; }

    /// <summary>
    /// Gets a value indicating whether the service is featured in marketing surfaces.
    /// </summary>
    public bool IsFeatured { get; init; }

    /// <summary>
    /// Gets a value indicating whether the service can be requested online.
    /// </summary>
    public bool IsBookableOnline { get; init; }

    /// <summary>
    /// Gets the configured display order for presentation.
    /// </summary>
    public int DisplayOrder { get; init; }

    /// <summary>
    /// Gets the parent category reference for the service.
    /// </summary>
    public ServiceCategoryReferenceResponse? Category { get; init; }
}
