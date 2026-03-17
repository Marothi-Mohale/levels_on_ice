using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers;

[Route("about")]
public class AboutController(ISitePageContentService sitePageContentService) : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        var model = sitePageContentService.GetAboutPage();
        return View(model);
    }
}
