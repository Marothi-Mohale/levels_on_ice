namespace LevelsOnIceSalon.Web.Contracts.PublicApi;

/// <summary>
/// Represents a lightweight service category view for collection responses.
/// </summary>
public sealed class ServiceCategorySummaryResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceCategorySummaryResponse"/> class.
    /// </summary>
    public ServiceCategorySummaryResponse(int id, string name, string slug, string? description, int displayOrder, int serviceCount)
    {
        Id = id;
        Name = name;
        Slug = slug;
        Description = description;
        DisplayOrder = displayOrder;
        ServiceCount = serviceCount;
    }

    /// <summary>
    /// Gets the category identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the user-facing category name.
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
}
