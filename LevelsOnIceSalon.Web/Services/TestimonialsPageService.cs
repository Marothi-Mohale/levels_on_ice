using LevelsOnIceSalon.Domain.Enums;
using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Services;

public class TestimonialsPageService(ApplicationDbContext dbContext) : ITestimonialsPageService
{
    public async Task<TestimonialsPageViewModel> BuildPageModelAsync(CancellationToken cancellationToken = default)
    {
        var testimonials = await dbContext.Testimonials
            .AsNoTracking()
            .Where(testimonial => testimonial.IsPublished && !testimonial.IsDeleted)
            .OrderByDescending(testimonial => testimonial.IsFeatured)
            .ThenBy(testimonial => testimonial.DisplayOrder)
            .ThenBy(testimonial => testimonial.CustomerName)
            .Select(testimonial => new TestimonialCardViewModel
            {
                CustomerName = testimonial.CustomerName,
                Quote = testimonial.Quote,
                Rating = testimonial.Rating,
                ServiceName = testimonial.ServiceName,
                IsFeatured = testimonial.IsFeatured,
                SourceLabel = testimonial.SourceType == TestimonialSourceType.Direct ? "Client review" : "Social proof"
            })
            .ToListAsync(cancellationToken);

        return new TestimonialsPageViewModel
        {
            PageTitle = "Testimonials",
            MetaDescription = "Read client testimonials for Levels On Ice Salon in Mowbray, Cape Town and see why women trust the salon for polished nails, hairstyles, and premium beauty appointments.",
            BannerTitle = "Client love that makes booking feel easy.",
            BannerCopy = "Real reactions, polished results, and the kind of social proof that helps first-time clients feel confident before they book.",
            FeaturedTestimonials = testimonials.Where(testimonial => testimonial.IsFeatured).Take(3).ToList(),
            Testimonials = testimonials,
            PrimaryCta = new CallToActionViewModel
            {
                Label = "Book With Confidence",
                Url = "/book-appointment",
                SupportingText = "Turn social proof into your next beauty booking."
            }
        };
    }
}
