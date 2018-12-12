using System.Linq;
using Weather.Domain.Abstract;
using Weather.WebUI.Services;

namespace Weather.WebUI.Jobs
{
    public class ReadActualWeatherForAllCitiesJob
    {
        public ReadActualWeatherForAllCitiesJob(ICityRepository cityRepository, IExternalApiWeatherService externalApiWeatherService)
        {
            _cityRepository = cityRepository;
            _externalApiWeatherService = externalApiWeatherService;
        }

        public void Run()
        {
            var cities = _cityRepository
                .Cities
                .ToList();

            cities.ForEach(_externalApiWeatherService.DownloadCityWeather);
        }

        private readonly ICityRepository _cityRepository;
        private readonly IExternalApiWeatherService _externalApiWeatherService;
    }
}