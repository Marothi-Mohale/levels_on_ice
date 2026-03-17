using LevelsOnIceSalon.Domain.Entities;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Areas.Admin.Controllers;

public class ServiceCategoriesController(ApplicationDbContext dbContext) : AdminControllerBase
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var items = await dbContext.ServiceCategories
            .AsNoTracking()
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return View(items);
    }

    public IActionResult Create() => View("Edit", new ServiceCategoryFormViewModel());

    [HttpPost]
    public async Task<IActionResult> Create(ServiceCategoryFormViewModel model, CancellationToken cancellationToken)
    {
        if (await SlugExistsAsync(model.Slug, null, cancellationToken))
        {
            ModelState.AddModelError(nameof(model.Slug), "This slug is already in use.");
        }

        if (!ModelState.IsValid)
        {
            return View("Edit", model);
        }

        var entity = new ServiceCategory();
        Map(model, entity);
        dbContext.ServiceCategories.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("Service category created.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.ServiceCategories.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        return View(new ServiceCategoryFormViewModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Slug = entity.Slug,
            Description = entity.Description,
            DisplayOrder = entity.DisplayOrder,
            IsActive = entity.IsActive
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, ServiceCategoryFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (await SlugExistsAsync(model.Slug, id, cancellationToken))
        {
            ModelState.AddModelError(nameof(model.Slug), "This slug is already in use.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var entity = await dbContext.ServiceCategories.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        Map(model, entity);
        entity.UpdatedAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("Service category updated.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.ServiceCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity is null ? NotFound() : View(entity);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.ServiceCategories
            .Include(x => x.Services)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
        {
            return NotFound();
        }

        if (entity.Services.Count > 0)
        {
            SetErrorMessage("Delete services in this category first.");
            return RedirectToAction(nameof(Index));
        }

        dbContext.ServiceCategories.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("Service category deleted.");
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> SlugExistsAsync(string slug, int? excludingId, CancellationToken cancellationToken) =>
        await dbContext.ServiceCategories.AnyAsync(
            x => x.Slug == slug && (!excludingId.HasValue || x.Id != excludingId.Value),
            cancellationToken);

    private static void Map(ServiceCategoryFormViewModel source, ServiceCategory target)
    {
        target.Name = source.Name.Trim();
        target.Slug = source.Slug.Trim().ToLowerInvariant();
        target.Description = string.IsNullOrWhiteSpace(source.Description) ? null : source.Description.Trim();
        target.DisplayOrder = source.DisplayOrder;
        target.IsActive = source.IsActive;
    }
}
