using System.ComponentModel.DataAnnotations;
using LevelsOnIceSalon.Domain.Common;

namespace LevelsOnIceSalon.Domain.Entities;

public class OpeningHour : AuditableEntity
{
    [Range(0, 6)]
    public int DayOfWeek { get; set; }

    public TimeOnly? OpenTime { get; set; }

    public TimeOnly? CloseTime { get; set; }

    [StringLength(120)]
    public string? Notes { get; set; }

    public bool IsClosed { get; set; }
}
