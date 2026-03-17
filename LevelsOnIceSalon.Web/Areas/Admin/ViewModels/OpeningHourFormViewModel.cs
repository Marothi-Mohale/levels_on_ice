using System.ComponentModel.DataAnnotations;

namespace LevelsOnIceSalon.Web.Areas.Admin.ViewModels;

public class OpeningHourFormViewModel
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
}
