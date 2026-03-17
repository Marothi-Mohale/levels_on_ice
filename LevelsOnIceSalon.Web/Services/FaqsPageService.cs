using LevelsOnIceSalon.Infrastructure.Data;
using LevelsOnIceSalon.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LevelsOnIceSalon.Web.Services;

public class FaqsPageService(ApplicationDbContext dbContext) : IFaqsPageService
{
    public async Task<FaqsPageViewModel> BuildPageModelAsync(CancellationToken cancellationToken = default)
    {
        var faqs = await dbContext.Faqs
            .AsNoTracking()
            .Where(faq => faq.IsPublished && !faq.IsDeleted)
            .OrderBy(faq => faq.Category)
            .ThenBy(faq => faq.DisplayOrder)
            .Select(faq => new FaqItemViewModel
            {
                Id = faq.Id,
                Question = faq.Question,
                Answer = faq.Answer,
                Category = faq.Category ?? "General"
            })
            .ToListAsync(cancellationToken);

        var categories = faqs
            .GroupBy(faq => faq.Category)
            .Select(group => new FaqCategoryViewModel
            {
                Name = group.Key,
                Slug = ToSlug(group.Key),
                Items = group.ToList()
            })
            .ToList();

        return new FaqsPageViewModel
        {
            PageTitle = "FAQs",
            MetaDescription = "Find answers about bookings, walk-ins, salon visits, and premium beauty services at Levels On Ice Salon in Mowbray, Cape Town.",
            BannerTitle = "Everything clients usually want to know before they book.",
            BannerCopy = "Quick answers, clear expectations, and the kind of practical detail that removes hesitation before your next beauty appointment.",
            Categories = categories,
            PrimaryCta = new CallToActionViewModel
            {
                Label = "Book Appointment",
                Url = "/book-appointment",
                SupportingText = "Move from questions to your booking request."
            }
        };
    }

    private static string ToSlug(string value) =>
        value.ToLowerInvariant()
            .Replace("&", "and", StringComparison.Ordinal)
            .Replace(" / ", "-", StringComparison.Ordinal)
            .Replace(" ", "-", StringComparison.Ordinal);
}
