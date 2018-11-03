using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using Weather.WebUI.Models;
using Weather.Domain.Entities;
using Weather.Domain.Abstract;
using System.Web.Routing;
using System.Net;

namespace Weather.WebUI.Controllers
{
    public class CityController : MyCustomController
    {
        private IUserRepository userRepository;
        private IWeatherRepository weatherRepository;

        public CityController(IUserRepository userRepo, IWeatherRepository weatherRepo) : base(weatherRepo)
        {
            userRepository = userRepo;
            weatherRepository = weatherRepo;
        }

        public ActionResult SearchCity(string search = null)
        {
            if (string.IsNullOrEmpty(search))
                return RedirectToAction("Index", "Weather");

            ViewBag.SearchCity = search;
            List<City> SearchCities = new List<City>();
            foreach (var city in GetAllCities())
            {
                if (city.Name.ToLower().Contains(search.ToLower()))
                    SearchCities.Add(city);
            }
            return View(SearchCities);
        }

        public ActionResult AddNewCity(string name, string country, int id)
        {
            if ((string.IsNullOrEmpty(name)) || (string.IsNullOrEmpty(country)))
                return RedirectToAction("Index", "Weather");

            User user = (User)Session["User"];
            City city = user.Cities.FirstOrDefault(x => x.Name == name);
            if (city == null)
            {
                City newCity = new City()
                {
                    Name = name,
                    Country = country,
                    _Id = id
                };
                userRepository.AddCity(user, newCity);
                using (WeatherController weatherController = new WeatherController(userRepository, weatherRepository))
                {
                    weatherController.DownloadCityWeather(newCity);
                }
                Session["User"] = userRepository.Users.FirstOrDefault(x => x.UserID == user.UserID);
            }
            return RedirectToAction("Index", "Weather");
        }

        public ActionResult DeleteCity(int cityID)
        {
            User user = (User)Session["User"];
            userRepository.DeleteCity(user, cityID);
            return RedirectToAction("Index", "Weather");
        }

        public List<City> GetAllCitiesAlternate()
        {
            List<City> AllCities = new List<City>();
            StreamReader citiesListJson = new StreamReader(@"C:\Users\Vellietto\Desktop\VSProjects\Weather\Weather.WebUI\Json\city.list.json");
            string line;
            while ((line = citiesListJson.ReadLine()) != null)
            {
                City city = new JavaScriptSerializer().Deserialize<City>(line);
                AllCities.Add(city);
            }
            return AllCities.Where(x => x.Name.Length > 0).ToList();
        }

        public List<City> GetAllCities()
        {
            List<City> AllCities = new List<City>();
            StreamReader citiesListJson = new StreamReader(Server.MapPath(Url.Content("~/Json/city.list.json")));
            string line;
            while ((line = citiesListJson.ReadLine()) != null)
            {
                City city = new JavaScriptSerializer().Deserialize<City>(line);
                AllCities.Add(city);
            }
            List<City> DistinctCities = AllCities.GroupBy(x => x.Name).Select(x => x.First()).ToList();
            return DistinctCities.Where(x => x.Name.Length > 0).ToList();
        }
    }
}