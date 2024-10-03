using FeatureFlipping.Infrastructure.Database;
using FeatureFlipping.Infrastructure.Database.Entities;
using FeatureFlipping.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FeatureFlipping.Infrastructure.Services;

public class CarService : ICarService
{
    private readonly ApplicationDbContext _context;

    public CarService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Car>?> GetCarsAsync()
    {
        return await _context.Cars
            .Include(c => c.Manufacturer)
            .Where(c => c.IsActive)
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