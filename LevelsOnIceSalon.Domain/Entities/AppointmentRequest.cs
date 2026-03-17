using System.ComponentModel.DataAnnotations;
using LevelsOnIceSalon.Domain.Common;
using LevelsOnIceSalon.Domain.Enums;

namespace LevelsOnIceSalon.Domain.Entities;

public class AppointmentRequest : AuditableEntity
{
    public int? ServiceId { get; set; }

    public int? TeamMemberId { get; set; }

    [Required]
    [StringLength(160)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(40)]
    public string PhoneNumber { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(180)]
    public string? Email { get; set; }

    [StringLength(120)]
    public string? PreferredStylistName { get; set; }

    public DateOnly? PreferredDate { get; set; }

    public TimeOnly? PreferredTime { get; set; }

    [StringLength(120)]
    public string? Occasion { get; set; }

    [StringLength(1500)]
    public string? Notes { get; set; }

    [StringLength(1500)]
    public string? InspirationReference { get; set; }

    public bool IsFirstTimeClient { get; set; }

    public AppointmentRequestStatus Status { get; set; } = AppointmentRequestStatus.Pending;

    public AppointmentRequestSource Source { get; set; } = AppointmentRequestSource.Website;

    [StringLength(1500)]
    public string? AdminNotes { get; set; }

    public Service? Service { get; set; }

    public TeamMember? TeamMember { get; set; }
}
