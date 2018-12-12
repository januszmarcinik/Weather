using System;
using System.Linq;
using System.Web.Mvc;
using Weather.Domain.Abstract;

namespace Weather.WebUI.Controllers
{
    public class MyCustomController : Controller
    {
        protected bool IsWeatherActual = true;
        private readonly IWeatherRepository _weatherRepository;

        public MyCustomController(IWeatherRepository weatherRepo)
        {
            _weatherRepository = weatherRepo;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var latestWeatherReading = _weatherRepository.WeatherReadings.OrderByDescending(x => x.Date).FirstOrDefault()?.Date;
            if (latestWeatherReading.HasValue == false)
            {
                IsWeatherActual = false;
                return;
            }

            var dateNow = DateTime.Now;
            var timeSpan = dateNow - latestWeatherReading.Value;
            if (timeSpan.TotalMinutes > 10)
            {
                IsWeatherActual = false;
            }
        }
    }
}