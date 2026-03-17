using LevelsOnIceSalon.Domain.Entities;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Areas.Admin.Controllers;

public class TestimonialsController(ApplicationDbContext dbContext) : AdminControllerBase
{
    private const int PageSize = 10;

    public async Task<IActionResult> Index(int page = 1, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Testimonials
            .AsNoTracking()
            .OrderByDescending(x => x.IsFeatured)
            .ThenBy(x => x.DisplayOrder)
            .ThenBy(x => x.CustomerName);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync(cancellationToken);
        return View(new PagedResult<Testimonial>
        {
            Items = items,
            PageNumber = page,
            PageSize = PageSize,
            TotalCount = totalCount
        });
    }

    public IActionResult Create() => View("Edit", new TestimonialFormViewModel());

    [HttpPost]
    public async Task<IActionResult> Create(TestimonialFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", model);
        }

        var entity = new Testimonial();
        Map(model, entity);
        dbContext.Testimonials.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("Testimonial created.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Testimonials.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        return View(new TestimonialFormViewModel
        {
            Id = entity.Id,
            CustomerName = entity.CustomerName,
            Quote = entity.Quote,
            Rating = entity.Rating,
            ServiceName = entity.ServiceName,
            SourceType = entity.SourceType,
            SourceUrl = entity.SourceUrl,
            IsFeatured = entity.IsFeatured,
            IsPublished = entity.IsPublished,
            DisplayOrder = entity.DisplayOrder
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, TestimonialFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var entity = await dbContext.Testimonials.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        Map(model, entity);
        entity.UpdatedAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("Testimonial updated.");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Testimonials.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return entity is null ? NotFound() : View(entity);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Testimonials.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        dbContext.Testimonials.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("Testimonial deleted.");
        return RedirectToAction(nameof(Index));
    }

    private static void Map(TestimonialFormViewModel source, Testimonial target)
    {
        target.CustomerName = source.CustomerName.Trim();
        target.Quote = source.Quote.Trim();
        target.Rating = source.Rating;
        target.ServiceName = string.IsNullOrWhiteSpace(source.ServiceName) ? null : source.ServiceName.Trim();
        target.SourceType = source.SourceType;
        target.SourceUrl = string.IsNullOrWhiteSpace(source.SourceUrl) ? null : source.SourceUrl.Trim();
        target.IsFeatured = source.IsFeatured;
        target.IsPublished = source.IsPublished;
        target.DisplayOrder = source.DisplayOrder;
    }
}
