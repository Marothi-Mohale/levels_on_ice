using LevelsOnIceSalon.Domain.Common;

namespace LevelsOnIceSalon.Domain.Entities;

public class BookingRequest : AuditableEntity
{
    public int? ServiceId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public DateOnly? PreferredDate { get; set; }

    public TimeOnly? PreferredTime { get; set; }

    public string? Notes { get; set; }

    public string Status { get; set; } = "New";

    public string? AdminNotes { get; set; }

    public SalonService? Service { get; set; }
}
