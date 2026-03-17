using LevelsOnIceSalon.Domain.Enums;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Areas.Admin.Controllers;

public class DashboardController(ApplicationDbContext dbContext) : AdminControllerBase
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = new AdminDashboardViewModel
        {
            ServiceCount = await dbContext.Services.CountAsync(cancellationToken),
            CategoryCount = await dbContext.ServiceCategories.CountAsync(cancellationToken),
            GalleryImageCount = await dbContext.GalleryImages.CountAsync(cancellationToken),
            TestimonialCount = await dbContext.Testimonials.CountAsync(cancellationToken),
            FaqCount = await dbContext.Faqs.CountAsync(cancellationToken),
            PendingAppointmentCount = await dbContext.AppointmentRequests.CountAsync(request => request.Status == AppointmentRequestStatus.Pending, cancellationToken),
            NewContactMessageCount = await dbContext.ContactMessages.CountAsync(message => message.Status == ContactMessageStatus.New, cancellationToken),
            SiteSettingCount = await dbContext.SiteSettings.CountAsync(cancellationToken),
            OpeningHourCount = await dbContext.OpeningHours.CountAsync(cancellationToken)
        };

        return View(model);
    }
}
