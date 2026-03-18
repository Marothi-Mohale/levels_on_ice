namespace LevelsOnIceSalon.Web.Contracts.PublicApi;

/// <summary>
/// Represents the detailed service category resource returned by a single-item lookup.
/// </summary>
public sealed class ServiceCategoryDetailResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceCategoryDetailResponse"/> class.
    /// </summary>
    public ServiceCategoryDetailResponse(
        int id,
        string name,
        string slug,
        string? description,
        int displayOrder,
        int serviceCount,
        IReadOnlyList<ServiceSummaryResponse> services)
    {
        Id = id;
        Name = name;
        Slug = slug;
        Description = description;
        DisplayOrder = displayOrder;
        ServiceCount = serviceCount;
        Services = services;
    }

    /// <summary>
    /// Gets the category identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the category display name.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets the stable slug used in URLs and client lookups.
    /// </summary>
    public string Slug { get; init; }

    /// <summary>
    /// Gets the short descriptive text for the category.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the configured display order for presentation.
    /// </summary>
    public int DisplayOrder { get; init; }

    /// <summary>
    /// Gets the number of active services in the category.
    /// </summary>
    public int ServiceCount { get; init; }

    /// <summary>
    /// Gets the active services that belong to the category.
    /// </summary>
    public IReadOnlyList<ServiceSummaryResponse> Services { get; init; }
}
