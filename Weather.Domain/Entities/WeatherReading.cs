using System;

namespace Weather.Domain.Entities
{
    public class WeatherReading
    {
        public int WeatherReadingID { get; set; }
        public int CityID { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public DateTime Date { get; set; }
    }
}