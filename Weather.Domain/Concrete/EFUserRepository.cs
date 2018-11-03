using System;
using System.Collections.Generic;
using Weather.Domain.Abstract;
using Weather.Domain.Entities;
using System.Linq;

namespace Weather.Domain.Concrete
{
    public class EFUserRepository : IUserRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<User> Users
        {
            get { return context.Users; }
        }

        public void AddCity(User user, City city)
        {
            User dbEntry = context.Users.FirstOrDefault(x => x.UserID == user.UserID);
            dbEntry.Cities.Add(city);
            context.SaveChanges();
        }

        public void DeleteCity(User user, int cityID)
        {
            user.Cities.Remove(user.Cities.FirstOrDefault(x => x.CityID == cityID));
            context.SaveChanges();
        }

        public void AddUser(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}