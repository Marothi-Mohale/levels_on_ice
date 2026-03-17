namespace LevelsOnIceSalon.Web.ViewModels;

public class TestimonialsPageViewModel
{
    public string PageTitle { get; set; } = string.Empty;

    public string MetaDescription { get; set; } = string.Empty;

    public string BannerTitle { get; set; } = string.Empty;

    public string BannerCopy { get; set; } = string.Empty;

    public IList<TestimonialCardViewModel> FeaturedTestimonials { get; set; } = [];

    public IList<TestimonialCardViewModel> Testimonials { get; set; } = [];

    public CallToActionViewModel PrimaryCta { get; set; } = new();
}
