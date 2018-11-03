using System;
using System.Linq;
using System.Web.Mvc;
using Weather.Domain.Entities;
using Weather.Domain.Abstract;
using System.Web.Routing;

namespace Weather.WebUI.Controllers
{
    public class MyCustomController : Controller
    {
        protected bool IsWeatherActual = true;
        private IWeatherRepository weatherRepository;

        public MyCustomController(IWeatherRepository weatherRepo)
        {
            weatherRepository = weatherRepo;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if ((User)Session["User"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "User", action = "Login" }));
            }
            else
            {
                User user = (User)Session["User"];
                ViewBag.UserName = user.Login;
            }

            DateTime latestWeatherReading = weatherRepository.WeatherReadings.OrderByDescending(x => x.Date).FirstOrDefault().Date;
            DateTime dateNow = DateTime.Now;
            TimeSpan timeSpan = dateNow - latestWeatherReading;
            if (timeSpan.TotalMinutes > 10)
                IsWeatherActual = false;
        }
    }
}