using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers;

[Route("faqs")]
public class FaqsController(ISitePageContentService sitePageContentService) : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View(sitePageContentService.GetFaqsPage());
    }
}
