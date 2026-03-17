using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LevelsOnIceSalon.Web.Areas.Admin.ViewModels;

public class OpeningHourFormViewModel : IValidatableObject
{
    public int? Id { get; set; }

    [Range(0, 6)]
    [Display(Name = "Day Of Week")]
    public int DayOfWeek { get; set; }

    [Display(Name = "Open Time")]
    public TimeOnly? OpenTime { get; set; }

    [Display(Name = "Close Time")]
    public TimeOnly? CloseTime { get; set; }

    [StringLength(120)]
    public string? Notes { get; set; }

    [Display(Name = "Closed")]
    public bool IsClosed { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IsClosed)
        {
            yield break;
        }

        if (!OpenTime.HasValue)
        {
            yield return new ValidationResult("Open time is required unless the day is closed.", [nameof(OpenTime)]);
        }

        if (!CloseTime.HasValue)
        {
            yield return new ValidationResult("Close time is required unless the day is closed.", [nameof(CloseTime)]);
        }

        if (OpenTime.HasValue && CloseTime.HasValue && CloseTime <= OpenTime)
        {
            yield return new ValidationResult("Close time must be later than open time.", [nameof(CloseTime)]);
        }
    }
}
