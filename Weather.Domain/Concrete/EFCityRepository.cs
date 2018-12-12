using System.Collections.Generic;
using System.Linq;
using Weather.Domain.Abstract;
using Weather.Domain.Entities;

namespace Weather.Domain.Concrete
{
    public class EFCityRepository : ICityRepository
    {
        private readonly EFDbContext _context = new EFDbContext();

        public IEnumerable<City> Cities => _context.Cities;

        public City Add(int externalApiId, string name, string country)
        {
            var city = new City
            {
                ExternalApiId = externalApiId,
                Name = name,
                Country = country
            };
            _context.Cities.Add(city);
            _context.SaveChanges();

            return city;
        }

        public void Delete(int id)
        {
            var city = _context.Cities.SingleOrDefault(x => x.Id == id);
            if (city == null) return;

            _context.Cities.Remove(city);
            _context.SaveChanges();
        }
    }
}