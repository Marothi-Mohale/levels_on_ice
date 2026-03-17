using System.ComponentModel.DataAnnotations;
using LevelsOnIceSalon.Domain.Common;
using LevelsOnIceSalon.Domain.Enums;

namespace LevelsOnIceSalon.Domain.Entities;

public class ContactMessage : AuditableEntity
{
    [Required]
    [StringLength(160)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(180)]
    public string Email { get; set; } = string.Empty;

    [StringLength(40)]
    public string? PhoneNumber { get; set; }

    [Required]
    [StringLength(160)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [StringLength(2500)]
    public string Message { get; set; } = string.Empty;

    public ContactMessageStatus Status { get; set; } = ContactMessageStatus.New;

    [StringLength(1500)]
    public string? AdminReplyNotes { get; set; }
}
