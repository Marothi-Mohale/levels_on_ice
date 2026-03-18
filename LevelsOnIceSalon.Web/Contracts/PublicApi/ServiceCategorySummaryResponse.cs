namespace LevelsOnIceSalon.Web.Contracts.PublicApi;

public sealed record ServiceCategorySummaryResponse(
    int Id,
    string Name,
    string Slug,
    string? Description,
    int DisplayOrder,
    int ServiceCount);
