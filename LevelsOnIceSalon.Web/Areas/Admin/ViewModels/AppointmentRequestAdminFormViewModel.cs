using System.ComponentModel.DataAnnotations;
using LevelsOnIceSalon.Domain.Enums;

namespace LevelsOnIceSalon.Web.Areas.Admin.ViewModels;

public class AppointmentRequestAdminFormViewModel
{
    public int Id { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string? ServiceName { get; set; }

    [Display(Name = "Status")]
    public AppointmentRequestStatus Status { get; set; }

    [Display(Name = "Admin Notes")]
    [StringLength(1500)]
    public string? AdminNotes { get; set; }
}
