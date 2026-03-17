using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LevelsOnIceSalon.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefreshServiceSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "High-gloss nail artistry for soft luxury girls, statement sets, and polished everyday confidence.");

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Name", "Slug" },
                values: new object[] { "Effortless glam, soft luxury styling, and event-ready looks with a premium salon finish.", "Hairstyles", "hairstyles" });

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name", "Slug" },
                values: new object[] { "Protective styling that feels neat, feminine, trend-aware, and beautifully camera-ready.", "Braids & Protective Styles", "braids-protective-styles" });

            migrationBuilder.InsertData(
                table: "ServiceCategories",
                columns: new[] { "Id", "CreatedAtUtc", "Description", "DisplayOrder", "IsActive", "IsDeleted", "Name", "PublishedAtUtc", "Slug", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Finishing touches that elevate your appointment from simple upkeep to a full beauty moment.", 4, true, false, "Beauty Add-ons", null, "beauty-add-ons", null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Elegant beauty styling for weddings, celebrations, shoots, graduations, and unforgettable entrances.", 5, true, false, "Bridal / Special Occasion", null, "bridal-special-occasion", null }
                });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DurationMinutes", "FullDescription", "Name", "Price", "ShortDescription", "Slug" },
                values: new object[] { 80, "A premium gel manicure with precision prep, shaping, cuticle care, and a glossy salon finish that feels polished, soft, and photo-ready.", "Gloss Theory Gel Set", 320m, "A sleek gel finish for girls who love clean, expensive-looking nails every day.", "gloss-theory-gel-set" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DurationMinutes", "FullDescription", "Name", "Price", "ShortDescription", "Slug" },
                values: new object[] { 140, "Our statement acrylic set is built for noticeable beauty with sculpted shape, length customization, and a crisp, luxe finish that photographs beautifully.", "Iced-Out Acrylic Signature Set", 520m, "A bold acrylic look with shape, length, and attitude for girls who love a standout finish.", "iced-out-acrylic-signature-set" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DisplayOrder", "DurationMinutes", "FullDescription", "IsFeatured", "Name", "Price", "PricingType", "ServiceCategoryId", "ShortDescription", "Slug" },
                values: new object[] { 3, 90, "Perfect for maintaining a polished acrylic or gel look, this refill includes reshaping, balancing, and a refined finish that keeps your set looking elevated.", false, "French Girl Refill", 380m, 2, 1, "A fresh refill for clients who love soft luxury nails with a clean designer finish.", "french-girl-refill" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DisplayOrder", "FullDescription", "Name", "Price", "ShortDescription", "Slug" },
                values: new object[] { 1, "This silk press service is designed for sleek movement, healthy shine, and a refined finish that feels expensive without looking overdone.", "Silk Press & Sway Finish", 650m, "A smooth, bouncy finish with movement, shine, and soft luxury energy.", "silk-press-sway-finish" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DisplayOrder", "FullDescription", "IsFeatured", "Name", "Price", "ServiceCategoryId", "ShortDescription", "Slug" },
                values: new object[] { 2, "A styled finish built around volume, shape, and softness for clients who want occasion-ready hair that still feels modern and wearable.", true, "Soft Glam Curls Styling", 480m, 2, "Defined curls and a glossy finish for birthdays, dinners, graduations, and pretty-girl weekends.", "soft-glam-curls-styling" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "CreatedAtUtc", "DisplayOrder", "DurationMinutes", "FullDescription", "IsActive", "IsBookableOnline", "IsDeleted", "IsFeatured", "Name", "Price", "PricingType", "PublishedAtUtc", "ServiceCategoryId", "ShortDescription", "Slug", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 300, "A client-favorite protective style with a clean parting pattern, natural fall, and a polished result that feels stylish from day one.", true, true, false, true, "Knotless Braids, Mid-Back", 950m, 2, null, 3, "Neat, lightweight knotless braids with a soft feminine finish and everyday luxury feel.", "knotless-braids-mid-back", null },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 360, "This premium boho knotless style blends neat structure with soft loose detail, making it ideal for clients who want protective styling with movement and personality.", true, true, false, true, "Boho Knotless Luxe", 1350m, 2, null, 3, "A soft, romantic protective style with loose textured pieces for an elevated look.", "boho-knotless-luxe", null },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 120, "A neat protective style with detail-focused parting and a sleek finish that works beautifully for everyday wear, holidays, and active weeks.", true, true, false, false, "Sleek Cornrow Design", 450m, 2, null, 3, "A clean braided look with precise lines for sporty-chic, low-maintenance beauty.", "sleek-cornrow-design", null },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 35, "A quick beauty add-on that shapes, tidies, and softly defines the brows for a more put-together finish.", true, true, false, false, "Brow Clean-Up & Tint", 180m, 1, null, 4, "A polished brow refresh that sharpens your whole face in under an hour.", "brow-clean-up-tint", null },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 60, "A lightweight glam makeup add-on designed to enhance features and complete your salon look without needing a full separate appointment.", true, true, false, true, "Soft Glam Face Beat Add-On", 420m, 2, null, 4, "A camera-friendly glam touch-up for clients pairing hair and beauty in one visit.", "soft-glam-face-beat-add-on", null },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 20, "An easy beauty add-on that adds eye definition and softness, ideal before events, dates, birthdays, and content days.", true, true, false, false, "Lash Flick Finish", 150m, 1, null, 4, "A flirty finishing touch for girls who want their salon look to feel complete.", "lash-flick-finish", null },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 150, "A bridal planning appointment focused on trying the look, refining the finish, and building confidence around your final beauty direction.", true, true, false, true, "Bridal Preview Styling Session", 950m, 2, null, 5, "A consultation-led preview for brides who want to feel fully sure before the big day.", "bridal-preview-styling-session", null },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 180, "A bespoke bridal hair service tailored to the dress, mood board, veil, and wedding energy, with a refined finish that still feels modern and youthful.", true, true, false, true, "Wedding Morning Glam Hair", null, 3, null, 5, "Elegant bridal hairstyling with a soft luxury finish made for photos and unforgettable entrances.", "wedding-morning-glam-hair", null },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 180, "Created for matric dance and milestone celebrations, this signature glam service is youthful, elegant, and styled to feel memorable on camera and in person.", true, true, false, true, "Matric Dance Signature Glam", 1200m, 2, null, 5, "A polished hair-and-beauty look for entrance moments, photos, and all-night confidence.", "matric-dance-signature-glam", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Premium nail artistry, statement sets, and polished everyday finishes.");

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Name", "Slug" },
                values: new object[] { "Youthful, stylish, and occasion-ready hairstyles tailored to your look.", "Custom Hairstyles", "custom-hairstyles" });

            migrationBuilder.UpdateData(
                table: "ServiceCategories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name", "Slug" },
                values: new object[] { "Beauty finishing services that complete a polished salon visit.", "Beauty Services", "beauty-services" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DurationMinutes", "FullDescription", "Name", "Price", "ShortDescription", "Slug" },
                values: new object[] { 75, "A long-lasting gel manicure with prep, shaping, cuticle care, and a polished premium finish.", "Luxury Gel Manicure", 280m, "Glossy, premium gel finish designed for clean everyday confidence.", "luxury-gel-manicure" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DurationMinutes", "FullDescription", "Name", "Price", "ShortDescription", "Slug" },
                values: new object[] { 120, "Acrylic set with custom shaping and a statement-ready finish ideal for events and polished everyday style.", "Statement Acrylic Set", 450m, "Bold acrylic styling for clients who want a standout after-look.", "statement-acrylic-set" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DisplayOrder", "DurationMinutes", "FullDescription", "IsFeatured", "Name", "Price", "PricingType", "ServiceCategoryId", "ShortDescription", "Slug" },
                values: new object[] { 1, 180, "A consultation-led bridal hairstyle designed around the dress, event mood, and desired finish.", true, "Custom Bridal Hairstyling", null, 3, 2, "Elegant bridal-ready styling with a soft luxury finish.", "custom-bridal-hairstyling" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DisplayOrder", "FullDescription", "Name", "Price", "ShortDescription", "Slug" },
                values: new object[] { 2, "A custom style for birthdays, celebrations, shoots, and events with a youthful but refined result.", "Occasion Hairstyling", 550m, "Soft glam and event-ready hairstyling for special moments.", "occasion-hairstyling" });

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DisplayOrder", "FullDescription", "IsFeatured", "Name", "Price", "ServiceCategoryId", "ShortDescription", "Slug" },
                values: new object[] { 1, "A beauty service focused on enhancing features with a soft glam, camera-friendly result.", false, "Soft Glam Makeup", 650m, 3, "A polished, feminine finish suitable for both casual and premium events.", "soft-glam-makeup" });
        }
    }
}
