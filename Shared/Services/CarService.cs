using FeatureFlipping.Database;
using FeatureFlipping.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Services.Interfaces;

namespace Shared.Services;

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
            .Include(c => c.Manufacturer) // Inclure le fabricant
            .ToListAsync();
    }
}