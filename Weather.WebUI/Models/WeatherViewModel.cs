namespace Weather.WebUI.Models
{
    public class WeatherViewModel
    {
        public int CityID { get; set; }
        public string CityName { get; set; }
        public string Country { get; set; }
        public int Humidity { get; set; }
        public double Temperature { get; set; }
    }
}