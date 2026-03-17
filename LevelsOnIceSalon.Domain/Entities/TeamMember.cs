using System.ComponentModel.DataAnnotations;
using LevelsOnIceSalon.Domain.Common;
using LevelsOnIceSalon.Domain.Enums;

namespace LevelsOnIceSalon.Domain.Entities;

public class TeamMember : AuditableEntity
{
    [Required]
    [StringLength(140)]
    public string FullName { get; set; } = string.Empty;

    public TeamMemberRole Role { get; set; } = TeamMemberRole.Hairstylist;

    [StringLength(250)]
    public string? ShortBio { get; set; }

    [StringLength(2000)]
    public string? FullBio { get; set; }

    [StringLength(400)]
    public string? PhotoPath { get; set; }

    [StringLength(160)]
    public string? InstagramHandle { get; set; }

    [StringLength(160)]
    public string? Specialty { get; set; }

    public bool IsFeatured { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public ICollection<AppointmentRequest> AppointmentRequests { get; set; } = new List<AppointmentRequest>();
}
