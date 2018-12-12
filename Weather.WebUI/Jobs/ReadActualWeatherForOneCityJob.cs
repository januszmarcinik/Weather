using Weather.Domain.Entities;
using Weather.WebUI.Services;

namespace Weather.WebUI.Jobs
{
    public class ReadActualWeatherForOneCityJob
    {
        public ReadActualWeatherForOneCityJob(IExternalApiWeatherService externalApiWeatherService)
        {
            _externalApiWeatherService = externalApiWeatherService;
        }

        public void Run(City city)
        {
            _externalApiWeatherService.DownloadCityWeather(city);
        }

        private readonly IExternalApiWeatherService _externalApiWeatherService;
    }
}