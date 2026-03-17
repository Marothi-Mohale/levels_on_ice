using LevelsOnIceSalon.Domain.Entities;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Areas.Admin.Controllers;

public class FaqsController(ApplicationDbContext dbContext) : AdminControllerBase
{
    private const int PageSize = 10;

    public async Task<IActionResult> Index(int page = 1, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Faqs
            .AsNoTracking()
            .OrderBy(x => x.Category)
            .ThenBy(x => x.DisplayOrder)
            .ThenBy(x => x.Question);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync(cancellationToken);
        return View(new PagedResult<FAQ>
        {
            Items = items,
            PageNumber = page,
            PageSize = PageSize,
            TotalCount = totalCount
        });
    }

    public IActionResult Create() => View("Edit", new FaqFormViewModel());

    [HttpPost]
    public async Task<IActionResult> Create(FaqFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", model);
        }

        var entity = new FAQ();
        Map(model, entity);
        dbContext.Faqs.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("FAQ created.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Faqs.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        return View(new FaqFormViewModel
        {
            Id = entity.Id,
            Question = entity.Question,
            Answer = entity.Answer,
            Category = entity.Category,
            DisplayOrder = entity.DisplayOrder,
            IsPublished = entity.IsPublished
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, FaqFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var entity = await dbContext.Faqs.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        Map(model, entity);
        entity.UpdatedAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("FAQ updated.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Faqs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return entity is null ? NotFound() : View(entity);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Faqs.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        dbContext.Faqs.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("FAQ deleted.");
        return RedirectToAction(nameof(Index));
    }

    private static void Map(FaqFormViewModel source, FAQ target)
    {
        target.Question = source.Question.Trim();
        target.Answer = source.Answer.Trim();
        target.Category = string.IsNullOrWhiteSpace(source.Category) ? null : source.Category.Trim();
        target.DisplayOrder = source.DisplayOrder;
        target.IsPublished = source.IsPublished;
    }
}
