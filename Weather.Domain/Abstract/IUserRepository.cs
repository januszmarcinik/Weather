using System.Collections.Generic;
using Weather.Domain.Entities;

namespace Weather.Domain.Abstract
{
    public interface IUserRepository
    {
        IEnumerable<User> Users { get; }

        void AddCity(User user, City city);
        void DeleteCity(User user, int cityID);
        void AddUser(User user);
    }
}