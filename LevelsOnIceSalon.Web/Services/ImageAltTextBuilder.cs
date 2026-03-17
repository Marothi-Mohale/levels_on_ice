namespace LevelsOnIceSalon.Web.Services;

public static class ImageAltTextBuilder
{
    public static string ForService(string serviceName) =>
        $"{serviceName} at Levels On Ice Salon in Mowbray, Cape Town";

    public static string ForGallery(string title, string category) =>
        $"{title} from the {category} gallery at Levels On Ice Salon in Mowbray, Cape Town";
}
