using LevelsOnIceSalon.Web.Data;
using LevelsOnIceSalon.Infrastructure.DependencyInjection;
using LevelsOnIceSalon.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddResponseCaching();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseResponseCaching();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await SeedData.InitializeAsync(app.Services);

app.Run();
