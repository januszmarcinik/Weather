using System;
using System.Linq;
using System.Web.Http;
using Weather.Domain.Abstract;
using Weather.WebUI.Helpers;
using Weather.WebUI.Models;

namespace Weather.WebUI.Controllers
{
    [RoutePrefix("api/shared")]
    public class SharedController : ApiController
    {
        private readonly ICityRepository _cityRepository;
        private readonly IWeatherRepository _weatherRepository;

        public SharedController(ICityRepository cityRepository, IWeatherRepository weatherRepository)
        {
            _cityRepository = cityRepository;
            _weatherRepository = weatherRepository;
        }

        [HttpGet]
        [Route("weather")]
        public IHttpActionResult WeatherForAllCities()
        {
            var model = WeatherPreviewHelper.GetForAllCities(_weatherRepository, _cityRepository);
            if (model.Count == 0)
            {
                return NotFound();
            }

            return Ok(model);
        }

        [HttpGet]
        [Route("weather/{cityId}")]
        public IHttpActionResult WeatherForCity(int cityId)
        {
            var city = _cityRepository.Cities
                .SingleOrDefault(x => x.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var weatherReadings = _weatherRepository.WeatherReadings
                .Where(x => x.CityID == cityId)
                .OrderByDescending(x => x.Date)
                .Select(x => new CityWeatherHistogramViewModel.WeatherReadingViewModel(x.Date, x.Humidity, x.Temperature))
                .ToList();

            if (weatherReadings.Count == 0)
            {
                return NotFound();
            }

            var model = new CityWeatherHistogramViewModel(cityId, city.Name, city.Country, weatherReadings);

            return Ok(model);
        }
    }
}
