using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers;

public class HomeController(ISitePageContentService sitePageContentService) : Controller
{
    [HttpGet("")]
    [HttpGet("/")]
    public IActionResult Index()
    {
        var model = sitePageContentService.GetHomePage();
        return View(model);
    }

    public IActionResult Error()
    {
        return View();
    }
}
