using System.ComponentModel.DataAnnotations;
using LevelsOnIceSalon.Domain.Common;
using LevelsOnIceSalon.Domain.Enums;

namespace LevelsOnIceSalon.Domain.Entities;

public class PromotionBanner : AuditableEntity
{
    [Required]
    [StringLength(160)]
    public string Title { get; set; } = string.Empty;

    [StringLength(400)]
    public string? Subtitle { get; set; }

    [StringLength(120)]
    public string? CallToActionText { get; set; }

    [StringLength(300)]
    public string? CallToActionUrl { get; set; }

    public PromotionBannerStyle Style { get; set; } = PromotionBannerStyle.Primary;

    public DateOnly? StartsOn { get; set; }

    public DateOnly? EndsOn { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }
}
