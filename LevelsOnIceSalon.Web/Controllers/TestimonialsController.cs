using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers;

[Route("testimonials")]
public class TestimonialsController(ITestimonialsPageService testimonialsPageService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await testimonialsPageService.BuildPageModelAsync(cancellationToken);
        ViewData["MetaDescription"] = model.MetaDescription;
        return View(model);
    }
}
