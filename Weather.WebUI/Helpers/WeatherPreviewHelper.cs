using System.Collections.Generic;
using System.Linq;
using Weather.Domain.Abstract;
using Weather.WebUI.Models;

namespace Weather.WebUI.Helpers
{
    public class WeatherPreviewHelper
    {
        public static List<WeatherViewModel> GetForAllCities(IWeatherRepository weatherRepository, ICityRepository cityRepository)
        {
            var cities = cityRepository
                .Cities
                .ToList();

            var weatherReadings = weatherRepository
                .WeatherReadings
                .GroupBy(group => group.CityID)
                .Select(x => x
                    .OrderByDescending(g => g.Date)
                    .FirstOrDefault()
                )
                .ToList();

            var model = cities
                .Join(
                    weatherReadings,
                    city => city.Id,
                    weatherReading => weatherReading.CityID,
                    (city, weatherReading) => new WeatherViewModel(city.Id, city.Name, city.Country, weatherReading.Humidity, weatherReading.Temperature))
                .ToList();

            return model;
        }
    }
}