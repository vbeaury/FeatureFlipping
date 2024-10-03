using FeatureFlipping.Database.Entities;
using Microsoft.AspNetCore.Components;
using Shared.Services;
using Shared.Services.Interfaces;

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