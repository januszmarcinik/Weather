using System.Collections.Generic;
using Weather.Domain.Abstract;
using Weather.Domain.Entities;

namespace Weather.Domain.Concrete
{
    public class EFCityRepository : ICityRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<City> Cities
        {
            get { return context.Cities; }
        }
    }
}