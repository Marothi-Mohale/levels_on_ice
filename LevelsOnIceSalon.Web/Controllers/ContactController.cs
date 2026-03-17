using LevelsOnIceSalon.Web.Services;
using LevelsOnIceSalon.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers;

[Route("contact")]
public class ContactController(IContactPageService contactPageService) : Controller
{
    private const string ContactStatusMessageKey = "ContactStatusMessage";

    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var statusMessage = TempData[ContactStatusMessageKey] as string;
        var model = await contactPageService.BuildPageModelAsync(
            statusMessage: statusMessage,
            isSuccess: !string.IsNullOrWhiteSpace(statusMessage),
            cancellationToken: cancellationToken);

        ViewData["MetaDescription"] = model.MetaDescription;
        return View(model);
    }

    [HttpPost("")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactFormViewModel form, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            var invalidModel = await contactPageService.BuildPageModelAsync(
                form,
                "Please correct the highlighted fields and try again.",
                isSuccess: false,
                cancellationToken: cancellationToken);

            ViewData["MetaDescription"] = invalidModel.MetaDescription;
            return View(invalidModel);
        }

        var result = await contactPageService.SubmitAsync(form, cancellationToken);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Message);

            var failedModel = await contactPageService.BuildPageModelAsync(
                form,
                result.Message,
                isSuccess: false,
                cancellationToken: cancellationToken);

            ViewData["MetaDescription"] = failedModel.MetaDescription;
            return View(failedModel);
        }

        TempData[ContactStatusMessageKey] = result.Message;
        return RedirectToAction(nameof(Index));
    }
}
