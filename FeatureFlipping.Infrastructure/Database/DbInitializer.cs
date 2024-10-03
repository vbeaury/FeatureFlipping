using FeatureFlipping.Infrastructure.Database.Entities;

namespace FeatureFlipping.Infrastructure.Database;

public class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (context.Manufacturers.Any())
        {
            return;
        }

        var manufacturers = new Manufacturer[]
        {
            new() { Name = "Toyota", Country = "Japan" },
            new() { Name = "Ford", Country = "USA" },
            new() { Name = "BMW", Country = "Germany" }
        };

        context.Manufacturers.AddRange(manufacturers);
        context.SaveChanges();

        var cars = new Car[]
        {
            new() { Model = "Corolla", Color = "Red", Year = 2020, LicensePlate = "ABC123", ManufacturerId = manufacturers[0].Id },
            new() { Model = "Mustang", Color = "Blue", Year = 2019, LicensePlate = "XYZ789", ManufacturerId = manufacturers[1].Id },
            new() { Model = "X5", Color = "Black", Year = 2021, LicensePlate = "LMN456", ManufacturerId = manufacturers[2].Id }
        };

        context.Cars.AddRange(cars);
        context.SaveChanges();
    }
}