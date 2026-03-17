using System.ComponentModel.DataAnnotations;
using LevelsOnIceSalon.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace LevelsOnIceSalon.Web.Areas.Admin.ViewModels;

public class GalleryImageFormViewModel
{
    public int? Id { get; set; }

    public int? ServiceId { get; set; }

    [Required]
    [StringLength(160)]
    public string Title { get; set; } = string.Empty;

    [StringLength(400)]
    public string? ExistingImagePath { get; set; }

    [Display(Name = "Image File")]
    public IFormFile? ImageFile { get; set; }

    [StringLength(200)]
    [Required]
    [Display(Name = "Alt Text")]
    public string AltText { get; set; } = string.Empty;

    [Display(Name = "Image Type")]
    public GalleryImageType ImageType { get; set; } = GalleryImageType.Nails;

    [StringLength(500)]
    public string? Caption { get; set; }

    [Display(Name = "Featured")]
    public bool IsFeatured { get; set; }

    [Display(Name = "Sort Order")]
    public int DisplayOrder { get; set; }

    [StringLength(40)]
    [Display(Name = "Source Type")]
    public string? SourceType { get; set; }

    [StringLength(400)]
    [Display(Name = "Source URL")]
    public string? SourceUrl { get; set; }

    [Display(Name = "Published")]
    public bool IsPublished { get; set; } = true;
}
