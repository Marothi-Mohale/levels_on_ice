using LevelsOnIceSalon.Web.ViewModels;

namespace LevelsOnIceSalon.Web.Services;

public class SitePageContentService : ISitePageContentService
{
    public HomePageViewModel GetHomePage()
    {
        return new HomePageViewModel
        {
            PageTitle = "Home",
            NavigationTitle = "Home",
            SalonName = "Levels On Ice Salon",
            Tagline = "Classy beauty experiences for bold, stylish women in Cape Town.",
            Intro = "A premium beauty destination designed to convert first impressions into booked appointments.",
            Purpose = "Introduce the brand, establish trust, showcase the salon experience, and move visitors into gallery exploration or booking.",
            Address = "102 Main Road, Mowbray, Cape Town",
            PhoneNumber = "081 390 6634",
            Email = "levelsonicegroup@gmail.com",
            FeaturedTestimonials =
            [
                new TestimonialCardViewModel
                {
                    CustomerName = "Zinhle, Cape Town",
                    Quote = "My nails looked expensive, clean, and feminine. I left feeling like the best version of myself.",
                    Rating = 5,
                    ServiceName = "Gloss Theory Gel Set",
                    IsFeatured = true,
                    SourceLabel = "Client review"
                },
                new TestimonialCardViewModel
                {
                    CustomerName = "Anele, Mowbray",
                    Quote = "The hairstyle was soft glam perfection. It held beautifully and looked incredible in every photo.",
                    Rating = 5,
                    ServiceName = "Soft Glam Curls Styling",
                    IsFeatured = true,
                    SourceLabel = "Client review"
                },
                new TestimonialCardViewModel
                {
                    CustomerName = "Kayla, Observatory",
                    Quote = "You can feel the quality in the details. The salon is stylish, welcoming, and the results speak for themselves.",
                    Rating = 5,
                    ServiceName = "Boho Knotless Luxe",
                    IsFeatured = true,
                    SourceLabel = "Client review"
                }
            ],
            PrimaryCta = new CallToActionViewModel
            {
                Label = "Request Appointment",
                Url = "/book-appointment",
                SupportingText = "Encourage visitors to start the booking journey quickly."
            },
            DataSource = "Site settings, featured services, featured gallery images, testimonials, and contact details.",
            SeoIntent = "Cape Town salon homepage targeting branded search, local beauty salon queries, and premium nails and hairstyles intent.",
            Sections =
            [
                new HomeSectionPlanViewModel
                {
                    SectionId = "hero",
                    Name = "Hero",
                    Purpose = "Create a premium first impression, communicate brand positioning, and present the primary booking CTA immediately.",
                    ContentBlocks =
                    [
                        new ContentBlockViewModel { Title = "Brand statement", Description = "Concise positioning message for stylish, premium salon experiences." },
                        new ContentBlockViewModel { Title = "Supporting proof", Description = "Amenities, salon atmosphere, and quick contact cues." },
                        new ContentBlockViewModel { Title = "Primary actions", Description = "Book now and view gallery entry points." }
                    ],
                    PrimaryCta = new CallToActionViewModel { Label = "Book Now", Url = "/book-appointment", SupportingText = "Push high-intent traffic toward booking." },
                    DataSource = "Site settings and homepage hero content managed in admin.",
                    SeoIntent = "Capture branded and local transactional salon traffic."
                },
                new HomeSectionPlanViewModel
                {
                    SectionId = "why-choose-us",
                    Name = "Why Choose Us",
                    Purpose = "Build trust by showing what makes the salon experience polished, premium, and client-friendly.",
                    ContentBlocks =
                    [
                        new ContentBlockViewModel { Title = "Premium service cues", Description = "Classy finish, trend awareness, and confidence-led beauty outcomes." },
                        new ContentBlockViewModel { Title = "Client comfort", Description = "Free WiFi, refreshments, and walk-in friendliness." },
                        new ContentBlockViewModel { Title = "Occasion range", Description = "Suitable for bridal, events, and everyday maintenance." }
                    ],
                    PrimaryCta = new CallToActionViewModel { Label = "See Why Clients Love Us", Url = "/testimonials", SupportingText = "Lead visitors into credibility content." },
                    DataSource = "Static brand content and editable settings.",
                    SeoIntent = "Support engagement and trust signals for conversion-focused visitors."
                },
                new HomeSectionPlanViewModel
                {
                    SectionId = "featured-services",
                    Name = "Featured Services",
                    Purpose = "Highlight the most desirable services quickly and guide visitors into deeper service exploration.",
                    ContentBlocks =
                    [
                        new ContentBlockViewModel { Title = "Nails", Description = "Featured premium nail offerings and statement looks." },
                        new ContentBlockViewModel { Title = "Custom Hairstyles", Description = "Occasion-ready and personality-led hairstyle options." },
                        new ContentBlockViewModel { Title = "Beauty Services", Description = "Supporting beauty treatments that complete the look." }
                    ],
                    PrimaryCta = new CallToActionViewModel { Label = "Explore Services", Url = "/services", SupportingText = "Move users from homepage browsing into service consideration." },
                    DataSource = "Featured service categories and services from the database.",
                    SeoIntent = "Target service-intent searches around nails, hairstyles, and beauty services in Cape Town."
                },
                new HomeSectionPlanViewModel
                {
                    SectionId = "gallery-preview",
                    Name = "Gallery Preview",
                    Purpose = "Use finished looks to create aspiration, social proof, and visual desire.",
                    ContentBlocks =
                    [
                        new ContentBlockViewModel { Title = "Editorial grid", Description = "Curated best work with polished imagery and captions." },
                        new ContentBlockViewModel { Title = "Category previews", Description = "Nails, hairstyles, and beauty transformations." }
                    ],
                    PrimaryCta = new CallToActionViewModel { Label = "View Full Gallery", Url = "/gallery", SupportingText = "Encourage deeper visual browsing before booking." },
                    DataSource = "Published gallery images from the CMS and local salon image library.",
                    SeoIntent = "Capture inspiration-focused salon searches and improve on-site engagement."
                },
                new HomeSectionPlanViewModel
                {
                    SectionId = "client-testimonials",
                    Name = "Client Testimonials",
                    Purpose = "Reinforce quality and trust using real client experiences and satisfaction cues.",
                    ContentBlocks =
                    [
                        new ContentBlockViewModel { Title = "Featured reviews", Description = "Short, polished review snippets from happy clients." },
                        new ContentBlockViewModel { Title = "Service context", Description = "Attach reviews to nails, hairstyles, or beauty services for relevance." }
                    ],
                    PrimaryCta = new CallToActionViewModel { Label = "Read More Reviews", Url = "/testimonials", SupportingText = "Support final conversion decisions." },
                    DataSource = "Published testimonials managed in admin.",
                    SeoIntent = "Support branded trust queries and improve conversion confidence."
                },
                new HomeSectionPlanViewModel
                {
                    SectionId = "visit-us",
                    Name = "Visit Us / Contact Snapshot",
                    Purpose = "Make location, phone, and email instantly accessible for local visitors ready to act.",
                    ContentBlocks =
                    [
                        new ContentBlockViewModel { Title = "Location details", Description = "Address and local area context." },
                        new ContentBlockViewModel { Title = "Direct contact", Description = "Phone and email shortcuts." },
                        new ContentBlockViewModel { Title = "Visit cues", Description = "Walk-ins welcome and hospitality features." }
                    ],
                    PrimaryCta = new CallToActionViewModel { Label = "Contact Us", Url = "/contact", SupportingText = "Support visitors who want direct contact before booking." },
                    DataSource = "Site settings and contact details.",
                    SeoIntent = "Target local intent and support location-specific discoverability."
                },
                new HomeSectionPlanViewModel
                {
                    SectionId = "cta-banner",
                    Name = "CTA Banner",
                    Purpose = "Deliver a strong final conversion moment near the end of the homepage.",
                    ContentBlocks =
                    [
                        new ContentBlockViewModel { Title = "Urgency cue", Description = "Invitational message to secure the next beauty appointment." },
                        new ContentBlockViewModel { Title = "Simple action", Description = "Single high-contrast booking button." }
                    ],
                    PrimaryCta = new CallToActionViewModel { Label = "Book Appointment", Url = "/book-appointment", SupportingText = "Capture high-intent users at the end of the page." },
                    DataSource = "Static CTA content or editable homepage banner settings.",
                    SeoIntent = "Not primarily SEO-driven; focused on conversion completion."
                }
            ]
        };
    }

