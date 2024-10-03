using FeatureFlipping.Database.Entities;

namespace Shared.Services.Interfaces;

public interface ICarService
{
    Task<List<Car>?> GetCarsAsync();
}