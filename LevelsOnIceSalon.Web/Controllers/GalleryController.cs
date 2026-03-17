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
        return View(model);
    }
}
