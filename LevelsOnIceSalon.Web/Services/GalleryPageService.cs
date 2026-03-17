using LevelsOnIceSalon.Domain.Entities;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Services;

public class GalleryPageService(ApplicationDbContext dbContext, IWebHostEnvironment environment) : IGalleryPageService
{
    private static readonly string[] SupportedExtensions = [".jpg", ".jpeg", ".png", ".webp"];

    public async Task<GalleryPageViewModel> GetGalleryPageAsync(CancellationToken cancellationToken = default)
    {
        var databaseItems = await dbContext.GalleryImages
            .AsNoTracking()
            .Where(image => image.IsPublished && !image.IsDeleted)
            .OrderByDescending(image => image.IsFeatured)
            .ThenBy(image => image.DisplayOrder)
            .ThenBy(image => image.Title)
            .Select(image => new GalleryItemViewModel
            {
                Title = image.Title,
                ImageUrl = image.ImagePath.StartsWith("/") ? image.ImagePath : "/" + image.ImagePath.TrimStart('/'),
                ThumbnailUrl = image.ThumbnailPath == null ? null : (image.ThumbnailPath.StartsWith("/") ? image.ThumbnailPath : "/" + image.ThumbnailPath.TrimStart('/')),
                AltText = image.AltText,
                Category = image.ImageType.ToString(),
                Caption = image.Caption,
                IsFeatured = image.IsFeatured,
                DisplayOrder = image.DisplayOrder
            })
            .ToListAsync(cancellationToken);

        var items = databaseItems.Any()
            ? databaseItems
            : GetStaticFallbackImages();

        var categories = items
            .Select(item => item.Category)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(category => category)
            .ToList();

        return new GalleryPageViewModel
        {
            PageTitle = "Gallery",
            BannerTitle = "A visual showcase of polished finishes, beautiful detail, and beauty work made to be remembered.",
            BannerCopy = "Explore a Mowbray, Cape Town salon gallery designed to feel premium, social-ready, and easy to browse. Today it can load from your local salon image folder, and when admin management is live it can switch seamlessly to database-driven content.",
            PrimaryCta = new CallToActionViewModel
            {
                Label = "Book Your Look",
                Url = "/book-appointment",
                SupportingText = "Turn inspiration into a booking request."
            },
            IsUsingDatabaseImages = databaseItems.Any(),
            Categories = categories,
            Items = items
        };
    }

    private List<GalleryItemViewModel> GetStaticFallbackImages()
    {
        var salonPath = Path.Combine(environment.WebRootPath, "images", "salon");
        if (!Directory.Exists(salonPath))
        {
            return [];
        }

        return Directory.EnumerateFiles(salonPath)
            .Where(file => SupportedExtensions.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase))
            .Where(file => !Path.GetFileName(file).StartsWith(".", StringComparison.Ordinal))
            .Where(file => !ShouldSkipStaticFile(Path.GetFileName(file)))
            .OrderBy(file => Path.GetFileName(file))
            .Select((file, index) => MapStaticFile(file, index))
            .ToList();
    }

    private static bool ShouldSkipStaticFile(string fileName)
    {
        var normalized = fileName.ToLowerInvariant();

        return normalized.StartsWith("logo.", StringComparison.Ordinal)
            || normalized.StartsWith("hero-main.", StringComparison.Ordinal)
            || normalized.StartsWith("hero-detail.", StringComparison.Ordinal);
    }

    private static GalleryItemViewModel MapStaticFile(string filePath, int index)
    {
        var fileName = Path.GetFileName(filePath);
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        var title = HumanizeFileName(nameWithoutExtension);
        var category = InferCategory(nameWithoutExtension);

        return new GalleryItemViewModel
        {
            Title = title,
            ImageUrl = $"/images/salon/{fileName}",
            ThumbnailUrl = $"/images/salon/{fileName}",
            AltText = ImageAltTextBuilder.ForGallery(title, category),
            Category = category,
            Caption = $"Levels On Ice Salon {category.ToLowerInvariant()} showcase",
            IsFeatured = index < 3,
            DisplayOrder = index + 1
        };
    }

    private static string HumanizeFileName(string value)
    {
        return string.Join(" ", value
            .Replace("_", " ", StringComparison.Ordinal)
            .Replace("-", " ", StringComparison.Ordinal)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(part => char.ToUpperInvariant(part[0]) + part[1..].ToLowerInvariant()));
    }

    private static string InferCategory(string fileName)
    {
        var normalized = fileName.ToLowerInvariant();

        if (normalized.Contains("braid", StringComparison.Ordinal) || normalized.Contains("protective", StringComparison.Ordinal))
        {
            return "Braids & Protective Styles";
        }

        if (normalized.Contains("bridal", StringComparison.Ordinal) || normalized.Contains("occasion", StringComparison.Ordinal))
        {
            return "Bridal / Special Occasion";
        }

        if (normalized.Contains("nails", StringComparison.Ordinal) || normalized.Contains("acrylic", StringComparison.Ordinal) || normalized.Contains("gel", StringComparison.Ordinal))
        {
            return "Nails";
        }

        if (normalized.Contains("interior", StringComparison.Ordinal) || normalized.Contains("salon", StringComparison.Ordinal) || normalized.Contains("refreshment", StringComparison.Ordinal))
        {
            return "Salon Experience";
        }

        if (normalized.Contains("hair", StringComparison.Ordinal)
            || normalized.Contains("curl", StringComparison.Ordinal)
            || normalized.Contains("cornrow", StringComparison.Ordinal)
            || normalized.Contains("silk", StringComparison.Ordinal))
        {
            return "Hairstyles";
        }

        return "Gallery Highlights";
    }
}
