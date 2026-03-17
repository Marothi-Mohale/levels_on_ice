using LevelsOnIceSalon.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LevelsOnIceSalon.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var model = new HomePageViewModel
        {
            SalonName = "Levels On Ice Salon",
            Tagline = "Classy beauty experiences for bold, stylish women in Cape Town.",
            Address = "102 Main Road, Mowbray, Cape Town",
            PhoneNumber = "081 390 6634",
            Email = "levelsonicegroup@gmail.com"
        };

        return View(model);
    }

    public IActionResult Error()
    {
        return View();
    }
}