    public AboutPageViewModel GetAboutPage() =>
        new()
        {
            PageTitle = "About",
            NavigationTitle = "About",
            Intro = "Levels On Ice Salon is built for women who want beauty that feels polished, fashion-forward, and genuinely worth booking ahead for.",
            BrandStory = "The brand was shaped around a simple idea: salon visits should feel elevated from the moment you walk in to the moment you leave. Not stiff. Not intimidating. Just beautifully done, beautifully hosted, and designed for clients who love a classy finish with modern energy. Levels On Ice Salon brings together premium service, trend-aware beauty, and a warm atmosphere that makes clients feel looked after as much as they feel styled.",
            AudienceFit = "The salon speaks naturally to young women aged 15 to 35 who want everything from everyday polish to standout occasion beauty. That includes the girl booking her maintenance set, the friend getting ready for a birthday dinner, the graduate planning her entrance look, and the bride wanting soft luxury that still feels personal and current.",
            Atmosphere = "Inside the salon, the energy is stylish but welcoming. Clients can expect a clean, considered setting, a team that values detail, complimentary refreshments, free WiFi, and a beauty experience that never feels rushed. The goal is to make every appointment feel easy, personal, and quietly luxurious.",
            PrimaryCta = new CallToActionViewModel
            {
                Label = "Book Your Visit",
                Url = "/book-appointment",
                SupportingText = "Turn brand confidence into an appointment request."
            },
            DifferencePoints =
            [
                new ContentBlockViewModel
                {
                    Title = "Refined, not generic",
                    Description = "Every service is shaped to feel current, polished, and visually beautiful without slipping into cookie-cutter salon styling."
                },
                new ContentBlockViewModel
                {
                    Title = "Premium with warmth",
                    Description = "Clients get quality, professionalism, and attention to detail in a space that still feels welcoming, relaxed, and easy to return to."
                },
                new ContentBlockViewModel
                {
                    Title = "Built for real-life beauty moments",
                    Description = "From soft everyday maintenance to bridal, matric, and special occasions, the salon experience fits the rhythm of modern women."
                }
            ],
            TeamMembers =
            [
                new TeamMemberProfileViewModel
                {
                    Name = "Levels On Ice Creative Team",
                    Role = "Salon Beauty Specialists",
                    Bio = "A style-aware team focused on polished finishes, client comfort, and details that make every after-look feel elevated.",
                    Specialty = "Nails, occasion styling, protective looks, and beauty finishing touches"
                }
            ]
        };

