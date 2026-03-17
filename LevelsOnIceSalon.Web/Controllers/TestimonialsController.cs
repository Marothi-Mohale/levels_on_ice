using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers;

[Route("testimonials")]
public class TestimonialsController(ISitePageContentService sitePageContentService) : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View(sitePageContentService.GetTestimonialsPage());
    }
}
