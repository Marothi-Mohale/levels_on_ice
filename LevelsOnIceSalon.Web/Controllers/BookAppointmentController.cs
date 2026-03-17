using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers;

[Route("book-appointment")]
public class BookAppointmentController(ISitePageContentService sitePageContentService) : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View(sitePageContentService.GetBookAppointmentPage());
    }
}
