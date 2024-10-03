using FeatureFlipping.Infrastructure.Database;
using FeatureFlipping.Infrastructure.Database.Entities;
using FeatureFlipping.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FeatureFlipping.Infrastructure.Services;

public class CarService : ICarService
{
    private readonly ApplicationDbContext _context;
    private readonly FeatureService _featureService;

    public CarService(ApplicationDbContext context, FeatureService featureService)
    {
        _context = context;
        _featureService = featureService;
    }

    public async Task<List<Car>?> GetCarsAsync()
    {
        var isFeatureCarVisibleEnabled = await _featureService.IsFeatureEnabledAsync("car-visibility");
        
        return await _context.Cars
            .Include(c => c.Manufacturer)
            .Where(c => !isFeatureCarVisibleEnabled || c.IsActive)
            .ToListAsync();
    }

    public async Task HideCarAsync(int carId)
    {
        var car = await _context.Cars.FindAsync(carId);
        
        if (car is not null)
        {
            car.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
}