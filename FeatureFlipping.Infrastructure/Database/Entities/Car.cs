namespace FeatureFlipping.Infrastructure.Database.Entities;

public class Car
{
    public int Id { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }
    public int Year { get; set; }

    // Foreign Key
    public int ManufacturerId { get; set; }
    public Manufacturer Manufacturer { get; set; } = null!;
}