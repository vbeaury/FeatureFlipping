using FeatureFlipping.Infrastructure.Database.Entities;
using FeatureFlipping.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FeatureFlipping.Client.Pages;

public partial class CarList : ComponentBase
{
    [Inject] private ICarService CarService { get; set; } = null!;
    
    private List<Car>? _cars;

    protected override async Task OnInitializedAsync()
    {
        _cars = await CarService.GetCarsAsync();
    }
}