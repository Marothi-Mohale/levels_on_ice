namespace LevelsOnIceSalon.Web.Contracts.PublicApi;

/// <summary>
/// Represents a compact category reference embedded in a service response.
/// </summary>
public sealed class ServiceCategoryReferenceResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceCategoryReferenceResponse"/> class.
    /// </summary>
    public ServiceCategoryReferenceResponse(int id, string name, string slug)
    {
        Id = id;
        Name = name;
        Slug = slug;
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
    /// Gets the category slug.
    /// </summary>
    public string Slug { get; init; }
}
