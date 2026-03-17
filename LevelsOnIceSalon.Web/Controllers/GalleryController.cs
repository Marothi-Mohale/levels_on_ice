using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers;

[Route("gallery")]
public class GalleryController(IGalleryPageService galleryPageService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await galleryPageService.GetGalleryPageAsync(cancellationToken);
        this.ApplySeo(
            model.PageTitle,
            "Browse the Levels On Ice Salon gallery for premium nails, hairstyles, braids, and beauty inspiration from Mowbray, Cape Town.",
            "/images/salon/nails-featured-02.jpg");
        return View(model);
    }
}
