using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers;

[Route("gallery")]
public class GalleryController(ISitePageContentService sitePageContentService) : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View(sitePageContentService.GetGalleryPage());
    }
}
