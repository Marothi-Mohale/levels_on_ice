using LevelsOnIceSalon.Web.Services;
using LevelsOnIceSalon.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers;

[Route("book-appointment")]
public class BookAppointmentController(IBookAppointmentService bookAppointmentService) : Controller
{
    private const string BookingStatusMessageKey = "BookingStatusMessage";

    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var statusMessage = TempData[BookingStatusMessageKey] as string;
        var model = await bookAppointmentService.BuildPageModelAsync(
            statusMessage: statusMessage,
            isSuccess: !string.IsNullOrWhiteSpace(statusMessage),
            cancellationToken: cancellationToken);
        return View(model);
    }

    [HttpPost("")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(BookAppointmentFormViewModel form, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            var invalidModel = await bookAppointmentService.BuildPageModelAsync(
                form,
                "Please correct the highlighted fields and try again.",
                isSuccess: false,
                cancellationToken: cancellationToken);

            return View(invalidModel);
        }

        var result = await bookAppointmentService.SubmitAsync(form, cancellationToken);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Message);

            var failedModel = await bookAppointmentService.BuildPageModelAsync(
                form,
                result.Message,
                isSuccess: false,
                cancellationToken: cancellationToken);

            return View(failedModel);
        }

        TempData[BookingStatusMessageKey] = result.Message;
        return RedirectToAction(nameof(Index));
    }
}
