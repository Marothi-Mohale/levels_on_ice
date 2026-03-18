using System.ComponentModel.DataAnnotations;
using LevelsOnIceSalon.Web.Validation;

namespace LevelsOnIceSalon.Web.Contracts.PublicApi.Requests;

public sealed class GetServicesRequest
{
    [StringLength(160)]
    public string? Category { get; init; }

    public bool? Featured { get; init; }

    public bool? BookableOnline { get; init; }

    [StringLength(120)]
    public string? Search { get; init; }

    [Range(1, int.MaxValue)]
    public int Page { get; init; } = 1;

    [Range(1, 100)]
    public int PageSize { get; init; } = 20;

    [StringLength(40)]
    [AllowedStringValues("displayOrder", "durationMinutes", "name", "price", ErrorMessage = "Sort must be one of: displayOrder, durationMinutes, name, price.")]
    public string Sort { get; init; } = "displayOrder";

    [StringLength(4)]
    [AllowedStringValues("asc", "desc", ErrorMessage = "Direction must be either 'asc' or 'desc'.")]
    public string Direction { get; init; } = "asc";
}
