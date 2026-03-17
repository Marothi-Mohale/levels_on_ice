using LevelsOnIceSalon.Domain.Entities;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Areas.Admin.Controllers;

public class ContactMessagesController(ApplicationDbContext dbContext) : AdminControllerBase
{
    private const int PageSize = 12;

    public async Task<IActionResult> Index(int page = 1, CancellationToken cancellationToken = default)
    {
        var query = dbContext.ContactMessages
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync(cancellationToken);
        return View(new PagedResult<ContactMessage>
        {
            Items = items,
            PageNumber = page,
            PageSize = PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.ContactMessages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return entity is null ? NotFound() : View(entity);
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.ContactMessages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        return View(new ContactMessageAdminFormViewModel
        {
            Id = entity.Id,
            FullName = entity.FullName,
            Subject = entity.Subject,
            Status = entity.Status,
            AdminReplyNotes = entity.AdminReplyNotes
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, ContactMessageAdminFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var entity = await dbContext.ContactMessages.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        entity.Status = model.Status;
        entity.AdminReplyNotes = string.IsNullOrWhiteSpace(model.AdminReplyNotes) ? null : model.AdminReplyNotes.Trim();
        entity.UpdatedAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("Contact message updated.");
        return RedirectToAction(nameof(Index));
    }
}
