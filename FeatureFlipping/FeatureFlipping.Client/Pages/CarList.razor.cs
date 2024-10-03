using FeatureFlipping.Infrastructure.Database.Entities;
using FeatureFlipping.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FeatureFlipping.Client.Pages;

public partial class CarList : ComponentBase
{
    [Inject] private ICarService CarService { get; set; } = null!;
    
    private List<Car>? _cars;
    private List<Car>? _allCars;

    protected override async Task OnInitializedAsync()
    {
        _allCars = await CarService.GetCarsAsync();
        _cars = _allCars;
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
}