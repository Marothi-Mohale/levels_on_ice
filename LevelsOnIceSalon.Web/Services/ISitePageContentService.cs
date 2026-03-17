using LevelsOnIceSalon.Web.ViewModels;

namespace LevelsOnIceSalon.Web.Services;

public interface ISitePageContentService
{
    HomePageViewModel GetHomePage();

    AboutPageViewModel GetAboutPage();

    PageBlueprintViewModel GetServicesPage();

    PageBlueprintViewModel GetGalleryPage();

    PageBlueprintViewModel GetTestimonialsPage();

    PageBlueprintViewModel GetFaqsPage();

    PageBlueprintViewModel GetContactPage();

    PageBlueprintViewModel GetBookAppointmentPage();
}
