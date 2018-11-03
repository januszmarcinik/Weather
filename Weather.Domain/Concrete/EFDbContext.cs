using System.Data.Entity;
using Weather.Domain.Entities;

namespace Weather.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<WeatherReading> WeatherReadings { get; set; }
    }
}