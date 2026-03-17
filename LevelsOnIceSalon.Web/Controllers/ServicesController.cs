using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers;

[Route("services")]
public class ServicesController(IServicesPageService servicesPageService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await servicesPageService.GetServicesPageAsync(cancellationToken);
        this.ApplySeo(
            model.PageTitle,
            "Explore nails, hairstyles, braids, beauty add-ons, and bridal beauty services at Levels On Ice Salon in Mowbray, Cape Town.",
            "/images/salon/hair-featured-01.jpg");
        return View(model);
    }
}
