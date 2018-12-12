using System;
using System.Net;
using System.Web.Script.Serialization;
using Weather.Domain.Abstract;
using Weather.Domain.Entities;
using Weather.WebUI.Models;

namespace Weather.WebUI.Services
{
    public class OpenWeatherMapService : IExternalApiWeatherService
    {
        private readonly IWeatherRepository _weatherRepository;

        public OpenWeatherMapService(IWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }

        public void DownloadCityWeather(City city)
        {
            var url = $"http://api.openweathermap.org/data/2.5/weather?APPID=75a3e7d73376fa1ae08a542ca103899a&id={city.ExternalApiId}";
            using (var webClient = new WebClient())
            {
                var json = webClient.DownloadString(url);
                var jsonModel = new JavaScriptSerializer().Deserialize<JsonViewModel>(json);
                var weather = new WeatherReading
                {
                    CityID = city.Id,
                    Temperature = jsonModel.Main.Temp,
                    Humidity = jsonModel.Main.Humidity,
                    Date = DateTime.Now
                };
                _weatherRepository.AddWeatherReading(weather);
            }
        }
    }
}