using FeatureFlipping.Infrastructure.Services;
using Microsoft.AspNetCore.Components;

namespace FeatureFlipping.Client.Pages;

public partial class Contact : ComponentBase
{
    [Inject] public FeatureService FeatureService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        if (!await FeatureService.IsFeatureEnabledAsync("ContactForm"))
        {
            NavigationManager.NavigateTo("/");
        }
    }
}