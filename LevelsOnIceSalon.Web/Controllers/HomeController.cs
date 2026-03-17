using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers;

public class HomeController(ISitePageContentService sitePageContentService) : Controller
{
    public IActionResult Index()
    {
        var model = sitePageContentService.GetHomePage();
        this.ApplySeo(
            model.PageTitle,
            "Levels On Ice Salon is a premium beauty salon in Mowbray, Cape Town for nails, hairstyles, braids, and polished beauty appointments.",
            "/images/salon/hair-featured-01.jpg");
        return View(model);
    }

    public IActionResult Error()
    {
        return View();
    }
}
