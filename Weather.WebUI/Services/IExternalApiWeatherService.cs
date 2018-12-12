using Weather.Domain.Entities;

namespace Weather.WebUI.Services
{
    public interface IExternalApiWeatherService
    {
        void DownloadCityWeather(City city);
    }
}