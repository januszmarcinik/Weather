﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using Hangfire;
using Weather.Domain.Abstract;
using Weather.WebUI.Jobs;
using Weather.WebUI.Models;

namespace Weather.WebUI.Controllers
{
    public class CityController : Controller
    {
        public CityController(IWeatherRepository weatherRepo, ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public ActionResult SearchCity(string search = null)
        {
            if (string.IsNullOrEmpty(search))
            {
                return RedirectToAction("Index", "Weather");
            }

            ViewBag.SearchCity = search;
            var searchCities = GetAllCities()
                .Where(x => x.Name.ToLower().Contains(search.ToLower()))
                .ToList();

            return View(searchCities);
        }

        public ActionResult AddNewCity(string name, string country, int id)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(country) || id <= 0)
                return RedirectToAction("Index", "Weather");

            var city = _cityRepository.Add(id, name, country);
            BackgroundJob.Enqueue<ReadActualWeatherForOneCityJob>(x => x.Run(city));

            return RedirectToAction("Index", "Weather");
        }

        public ActionResult DeleteCity(int cityId)
        {
            _cityRepository.Delete(cityId);

            return RedirectToAction("Index", "Weather");
        }

        public List<CityJsonViewModel> GetAllCities()
        {
            if (_allCitiesCache != null && _allCitiesCache.Count > 0)
            {
                return _allCitiesCache;
            }

            var allCities = new List<CityJsonViewModel>();
            var citiesListJson = new StreamReader(Server.MapPath(Url.Content("~/Json/city.list.json")));
            string line;
            while ((line = citiesListJson.ReadLine()) != null)
            {
                var city = new JavaScriptSerializer().Deserialize<CityJsonViewModel>(line);
                if (city.IsValid)
                {
                    allCities.Add(city);
                }
            }

            _allCitiesCache = allCities
                .GroupBy(x => x.Name)
                .Select(x => x.First())
                .ToList();

            return _allCitiesCache;
        }

        private readonly ICityRepository _cityRepository;
        private static List<CityJsonViewModel> _allCitiesCache;
    }
}