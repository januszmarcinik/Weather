using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Weather.WebUI.Models;
using Weather.Domain.Entities;
using Weather.Domain.Abstract;
using System.Net;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System.Drawing;

namespace Weather.WebUI.Controllers
{
    public class WeatherController : MyCustomController
    {
        private IUserRepository userRepository;
        private IWeatherRepository weatherRepository;

        public WeatherController(IUserRepository userRepo, IWeatherRepository weatherRepo) : base(weatherRepo)
        {
            userRepository = userRepo;
            weatherRepository = weatherRepo;
        }

        public ViewResult Index()
        {
            if (!IsWeatherActual)
            {
                UpdateWeather();
                IsWeatherActual = true;
            }
                
            User user = (User)Session["User"];
            IEnumerable<WeatherReading> WeatherReadings = weatherRepository.WeatherReadings.OrderByDescending(x => x.Date);
            List<WeatherViewModel> model = new List<WeatherViewModel>();
            foreach (var city in user.Cities)
            {
                WeatherReading weather = WeatherReadings.FirstOrDefault(x => x.CityID == city.CityID);
                if (weather != null)
                {
                    WeatherViewModel singleModel = new WeatherViewModel()
                    {
                        CityID = city.CityID,
                        CityName = city.Name,
                        Country = city.Country,
                        Temperature = weather.Temperature,
                        Humidity = weather.Humidity
                    };
                    model.Add(singleModel);
                }
            }
            return View(model);
        }

        public ViewResult Histogram(int cityID)
        {
            User user = (User)Session["User"];
            City city = user.Cities.FirstOrDefault(x => x.CityID == cityID);
            IEnumerable<WeatherReading> weatherReadings = weatherRepository.WeatherReadings.Where(x => x.CityID == cityID);

            double[] temperatureDataKelwin = weatherReadings.Select(x => x.Temperature).ToArray();
            for (int i = 0; i < temperatureDataKelwin.Length; i++)
            {
                temperatureDataKelwin[i] = Math.Round(temperatureDataKelwin[i] - 273.15, 0);
            }

            object[] temperatureData = temperatureDataKelwin.Cast<object>().ToArray();
            object[] humidityData = weatherReadings.Select(x => x.Humidity).Cast<object>().ToArray();
            string[] dates = weatherReadings.Select(x => x.Date.ToString()).ToArray();

            Highcharts temperatureChart = new Highcharts("temperature")
                .InitChart(new Chart
                {
                    Type = ChartTypes.Line,
                    MarginRight = 130,
                    MarginBottom = 25,
                    ClassName = "temperature"
                })
                .SetTitle(new Title { Text = "Temperatura (°C)", X = -20 })
                .SetSubtitle(new Subtitle { Text = "Źródło danych pogodowych: openweathermap.org", X = -20 })
                .SetXAxis(new XAxis { Categories = dates })
                .SetYAxis(new YAxis
                {
                    Title = new YAxisTitle { Text = "Temperatura (°C)" },
                    PlotLines = new[] { new YAxisPlotLines { Value = 0, Width = 1, Color = ColorTranslator.FromHtml("#808080") } }
                })
                .SetTooltip(new Tooltip { Formatter = @"function() { return '<b>'+ this.series.name +'</b><br/>'+ this.x +': '+ this.y +'°C'; }" })
                .SetLegend(new Legend
                {
                    Layout = Layouts.Vertical,
                    Align = HorizontalAligns.Right,
                    VerticalAlign = VerticalAligns.Top,
                    X = -10,
                    Y = 100,
                    BorderWidth = 0
                })
                .SetSeries(new[] { new Series { Name = city.Name, Data = new Data(temperatureData), Color = Color.FromArgb(217, 83, 79) } }
                );

            Highcharts humidityChart = new Highcharts("humidity")
                .InitChart(new Chart
                {
                    Type = ChartTypes.Line,
                    MarginRight = 130,
                    MarginBottom = 25,
                    ClassName = "humidity"
                })
                .SetTitle(new Title { Text = "Wilgotność (%)", X = -20 })
                .SetSubtitle(new Subtitle { Text = "Źródło danych pogodowych: openweathermap.org", X = -20 })
                .SetXAxis(new XAxis { Categories = dates })
                .SetYAxis(new YAxis
                {
                    Title = new YAxisTitle { Text = "Wilgotność (%)" },
                    PlotLines = new[] { new YAxisPlotLines { Value = 0, Width = 1, Color = ColorTranslator.FromHtml("#808080") } }
                })
                .SetTooltip(new Tooltip { Formatter = @"function() { return '<b>'+ this.series.name +'</b><br/>'+ this.x +': '+ this.y +'%'; }" })
                .SetLegend(new Legend
                {
                    Layout = Layouts.Vertical,
                    Align = HorizontalAligns.Right,
                    VerticalAlign = VerticalAligns.Top,
                    X = -10,
                    Y = 100,
                    BorderWidth = 0
                })
                .SetSeries(new[] { new Series { Name = city.Name, Data = new Data(humidityData), Color = Color.FromArgb(51, 122, 183) } }
                );

            HistogramViewModel model = new HistogramViewModel()
            {
                City = city.Name,
                TemperatureChart = temperatureChart,
                HumidityChart = humidityChart
            };

            return View(model);
        }

        public void UpdateWeather()
        {
            User user = (User)Session["User"];
            foreach(var city in user.Cities)
            {
                DownloadCityWeather(city);
            }
        }

        public void DownloadCityWeather(City city)
        {
            DateTime start = DateTime.Now;
            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?APPID=75a3e7d73376fa1ae08a542ca103899a&id={0}", city._Id);
            WebClient client = new WebClient();
            string json = client.DownloadString(url);
            JsonViewModel jsonModel = new JavaScriptSerializer().Deserialize<JsonViewModel>(json);
            WeatherReading weather = new WeatherReading()
            {
                CityID = city.CityID,
                Temperature = jsonModel.Main.Temp,
                Humidity = jsonModel.Main.Humidity,
                Date = DateTime.Now
            };
            weatherRepository.AddWeatherReading(weather);
            DateTime stop = DateTime.Now;
            TimeSpan timeSpan = start - stop;
            double seconds = timeSpan.TotalSeconds;
        }
    }
}