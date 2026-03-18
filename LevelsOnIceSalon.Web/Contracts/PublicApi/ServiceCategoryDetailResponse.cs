namespace LevelsOnIceSalon.Web.Contracts.PublicApi;

public sealed record ServiceCategoryDetailResponse(
    int Id,
    string Name,
    string Slug,
    string? Description,
    int DisplayOrder,
    int ServiceCount,
    IReadOnlyList<ServiceSummaryResponse> Services);
