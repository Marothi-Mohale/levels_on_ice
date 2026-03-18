namespace LevelsOnIceSalon.Web.Contracts.PublicApi;

public sealed record ServiceSummaryResponse(
    int Id,
    int CategoryId,
    string Name,
    string Slug,
    string? ShortDescription,
    string? FullDescription,
    decimal? Price,
    string PricingType,
    int? DurationMinutes,
    bool IsFeatured,
    bool IsBookableOnline,
    int DisplayOrder);
