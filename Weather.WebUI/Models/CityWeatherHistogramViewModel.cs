using System;
using System.Collections.Generic;

namespace Weather.WebUI.Models
{
    public class CityWeatherHistogramViewModel
    {
        public CityWeatherHistogramViewModel(int id, string name, string country, List<WeatherReadingViewModel> histogram)
        {
            Id = id;
            Name = name;
            Country = country;
            Histogram = histogram;
        }

        public int Id { get; }
        public string Name { get; }
        public string Country { get; }
        public List<WeatherReadingViewModel> Histogram { get; }

        public class WeatherReadingViewModel
        {
            public WeatherReadingViewModel(DateTime date, int humidity, double temperature)
            {
                Date = date;
                Humidity = humidity;
                Temperature = Math.Round(temperature - 273.15, 0);
            }

            public DateTime Date { get; }
            public int Humidity { get; }
            public double Temperature { get; }
        }
    }
}