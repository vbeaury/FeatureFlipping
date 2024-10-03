using FeatureFlipping.Infrastructure.Database.Entities;
using FeatureFlipping.Infrastructure.Services;
using FeatureFlipping.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FeatureFlipping.Client.Pages;

public partial class CarList : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ICarService CarService { get; set; } = null!;
    [Inject] private FeatureService FeatureService { get; set; } = null!;
    
    private List<Car>? _cars;
    private List<Car>? _allCars;
    private bool _isLicensePlateFeatureEnabled;
    private bool _isCarVisibilityFeatureEnabled;

    protected override async Task OnInitializedAsync()
    {
        _allCars = await CarService.GetCarsAsync();
        _cars = _allCars;

        var isLicensePlateFeatureEnabled = await FeatureService.IsFeatureEnabledAsync("license-plate");

        if (isLicensePlateFeatureEnabled != _isLicensePlateFeatureEnabled)
        {
            _isLicensePlateFeatureEnabled = isLicensePlateFeatureEnabled;
            StateHasChanged();
        }
        
        var isCarVisibilityFeatureEnabled = await FeatureService.IsFeatureEnabledAsync("car-visibility");

        if (isCarVisibilityFeatureEnabled != _isCarVisibilityFeatureEnabled)
        {
            _isCarVisibilityFeatureEnabled = isCarVisibilityFeatureEnabled;
            StateHasChanged();
        }
    }
    
    private void ApplyFilter(ChangeEventArgs e)
    {
        var filter = e.Value?.ToString();

        if (filter == null)
        {
            _cars = _allCars;
        }
        else
        {
            _cars = string.IsNullOrWhiteSpace(filter) ? 
                _allCars : 
                _allCars?.Where(c => c.LicensePlate?.Contains(filter, StringComparison.OrdinalIgnoreCase) == true)
                    .ToList();
        }
        
        StateHasChanged();
    }

    private async Task HandleHideCar(int carId)
    {
        await CarService.HideCarAsync(carId);
        NavigationManager.Refresh();
    }
}