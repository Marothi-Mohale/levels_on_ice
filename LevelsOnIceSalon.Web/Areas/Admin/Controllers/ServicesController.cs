using LevelsOnIceSalon.Domain.Entities;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Areas.Admin.Controllers;

public class ServicesController(ApplicationDbContext dbContext) : AdminControllerBase
{
    private const int PageSize = 10;

    public async Task<IActionResult> Index(int page = 1, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Services
            .AsNoTracking()
            .Include(x => x.ServiceCategory)
            .OrderBy(x => x.ServiceCategory!.DisplayOrder)
            .ThenBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync(cancellationToken);
        var model = new PagedResult<Service>
        {
            Items = items,
            PageNumber = page,
            PageSize = PageSize,
            TotalCount = totalCount
        };

        return View(model);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await PopulateCategoriesAsync(cancellationToken);
        return View("Edit", new ServiceFormViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(ServiceFormViewModel model, CancellationToken cancellationToken)
    {
        await ValidateServiceModelAsync(model, null, cancellationToken);
        if (!ModelState.IsValid)
        {
            await PopulateCategoriesAsync(cancellationToken);
            return View("Edit", model);
        }

        var entity = new Service();
        Map(model, entity);
        dbContext.Services.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("Service created.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Services.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        await PopulateCategoriesAsync(cancellationToken);
        return View(new ServiceFormViewModel
        {
            Id = entity.Id,
            ServiceCategoryId = entity.ServiceCategoryId,
            Name = entity.Name,
            Slug = entity.Slug,
            ShortDescription = entity.ShortDescription,
            FullDescription = entity.FullDescription,
            Price = entity.Price,
            PricingType = entity.PricingType,
            DurationMinutes = entity.DurationMinutes,
            IsFeatured = entity.IsFeatured,
            IsBookableOnline = entity.IsBookableOnline,
            IsActive = entity.IsActive,
            DisplayOrder = entity.DisplayOrder
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, ServiceFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        await ValidateServiceModelAsync(model, id, cancellationToken);
        if (!ModelState.IsValid)
        {
            await PopulateCategoriesAsync(cancellationToken);
            return View(model);
        }

        var entity = await dbContext.Services.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        Map(model, entity);
        entity.UpdatedAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("Service updated.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Services
            .AsNoTracking()
            .Include(x => x.ServiceCategory)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity is null ? NotFound() : View(entity);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Services.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        dbContext.Services.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("Service deleted.");
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateCategoriesAsync(CancellationToken cancellationToken)
    {
        ViewBag.Categories = await dbContext.ServiceCategories
            .AsNoTracking()
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToListAsync(cancellationToken);
    }

    private async Task ValidateServiceModelAsync(ServiceFormViewModel model, int? id, CancellationToken cancellationToken)
    {
        if (!await dbContext.ServiceCategories.AnyAsync(x => x.Id == model.ServiceCategoryId, cancellationToken))
        {
            ModelState.AddModelError(nameof(model.ServiceCategoryId), "Please select a valid category.");
        }

        if (await dbContext.Services.AnyAsync(x => x.Slug == model.Slug && (!id.HasValue || x.Id != id.Value), cancellationToken))
        {
            ModelState.AddModelError(nameof(model.Slug), "This slug is already in use.");
        }
    }

    private static void Map(ServiceFormViewModel source, Service target)
    {
        target.ServiceCategoryId = source.ServiceCategoryId;
        target.Name = source.Name.Trim();
        target.Slug = source.Slug.Trim().ToLowerInvariant();
        target.ShortDescription = string.IsNullOrWhiteSpace(source.ShortDescription) ? null : source.ShortDescription.Trim();
        target.FullDescription = string.IsNullOrWhiteSpace(source.FullDescription) ? null : source.FullDescription.Trim();
        target.Price = source.Price;
        target.PricingType = source.PricingType;
        target.DurationMinutes = source.DurationMinutes;
        target.IsFeatured = source.IsFeatured;
        target.IsBookableOnline = source.IsBookableOnline;
        target.IsActive = source.IsActive;
        target.DisplayOrder = source.DisplayOrder;
    }
}
