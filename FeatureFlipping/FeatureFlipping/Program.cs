using FeatureFlipping.Client.Pages;
using FeatureFlipping.Components;
using FeatureFlipping.Services;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var connectionString = builder.Configuration.GetConnectionString("AppConfig");

builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(connectionString)
        .Select("TestApp:*", LabelFilter.Null)
        .ConfigureRefresh(refreshOptions =>
            refreshOptions.Register("TestApp:Settings:Sentinel", refreshAll: true));
    options.UseFeatureFlags();
});

builder.Services.AddAzureAppConfiguration();

builder.Services.AddFeatureManagement();
builder.Services.AddScopedFeatureManagement();

builder.Services.AddScoped<FeatureService>();

var app = builder.Build();

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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(FeatureFlipping.Client._Imports).Assembly);

app.Run();