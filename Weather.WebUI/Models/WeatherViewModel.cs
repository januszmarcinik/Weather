using System;

namespace Weather.WebUI.Models
{
    public class WeatherViewModel
    {
        public WeatherViewModel(int cityId, string cityName, string country, int humidity, double temperature)
        {
            CityID = cityId;
            CityName = cityName;
            Country = country;
            Humidity = humidity;
            Temperature = Math.Round(temperature - 273.15, 0);
        }

        public int CityID { get; }
        public string CityName { get; }
        public string Country { get; }
        public int Humidity { get; }
        public double Temperature { get; }
    }
}