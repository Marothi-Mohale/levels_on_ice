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
        this.ApplySeo(
            model.PageTitle,
            "Learn about Levels On Ice Salon, a modern beauty destination in Mowbray, Cape Town known for polished service, premium finishes, and a welcoming salon experience.",
            "/images/salon/salon-interior-01.jpg");
        return View(model);
    }
}
