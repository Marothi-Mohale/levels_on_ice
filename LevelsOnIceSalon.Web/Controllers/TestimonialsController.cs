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
        this.ApplySeo(
            model.PageTitle,
            model.MetaDescription,
            "/images/salon/hair-curls-01.jpg");
        return View(model);
    }
}
