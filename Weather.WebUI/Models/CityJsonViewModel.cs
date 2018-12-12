namespace Weather.WebUI.Models
{
    public class CityJsonViewModel
    {
        public int _Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        public bool IsValid => _Id > 0 &&
                               string.IsNullOrEmpty(Name) == false &&
                               string.IsNullOrEmpty(Country) == false;
    }
}