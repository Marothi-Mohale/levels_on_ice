using System.ComponentModel.DataAnnotations;
using LevelsOnIceSalon.Domain.Enums;

namespace LevelsOnIceSalon.Web.Areas.Admin.ViewModels;

public class ContactMessageAdminFormViewModel
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    [Display(Name = "Status")]
    public ContactMessageStatus Status { get; set; }

    [Display(Name = "Admin Reply Notes")]
    [StringLength(1500)]
    public string? AdminReplyNotes { get; set; }
}
