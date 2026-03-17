using LevelsOnIceSalon.Domain.Entities;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Areas.Admin.Controllers;

public class GalleryImagesController(ApplicationDbContext dbContext, IWebHostEnvironment environment) : AdminControllerBase
{
    private const int PageSize = 12;

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
        var extension = Path.GetExtension(file.FileName);
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
}