    public PageBlueprintViewModel GetServicesPage() =>
        CreatePage(
            "Services",
            "Services",
            "Present all core service categories clearly so visitors can compare options and move toward booking.",
            "This page should make it easy to discover nails, custom hairstyles, and beauty services with enough context to support confident decisions.",
            "/book-appointment",
            "Request a Service Booking",
            "Service categories, services, pricing-from values, durations, and featured flags from the database.",
            "Target transactional service searches for nails, hairstyles, and beauty salon services in Cape Town.",
            [
                new ContentBlockViewModel { Title = "Service Categories", Description = "Clearly separated service groups for scanning and comparison." },
                new ContentBlockViewModel { Title = "Service Cards", Description = "Name, short description, pricing-from, and duration where available." },
                new ContentBlockViewModel { Title = "Service CTA", Description = "Direct path into booking the selected service." }
            ]);

    public PageBlueprintViewModel GetGalleryPage() =>
        CreatePage(
            "Gallery",
            "Gallery",
            "Showcase the salon's best finished looks to create aspiration, prove quality, and reflect a social-first brand.",
            "This page should act as the visual proof hub, with curated images organized for easy browsing and strong emotional impact.",
            "/book-appointment",
            "Book the Look",
            "Published gallery images, categories, captions, and alt text from the database plus local salon image assets.",
            "Target image- and inspiration-focused salon queries plus branded gallery searches.",
            [
                new ContentBlockViewModel { Title = "Featured Looks", Description = "Top-performing or signature transformations." },
                new ContentBlockViewModel { Title = "Category Filters", Description = "Nails, hairstyles, and beauty image groupings." },
                new ContentBlockViewModel { Title = "Caption Layer", Description = "Short style notes and service relevance." }
            ]);

