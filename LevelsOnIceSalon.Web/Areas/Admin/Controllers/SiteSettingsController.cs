using LevelsOnIceSalon.Domain.Entities;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Areas.Admin.Controllers;

public class SiteSettingsController(ApplicationDbContext dbContext) : AdminControllerBase
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var items = await dbContext.SiteSettings
            .AsNoTracking()
            .OrderBy(x => x.Group)
            .ThenBy(x => x.Key)
            .ToListAsync(cancellationToken);

        return View(items);
    }

    public IActionResult Create() => View("Edit", new SiteSettingFormViewModel());

    [HttpPost]
    public async Task<IActionResult> Create(SiteSettingFormViewModel model, CancellationToken cancellationToken)
    {
        if (await KeyExistsAsync(model.Key, null, cancellationToken))
        {
            ModelState.AddModelError(nameof(model.Key), "This setting key already exists.");
        }

        if (!ModelState.IsValid)
        {
            return View("Edit", model);
        }

        var entity = new SiteSetting();
        Map(model, entity);
        dbContext.SiteSettings.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("Site setting created.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.SiteSettings.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        return View(new SiteSettingFormViewModel
        {
            Id = entity.Id,
            Key = entity.Key,
            Value = entity.Value,
            Group = entity.Group,
            Description = entity.Description
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, SiteSettingFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (await KeyExistsAsync(model.Key, id, cancellationToken))
        {
            ModelState.AddModelError(nameof(model.Key), "This setting key already exists.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var entity = await dbContext.SiteSettings.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        Map(model, entity);
        entity.UpdatedAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("Site setting updated.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.SiteSettings.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return entity is null ? NotFound() : View(entity);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.SiteSettings.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        dbContext.SiteSettings.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("Site setting deleted.");
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> KeyExistsAsync(string key, int? excludingId, CancellationToken cancellationToken) =>
        await dbContext.SiteSettings.AnyAsync(
            x => x.Key == key && (!excludingId.HasValue || x.Id != excludingId.Value),
            cancellationToken);

    private static void Map(SiteSettingFormViewModel source, SiteSetting target)
    {
        target.Key = source.Key.Trim();
        target.Value = source.Value.Trim();
        target.Group = source.Group.Trim();
        target.Description = string.IsNullOrWhiteSpace(source.Description) ? null : source.Description.Trim();
    }
}
