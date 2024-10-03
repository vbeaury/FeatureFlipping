using FeatureFlipping.Components;
using FeatureFlipping.Infrastructure.Database;
using FeatureFlipping.Infrastructure.Services;
using FeatureFlipping.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using _Imports = FeatureFlipping.Client._Imports;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var connectionString = builder.Configuration.GetConnectionString("AppConfig");

builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(connectionString);
    options.UseFeatureFlags(featureFlagOptions =>
        featureFlagOptions.CacheExpirationInterval = TimeSpan.FromSeconds(10));
});

builder.Services.AddAzureAppConfiguration();

builder.Services.AddFeatureManagement();

builder.Services.AddScoped<FeatureService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddSingleton<IFeatureFlagObserver, FeatureFlagObserver>();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    DbInitializer.Initialize(context);
}

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
var observer = app.Services.GetRequiredService<IFeatureFlagObserver>();

Task.Run(async () =>
{
    using var scope = scopeFactory.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Surveiller le feature flag "car-visibility" et mettre à jour les voitures
    await observer.MonitorFeatureFlagAsync("car-visibility", async (isEnabled) =>
    {
        if (isEnabled)
        {
            // Activer toutes les voitures
            var cars = await context.Cars.ToListAsync();
            cars.ForEach(car => car.IsActive = true);
            await context.SaveChangesAsync();
        }
        else
        {
            // Désactiver toutes les voitures
            var cars = await context.Cars.ToListAsync();
            cars.ForEach(car => car.IsActive = false);
            await context.SaveChangesAsync();
        }
    });
});




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAzureAppConfiguration();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(_Imports).Assembly);

app.Run();