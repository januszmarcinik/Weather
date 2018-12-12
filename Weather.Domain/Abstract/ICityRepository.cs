using System.Collections.Generic;
using Weather.Domain.Entities;

namespace Weather.Domain.Abstract
{
    public interface ICityRepository
    {
        IEnumerable<City> Cities { get; }

        City Add(int externalApiId, string name, string country);

        void Delete(int id);
    }
}