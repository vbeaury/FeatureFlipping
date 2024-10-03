using FeatureFlipping.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace FeatureFlipping.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : DbContext(options)
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
}