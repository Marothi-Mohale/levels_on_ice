using LevelsOnIceSalon.Domain.Entities;
using LevelsOnIceSalon.Domain.Enums;

namespace LevelsOnIceSalon.Infrastructure.Data.Seed;

public static class SampleData
{
    public static readonly DateTime SeedCreatedAtUtc = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static IEnumerable<ServiceCategory> ServiceCategories =>
    [
        new ServiceCategory
        {
            Id = 1,
            Name = "Nails",
            Slug = "nails",
            Description = "High-gloss nail artistry for soft luxury girls, statement sets, and polished everyday confidence.",
            DisplayOrder = 1,
            IsActive = true,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new ServiceCategory
        {
            Id = 2,
            Name = "Hairstyles",
            Slug = "hairstyles",
            Description = "Effortless glam, soft luxury styling, and event-ready looks with a premium salon finish.",
            DisplayOrder = 2,
            IsActive = true,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new ServiceCategory
        {
            Id = 3,
            Name = "Braids & Protective Styles",
            Slug = "braids-protective-styles",
            Description = "Protective styling that feels neat, feminine, trend-aware, and beautifully camera-ready.",
            DisplayOrder = 3,
            IsActive = true,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new ServiceCategory
        {
            Id = 4,
            Name = "Beauty Add-ons",
            Slug = "beauty-add-ons",
            Description = "Finishing touches that elevate your appointment from simple upkeep to a full beauty moment.",
            DisplayOrder = 4,
            IsActive = true,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new ServiceCategory
        {
            Id = 5,
            Name = "Bridal / Special Occasion",
            Slug = "bridal-special-occasion",
            Description = "Elegant beauty styling for weddings, celebrations, shoots, graduations, and unforgettable entrances.",
            DisplayOrder = 5,
            IsActive = true,
            CreatedAtUtc = SeedCreatedAtUtc
        }
    ];

    public static IEnumerable<Service> Services =>
    [
        new Service
        {
            Id = 1,
            ServiceCategoryId = 1,
            Name = "Gloss Theory Gel Set",
            Slug = "gloss-theory-gel-set",
            ShortDescription = "A sleek gel finish for girls who love clean, expensive-looking nails every day.",
            FullDescription = "A premium gel manicure with precision prep, shaping, cuticle care, and a glossy salon finish that feels polished, soft, and photo-ready.",
            Price = 320m,
            PricingType = ServicePricingType.From,
            DurationMinutes = 80,
            IsFeatured = true,
            IsBookableOnline = true,
            IsActive = true,
            DisplayOrder = 1,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new Service
        {
            Id = 2,
            ServiceCategoryId = 1,
            Name = "Iced-Out Acrylic Signature Set",
            Slug = "iced-out-acrylic-signature-set",
            ShortDescription = "A bold acrylic look with shape, length, and attitude for girls who love a standout finish.",
            FullDescription = "Our statement acrylic set is built for noticeable beauty with sculpted shape, length customization, and a crisp, luxe finish that photographs beautifully.",
            Price = 520m,
            PricingType = ServicePricingType.From,
            DurationMinutes = 140,
            IsFeatured = true,
            IsBookableOnline = true,
            IsActive = true,
            DisplayOrder = 2,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new Service
        {
            Id = 3,
            ServiceCategoryId = 1,
            Name = "French Girl Refill",
            Slug = "french-girl-refill",
            ShortDescription = "A fresh refill for clients who love soft luxury nails with a clean designer finish.",
            FullDescription = "Perfect for maintaining a polished acrylic or gel look, this refill includes reshaping, balancing, and a refined finish that keeps your set looking elevated.",
            Price = 380m,
            PricingType = ServicePricingType.From,
            DurationMinutes = 90,
            IsFeatured = false,
            IsBookableOnline = true,
            IsActive = true,
            DisplayOrder = 3,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new Service
        {
            Id = 4,
            ServiceCategoryId = 2,
            Name = "Silk Press & Sway Finish",
            Slug = "silk-press-sway-finish",
            ShortDescription = "A smooth, bouncy finish with movement, shine, and soft luxury energy.",
            FullDescription = "This silk press service is designed for sleek movement, healthy shine, and a refined finish that feels expensive without looking overdone.",
            Price = 650m,
            PricingType = ServicePricingType.From,
            DurationMinutes = 120,
            IsFeatured = true,
            IsBookableOnline = true,
            IsActive = true,
            DisplayOrder = 1,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new Service
        {
            Id = 5,
            ServiceCategoryId = 2,
            Name = "Soft Glam Curls Styling",
            Slug = "soft-glam-curls-styling",
            ShortDescription = "Defined curls and a glossy finish for birthdays, dinners, graduations, and pretty-girl weekends.",
            FullDescription = "A styled finish built around volume, shape, and softness for clients who want occasion-ready hair that still feels modern and wearable.",
            Price = 480m,
            PricingType = ServicePricingType.From,
            DurationMinutes = 90,
            IsFeatured = true,
            IsBookableOnline = true,
            IsActive = true,
            DisplayOrder = 2,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new Service
        {
            Id = 6,
            ServiceCategoryId = 3,
            Name = "Knotless Braids, Mid-Back",
            Slug = "knotless-braids-mid-back",
            ShortDescription = "Neat, lightweight knotless braids with a soft feminine finish and everyday luxury feel.",
            FullDescription = "A client-favorite protective style with a clean parting pattern, natural fall, and a polished result that feels stylish from day one.",
            Price = 950m,
            PricingType = ServicePricingType.From,
            DurationMinutes = 300,
            IsFeatured = true,
            IsBookableOnline = true,
            IsActive = true,
            DisplayOrder = 1,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new Service
        {
            Id = 7,
            ServiceCategoryId = 3,
            Name = "Boho Knotless Luxe",
            Slug = "boho-knotless-luxe",
            ShortDescription = "A soft, romantic protective style with loose textured pieces for an elevated look.",
            FullDescription = "This premium boho knotless style blends neat structure with soft loose detail, making it ideal for clients who want protective styling with movement and personality.",
            Price = 1350m,
            PricingType = ServicePricingType.From,
            DurationMinutes = 360,
            IsFeatured = true,
            IsBookableOnline = true,
            IsActive = true,
            DisplayOrder = 2,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new Service
        {
            Id = 8,
            ServiceCategoryId = 3,
            Name = "Sleek Cornrow Design",
            Slug = "sleek-cornrow-design",
            ShortDescription = "A clean braided look with precise lines for sporty-chic, low-maintenance beauty.",
            FullDescription = "A neat protective style with detail-focused parting and a sleek finish that works beautifully for everyday wear, holidays, and active weeks.",
            Price = 450m,
            PricingType = ServicePricingType.From,
            DurationMinutes = 120,
            IsFeatured = false,
            IsBookableOnline = true,
            IsActive = true,
            DisplayOrder = 3,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new Service
        {
            Id = 9,
            ServiceCategoryId = 4,
            Name = "Brow Clean-Up & Tint",
            Slug = "brow-clean-up-tint",
            ShortDescription = "A polished brow refresh that sharpens your whole face in under an hour.",
            FullDescription = "A quick beauty add-on that shapes, tidies, and softly defines the brows for a more put-together finish.",
            Price = 180m,
            PricingType = ServicePricingType.Fixed,
            DurationMinutes = 35,
            IsFeatured = false,
            IsBookableOnline = true,
            IsActive = true,
            DisplayOrder = 1,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new Service
        {
            Id = 10,
            ServiceCategoryId = 4,
            Name = "Soft Glam Face Beat Add-On",
            Slug = "soft-glam-face-beat-add-on",
            ShortDescription = "A camera-friendly glam touch-up for clients pairing hair and beauty in one visit.",
            FullDescription = "A lightweight glam makeup add-on designed to enhance features and complete your salon look without needing a full separate appointment.",
            Price = 420m,
            PricingType = ServicePricingType.From,
            DurationMinutes = 60,
            IsFeatured = true,
            IsBookableOnline = true,
            IsActive = true,
            DisplayOrder = 2,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new Service
        {
            Id = 11,
            ServiceCategoryId = 4,
            Name = "Lash Flick Finish",
            Slug = "lash-flick-finish",
            ShortDescription = "A flirty finishing touch for girls who want their salon look to feel complete.",
            FullDescription = "An easy beauty add-on that adds eye definition and softness, ideal before events, dates, birthdays, and content days.",
            Price = 150m,
            PricingType = ServicePricingType.Fixed,
            DurationMinutes = 20,
            IsFeatured = false,
            IsBookableOnline = true,
            IsActive = true,
            DisplayOrder = 3,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new Service
        {
            Id = 12,
            ServiceCategoryId = 5,
            Name = "Bridal Preview Styling Session",
            Slug = "bridal-preview-styling-session",
            ShortDescription = "A consultation-led preview for brides who want to feel fully sure before the big day.",
            FullDescription = "A bridal planning appointment focused on trying the look, refining the finish, and building confidence around your final beauty direction.",
            Price = 950m,
            PricingType = ServicePricingType.From,
            DurationMinutes = 150,
            IsFeatured = true,
            IsBookableOnline = true,
            IsActive = true,
            DisplayOrder = 1,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new Service
        {
            Id = 13,
            ServiceCategoryId = 5,
            Name = "Wedding Morning Glam Hair",
            Slug = "wedding-morning-glam-hair",
            ShortDescription = "Elegant bridal hairstyling with a soft luxury finish made for photos and unforgettable entrances.",
            FullDescription = "A bespoke bridal hair service tailored to the dress, mood board, veil, and wedding energy, with a refined finish that still feels modern and youthful.",
            Price = null,
            PricingType = ServicePricingType.QuoteRequired,
            DurationMinutes = 180,
            IsFeatured = true,
            IsBookableOnline = true,
            IsActive = true,
            DisplayOrder = 2,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new Service
        {
            Id = 14,
            ServiceCategoryId = 5,
            Name = "Matric Dance Signature Glam",
            Slug = "matric-dance-signature-glam",
            ShortDescription = "A polished hair-and-beauty look for entrance moments, photos, and all-night confidence.",
            FullDescription = "Created for matric dance and milestone celebrations, this signature glam service is youthful, elegant, and styled to feel memorable on camera and in person.",
            Price = 1200m,
            PricingType = ServicePricingType.From,
            DurationMinutes = 180,
            IsFeatured = true,
            IsBookableOnline = true,
            IsActive = true,
            DisplayOrder = 3,
            CreatedAtUtc = SeedCreatedAtUtc
        }
    ];

    public static IEnumerable<FAQ> Faqs =>
    [
        new FAQ
        {
            Id = 1,
            Question = "Do you accept walk-ins?",
            Answer = "Yes. Walk-ins are welcome, but bookings are recommended for busy periods and longer appointments.",
            Category = "Booking",
            DisplayOrder = 1,
            IsPublished = true,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new FAQ
        {
            Id = 2,
            Question = "How does the appointment request work?",
            Answer = "You submit a booking request online, and the salon follows up to confirm the final time and service details.",
            Category = "Booking",
            DisplayOrder = 2,
            IsPublished = true,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new FAQ
        {
            Id = 3,
            Question = "Do you offer bridal or special occasion services?",
            Answer = "Yes. Levels On Ice Salon offers premium styling suitable for bridal, events, and special occasion beauty needs.",
            Category = "Services",
            DisplayOrder = 3,
            IsPublished = true,
            CreatedAtUtc = SeedCreatedAtUtc
        },
        new FAQ
        {
            Id = 4,
            Question = "What makes the salon experience comfortable?",
            Answer = "Clients can enjoy free WiFi, complimentary refreshments, and a stylish atmosphere designed for comfort and confidence.",
            Category = "Visit",
            DisplayOrder = 4,
            IsPublished = true,
            CreatedAtUtc = SeedCreatedAtUtc
        }
    ];
}
