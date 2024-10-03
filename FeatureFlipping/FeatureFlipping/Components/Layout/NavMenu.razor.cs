using FeatureFlipping.Infrastructure.Services;
using Microsoft.AspNetCore.Components;

namespace FeatureFlipping.Components.Layout;

public partial class NavMenu : ComponentBase
{
    [Inject] private FeatureService FeatureService { get; set; } = default!;
    
    private bool _isContactFeatureEnabled;
    
    protected override async Task OnInitializedAsync()
    {
        var contactFeatureEnabled = await FeatureService.IsFeatureEnabledAsync("contact-form");
        
        if (contactFeatureEnabled != _isContactFeatureEnabled)
        {
            _isContactFeatureEnabled = contactFeatureEnabled;
            StateHasChanged();
        }
        
        await base.OnInitializedAsync();
    }
}