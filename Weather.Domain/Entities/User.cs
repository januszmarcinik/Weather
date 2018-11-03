using System.Collections.Generic;

namespace Weather.Domain.Entities
{
    public class User
    {
        public User()
        {
            this.Cities = new HashSet<City>();
        }

        public int UserID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}