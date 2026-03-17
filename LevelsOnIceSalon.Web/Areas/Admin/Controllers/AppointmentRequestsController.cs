using LevelsOnIceSalon.Domain.Entities;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Areas.Admin.Controllers;

public class AppointmentRequestsController(ApplicationDbContext dbContext) : AdminControllerBase
{
    private const int PageSize = 12;

    public async Task<IActionResult> Index(int page = 1, CancellationToken cancellationToken = default)
    {
        var query = dbContext.AppointmentRequests
            .AsNoTracking()
            .Include(x => x.Service)
            .OrderByDescending(x => x.CreatedAtUtc);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync(cancellationToken);
        return View(new PagedResult<AppointmentRequest>
        {
            Items = items,
            PageNumber = page,
            PageSize = PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.AppointmentRequests
            .AsNoTracking()
            .Include(x => x.Service)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity is null ? NotFound() : View(entity);
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.AppointmentRequests
            .AsNoTracking()
            .Include(x => x.Service)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null)
        {
            return NotFound();
        }

        return View(new AppointmentRequestAdminFormViewModel
        {
            Id = entity.Id,
            CustomerName = entity.FullName,
            ServiceName = entity.Service?.Name,
            Status = entity.Status,
            AdminNotes = entity.AdminNotes
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, AppointmentRequestAdminFormViewModel model, CancellationToken cancellationToken)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var entity = await dbContext.AppointmentRequests.FindAsync([id], cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        entity.Status = model.Status;
        entity.AdminNotes = string.IsNullOrWhiteSpace(model.AdminNotes) ? null : model.AdminNotes.Trim();
        entity.UpdatedAtUtc = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        SetSuccessMessage("Appointment request updated.");
        return RedirectToAction(nameof(Index));
    }
}
