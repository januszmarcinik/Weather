using System.Collections.Generic;
using Weather.Domain.Entities;

namespace Weather.Domain.Abstract
{
    public interface ICityRepository
    {
        IEnumerable<City> Cities { get; }
    }
}