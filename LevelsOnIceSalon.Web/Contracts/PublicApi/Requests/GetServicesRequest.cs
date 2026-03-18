using System.ComponentModel.DataAnnotations;
using LevelsOnIceSalon.Web.Validation;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Contracts.PublicApi.Requests;

/// <summary>
/// Represents query-string filters, sort options, and paging controls for the services collection endpoint.
/// </summary>
public sealed class GetServicesRequest
{
    /// <summary>
    /// Gets the category slug filter.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("category")]
    [FromQuery(Name = "category")]
    [StringLength(160)]
    public string? Category { get; init; }

    /// <summary>
    /// Gets a value indicating whether only featured services should be returned.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("featured")]
    [FromQuery(Name = "featured")]
    public bool? Featured { get; init; }

    /// <summary>
    /// Gets a value indicating whether only online-bookable services should be returned.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("bookableOnline")]
    [FromQuery(Name = "bookableOnline")]
    public bool? BookableOnline { get; init; }

    /// <summary>
    /// Gets the free-text search term applied to service names and descriptions.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("search")]
    [FromQuery(Name = "search")]
    [StringLength(120)]
    public string? Search { get; init; }

    /// <summary>
    /// Gets the one-based page number to return.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("page")]
    [FromQuery(Name = "page")]
    [Range(1, int.MaxValue)]
    public int Page { get; init; } = 1;

    /// <summary>
    /// Gets the number of items to return per page.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("pageSize")]
    [FromQuery(Name = "pageSize")]
    [Range(1, 100)]
    public int PageSize { get; init; } = 20;

    /// <summary>
    /// Gets the sort field. Supported values are <c>displayOrder</c>, <c>durationMinutes</c>, <c>name</c>, and <c>price</c>.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("sort")]
    [FromQuery(Name = "sort")]
    [StringLength(40)]
    [AllowedStringValues("displayOrder", "durationMinutes", "name", "price", ErrorMessage = "Sort must be one of: displayOrder, durationMinutes, name, price.")]
    public string Sort { get; init; } = "displayOrder";

    /// <summary>
    /// Gets the sort direction. Supported values are <c>asc</c> and <c>desc</c>.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("direction")]
    [FromQuery(Name = "direction")]
    [StringLength(4)]
    [AllowedStringValues("asc", "desc", ErrorMessage = "Direction must be either 'asc' or 'desc'.")]
    public string Direction { get; init; } = "asc";
}
