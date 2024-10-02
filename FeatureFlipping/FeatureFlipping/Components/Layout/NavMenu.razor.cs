using Microsoft.AspNetCore.Components;
using Shared.Services;

namespace FeatureFlipping.Components.Layout;

public partial class NavMenu : ComponentBase
{
    [Inject] private FeatureService FeatureService { get; set; } = default!;
    
    private bool _isContactFeatureEnabled;
    
    protected override async Task OnInitializedAsync()
    {
        _isContactFeatureEnabled = await FeatureService.IsFeatureEnabledAsync("contact-form");
        await base.OnInitializedAsync();
    }
}