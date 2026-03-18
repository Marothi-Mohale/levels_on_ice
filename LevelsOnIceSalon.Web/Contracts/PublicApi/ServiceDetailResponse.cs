namespace LevelsOnIceSalon.Web.Contracts.PublicApi;

public sealed record ServiceDetailResponse(
    int Id,
    string Name,
    string Slug,
    string? ShortDescription,
    string? FullDescription,
    decimal? Price,
    string PricingType,
    int? DurationMinutes,
    bool IsFeatured,
    bool IsBookableOnline,
    int DisplayOrder,
    ServiceCategoryReferenceResponse? Category);