    public PageBlueprintViewModel GetTestimonialsPage() =>
        CreatePage(
            "Testimonials",
            "Testimonials",
            "Use customer voice to reduce hesitation and strengthen trust before booking.",
            "This page should reassure new visitors through relatable feedback, service-specific praise, and polished social proof.",
            "/book-appointment",
            "Book With Confidence",
            "Published testimonials, ratings, service names, and featured flags from the database.",
            "Target trust-oriented branded search and support conversion for undecided visitors.",
            [
                new ContentBlockViewModel { Title = "Featured Reviews", Description = "Best testimonials displayed prominently." },
                new ContentBlockViewModel { Title = "Service-Based Reviews", Description = "Group praise by nails, hair, or beauty relevance." },
                new ContentBlockViewModel { Title = "Trust Cues", Description = "Rating context and recurring positive themes." }
            ]);

    public PageBlueprintViewModel GetFaqsPage() =>
        CreatePage(
            "FAQs",
            "FAQs",
            "Answer common booking, walk-in, policy, and service questions before they become friction points.",
            "This page should reduce uncertainty, save staff time, and support confident booking decisions through clear answers.",
            "/book-appointment",
            "Still Ready to Book?",
            "Published FAQ items and categories from the database.",
            "Target informational local salon queries and rich snippet opportunities around policies and bookings.",
            [
                new ContentBlockViewModel { Title = "Booking Questions", Description = "Availability, appointments, and how the request process works." },
                new ContentBlockViewModel { Title = "Visit Questions", Description = "Walk-ins, timing, and what to expect at the salon." },
                new ContentBlockViewModel { Title = "Policy Questions", Description = "Cancellations, service preparation, and practical guidance." }
            ]);

    public PageBlueprintViewModel GetContactPage() =>
        CreatePage(
            "Contact",
            "Contact",
            "Give local visitors a fast way to call, email, locate, or visit the salon.",
            "This page should act as the practical action page for directions, direct communication, and location confidence.",
            "/book-appointment",
            "Book an Appointment",
            "Site settings for address, contact details, opening hours, map embed, and optional contact form data.",
            "Target local salon contact and location-intent searches in Mowbray and Cape Town.",
            [
                new ContentBlockViewModel { Title = "Contact Details", Description = "Phone number, email, and quick actions." },
                new ContentBlockViewModel { Title = "Location Block", Description = "Address and map support." },
                new ContentBlockViewModel { Title = "Visit Information", Description = "Walk-in welcome cues and practical visit notes." }
            ]);

    public PageBlueprintViewModel GetBookAppointmentPage() =>
        CreatePage(
            "Book Appointment",
            "Book Appointment",
            "Convert interested visitors into appointment requests with a simple, trustworthy, mobile-friendly form.",
            "This page should focus on clarity, low friction, and reassurance while collecting the details needed for admin follow-up.",
            "/contact",
            "Need Help First?",
            "Booking request form fields, service list, and contact details from the database and site settings.",
            "Target high-intent booking and appointment-request queries for salon services in Cape Town.",
            [
                new ContentBlockViewModel { Title = "Booking Form", Description = "Name, contact, service, preferred date and notes." },
                new ContentBlockViewModel { Title = "Expectation Setting", Description = "Explain that this is an appointment request pending confirmation." },
                new ContentBlockViewModel { Title = "Contact Backup", Description = "Phone and email options for visitors who prefer direct contact." }
            ]);

    private static PageBlueprintViewModel CreatePage(
        string pageTitle,
        string navigationTitle,
        string purpose,
        string intro,
        string ctaUrl,
        string ctaLabel,
        string dataSource,
        string seoIntent,
        IList<ContentBlockViewModel> contentBlocks)
    {
        return new PageBlueprintViewModel
        {
            PageTitle = pageTitle,
            NavigationTitle = navigationTitle,
            Purpose = purpose,
            Intro = intro,
            PrimaryCta = new CallToActionViewModel
            {
                Label = ctaLabel,
                Url = ctaUrl,
                SupportingText = "Primary action for this page."
            },
            DataSource = dataSource,
            SeoIntent = seoIntent,
            ContentBlocks = contentBlocks
        };
    }
}
