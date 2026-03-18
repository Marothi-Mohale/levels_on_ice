namespace LevelsOnIceSalon.Web.Contracts.PublicApi;

public sealed record ServiceCategoryReferenceResponse(
    int Id,
    string Name,
    string Slug);
