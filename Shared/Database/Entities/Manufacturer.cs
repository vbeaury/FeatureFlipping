namespace FeatureFlipping.Database.Entities;

public class Manufacturer
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Country { get; set; }

    // Navigation Property
    public ICollection<Car> Cars { get; set; } = new List<Car>();
}