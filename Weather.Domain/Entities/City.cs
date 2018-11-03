using System.Collections.Generic;

namespace Weather.Domain.Entities
{
    public class City
    {
        public City()
        {
            this.Users = new HashSet<User>();
        }

        public int CityID { get; set; }
        public int _Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}