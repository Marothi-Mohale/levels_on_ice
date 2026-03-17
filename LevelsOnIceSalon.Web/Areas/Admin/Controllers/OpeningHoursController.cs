using LevelsOnIceSalon.Domain.Entities;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Areas.Admin.Controllers;

public class OpeningHoursController(ApplicationDbContext dbContext) : AdminControllerBase
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var items = await dbContext.OpeningHours
            .AsNoTracking()
            .OrderBy(x => x.DayOfWeek)
            .ToListAsync(cancellationToken);

        return View(items);
    }

    public IActionResult Create() => View("Edit", new OpeningHourFormViewModel());

    [HttpPost]
    public async Task<IActionResult> Create(OpeningHourFormViewModel model, CancellationToken cancellationToken)
    {
        await ValidateDayAsync(model.DayOfWeek, null, cancellationToken);
        if (!ModelState.IsValid)
        {
            return View("Edit", model);
        }

        var entity = new OpeningHour();
        Map(model, entity);
        dbContext.OpeningHours.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("Opening hour created.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.OpeningHours.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        return View(new OpeningHourFormViewModel
        {
            Id = entity.Id,
            DayOfWeek = entity.DayOfWeek,
            OpenTime = entity.OpenTime,
            CloseTime = entity.CloseTime,
            Notes = entity.Notes,
            IsClosed = entity.IsClosed
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, OpeningHourFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        await ValidateDayAsync(model.DayOfWeek, id, cancellationToken);
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var entity = await dbContext.OpeningHours.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        Map(model, entity);
        entity.UpdatedAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("Opening hour updated.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.OpeningHours.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return entity is null ? NotFound() : View(entity);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.OpeningHours.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        dbContext.OpeningHours.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("Opening hour deleted.");
        return RedirectToAction(nameof(Index));
    }

    private async Task ValidateDayAsync(int dayOfWeek, int? excludingId, CancellationToken cancellationToken)
    {
        if (await dbContext.OpeningHours.AnyAsync(x => x.DayOfWeek == dayOfWeek && (!excludingId.HasValue || x.Id != excludingId.Value), cancellationToken))
        {
            ModelState.AddModelError(nameof(OpeningHourFormViewModel.DayOfWeek), "This day already has opening hours.");
        }
    }

    private static void Map(OpeningHourFormViewModel source, OpeningHour target)
    {
        target.DayOfWeek = source.DayOfWeek;
        target.IsClosed = source.IsClosed;
        target.OpenTime = source.IsClosed ? null : source.OpenTime;
        target.CloseTime = source.IsClosed ? null : source.CloseTime;
        target.Notes = string.IsNullOrWhiteSpace(source.Notes) ? null : source.Notes.Trim();
    }
}
