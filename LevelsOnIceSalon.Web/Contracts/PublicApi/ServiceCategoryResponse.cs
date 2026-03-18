namespace LevelsOnIceSalon.Web.Contracts.PublicApi;

public sealed record ServiceCategoryResponse(
    int Id,
    string Name,
    string Slug,
    string? Description,
    int DisplayOrder,
    IReadOnlyList<ServiceSummaryResponse> Services);
