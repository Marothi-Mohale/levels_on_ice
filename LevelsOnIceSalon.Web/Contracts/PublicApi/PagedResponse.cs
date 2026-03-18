namespace LevelsOnIceSalon.Web.Contracts.PublicApi;

/// <summary>
/// Represents a page of API results together with paging metadata.
/// </summary>
/// <typeparam name="T">The item type contained in the current page.</typeparam>
public sealed class PagedResponse<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedResponse{T}"/> class.
    /// </summary>
    public PagedResponse(IReadOnlyList<T> items, int page, int pageSize, int totalCount, int totalPages)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = totalPages;
    }

    /// <summary>
    /// Gets the items in the current page.
    /// </summary>
    public IReadOnlyList<T> Items { get; init; }

    /// <summary>
    /// Gets the current one-based page number.
    /// </summary>
    public int Page { get; init; }

    /// <summary>
    /// Gets the requested page size.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Gets the total number of matching items before paging.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Gets the total number of pages available for the current page size.
    /// </summary>
    public int TotalPages { get; init; }
}
