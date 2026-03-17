using LevelsOnIceSalon.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers;

[Route("faqs")]
public class FaqsController(IFaqsPageService faqsPageService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await faqsPageService.BuildPageModelAsync(cancellationToken);
        this.ApplySeo(
            model.PageTitle,
            model.MetaDescription,
            "/images/salon/salon-interior-02.jpg");
        return View(model);
    }
}
