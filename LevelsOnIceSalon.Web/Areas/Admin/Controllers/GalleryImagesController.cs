using LevelsOnIceSalon.Domain.Entities;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Areas.Admin.Controllers;

public class GalleryImagesController(
    ApplicationDbContext dbContext,
    IWebHostEnvironment environment,
    ILogger<GalleryImagesController> logger) : AdminControllerBase
{
    private const int PageSize = 12;
    private const long MaxUploadBytes = 5 * 1024 * 1024;
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

    public async Task<IActionResult> Index(int page = 1, CancellationToken cancellationToken = default)
    {
        var query = dbContext.GalleryImages
            .AsNoTracking()
            .Include(x => x.Service)
            .OrderByDescending(x => x.IsFeatured)
            .ThenBy(x => x.DisplayOrder)
            .ThenBy(x => x.Title);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync(cancellationToken);
        return View(new PagedResult<GalleryImage>
        {
            Items = items,
            PageNumber = page,
            PageSize = PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await PopulateServicesAsync(cancellationToken);
        return View("Edit", new GalleryImageFormViewModel());
    }

    [RequestFormLimits(MultipartBodyLengthLimit = MaxUploadBytes)]
    [HttpPost]
    public async Task<IActionResult> Create(GalleryImageFormViewModel model, CancellationToken cancellationToken)
    {
        await ValidateAsync(model, cancellationToken);
        if (!ModelState.IsValid)
        {
            await PopulateServicesAsync(cancellationToken);
            return View("Edit", model);
        }

        var entity = new GalleryImage();
        await MapAsync(model, entity, cancellationToken);
        dbContext.GalleryImages.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Created gallery image {GalleryImageId}.", entity.Id);
        SetSuccessMessage("Gallery image created.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.GalleryImages.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        await PopulateServicesAsync(cancellationToken);
        return View(new GalleryImageFormViewModel
        {
            Id = entity.Id,
            ServiceId = entity.ServiceId,
            Title = entity.Title,
            ExistingImagePath = entity.ImagePath,
            AltText = entity.AltText,
            ImageType = entity.ImageType,
            Caption = entity.Caption,
            IsFeatured = entity.IsFeatured,
            DisplayOrder = entity.DisplayOrder,
            SourceType = entity.SourceType,
            SourceUrl = entity.SourceUrl,
            IsPublished = entity.IsPublished
        });
    }

    [RequestFormLimits(MultipartBodyLengthLimit = MaxUploadBytes)]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, GalleryImageFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        await ValidateAsync(model, cancellationToken);
        if (!ModelState.IsValid)
        {
            await PopulateServicesAsync(cancellationToken);
            return View(model);
        }

        var entity = await dbContext.GalleryImages.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        await MapAsync(model, entity, cancellationToken);
        entity.UpdatedAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Updated gallery image {GalleryImageId}.", entity.Id);
        SetSuccessMessage("Gallery image updated.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.GalleryImages
            .AsNoTracking()
            .Include(x => x.Service)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity is null ? NotFound() : View(entity);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.GalleryImages.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        DeleteImageFile(entity.ImagePath);
        dbContext.GalleryImages.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Deleted gallery image {GalleryImageId}.", id);
        SetSuccessMessage("Gallery image deleted.");
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateServicesAsync(CancellationToken cancellationToken)
    {
        ViewBag.Services = await dbContext.Services
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToListAsync(cancellationToken);
    }

    private async Task ValidateAsync(GalleryImageFormViewModel model, CancellationToken cancellationToken)
    {
        if (model.ServiceId.HasValue && !await dbContext.Services.AnyAsync(x => x.Id == model.ServiceId.Value, cancellationToken))
        {
            ModelState.AddModelError(nameof(model.ServiceId), "Select a valid service.");
        }

        if (model.Id is null && model.ImageFile is null)
        {
            ModelState.AddModelError(nameof(model.ImageFile), "Please upload an image.");
        }

        if (model.ImageFile is not null)
        {
            ValidateImageFile(model.ImageFile);
        }
    }

    private async Task MapAsync(GalleryImageFormViewModel source, GalleryImage target, CancellationToken cancellationToken)
    {
        target.ServiceId = source.ServiceId;
        target.Title = source.Title.Trim();
        target.AltText = source.AltText.Trim();
        target.ImageType = source.ImageType;
        target.Caption = string.IsNullOrWhiteSpace(source.Caption) ? null : source.Caption.Trim();
        target.IsFeatured = source.IsFeatured;
        target.DisplayOrder = source.DisplayOrder;
        target.SourceType = string.IsNullOrWhiteSpace(source.SourceType) ? null : source.SourceType.Trim();
        target.SourceUrl = string.IsNullOrWhiteSpace(source.SourceUrl) ? null : source.SourceUrl.Trim();
        target.IsPublished = source.IsPublished;

        if (source.ImageFile is not null)
        {
            if (!string.IsNullOrWhiteSpace(target.ImagePath))
            {
                DeleteImageFile(target.ImagePath);
            }

            target.ImagePath = await SaveImageAsync(source.ImageFile, cancellationToken);
        }
        else if (!string.IsNullOrWhiteSpace(source.ExistingImagePath))
        {
            target.ImagePath = source.ExistingImagePath;
        }
    }

    private async Task<string> SaveImageAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var uploadsPath = Path.Combine(environment.WebRootPath, "uploads", "gallery");
        Directory.CreateDirectory(uploadsPath);
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadsPath, fileName);
        await using var stream = System.IO.File.Create(filePath);
        await file.CopyToAsync(stream, cancellationToken);
        return $"/uploads/gallery/{fileName}";
    }

    private void DeleteImageFile(string? imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath) || !imagePath.StartsWith("/uploads/gallery/", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var relativePath = imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.Combine(environment.WebRootPath, relativePath);
        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }
    }

    private void ValidateImageFile(IFormFile file)
    {
        if (file.Length <= 0)
        {
            ModelState.AddModelError(nameof(GalleryImageFormViewModel.ImageFile), "The uploaded image is empty.");
            return;
        }

        if (file.Length > MaxUploadBytes)
        {
            ModelState.AddModelError(nameof(GalleryImageFormViewModel.ImageFile), "Upload an image smaller than 5 MB.");
        }

        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
        {
            ModelState.AddModelError(nameof(GalleryImageFormViewModel.ImageFile), "Upload a JPG, PNG, or WebP image.");
            return;
        }

        if (!HasSupportedImageSignature(file))
        {
            ModelState.AddModelError(nameof(GalleryImageFormViewModel.ImageFile), "The uploaded file is not a supported image.");
        }
    }

    private static bool HasSupportedImageSignature(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        Span<byte> header = stackalloc byte[12];
        var bytesRead = stream.Read(header);
        if (bytesRead < 12)
        {
            return false;
        }

        var isJpeg = header[0] == 0xFF && header[1] == 0xD8;
        var isPng = header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47;
        var isWebp = header[0] == (byte)'R'
            && header[1] == (byte)'I'
            && header[2] == (byte)'F'
            && header[3] == (byte)'F'
            && header[8] == (byte)'W'
            && header[9] == (byte)'E'
            && header[10] == (byte)'B'
            && header[11] == (byte)'P';

        return isJpeg || isPng || isWebp;
    }
}
