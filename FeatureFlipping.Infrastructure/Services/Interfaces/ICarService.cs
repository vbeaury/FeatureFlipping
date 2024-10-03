using FeatureFlipping.Infrastructure.Database.Entities;

namespace FeatureFlipping.Infrastructure.Services.Interfaces;

public interface ICarService
{
    Task<List<Car>?> GetCarsAsync();
}