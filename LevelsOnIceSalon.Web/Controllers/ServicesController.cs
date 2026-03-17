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
        return View(model);
    }
}
