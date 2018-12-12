using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Weather.WebUI.Models;
using Weather.Domain.Entities;
using Weather.Domain.Abstract;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System.Drawing;

namespace Weather.WebUI.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherRepository _weatherRepository;
        private readonly ICityRepository _cityRepository;

        public WeatherController(IWeatherRepository weatherRepo, ICityRepository cityRepository) 
        {
            _weatherRepository = weatherRepo;
            _cityRepository = cityRepository;
        }

        public ViewResult Index()
        {
            var cities = _cityRepository
                .Cities
                .ToList();

            var weatherReadings = _weatherRepository
                .WeatherReadings
                .GroupBy(group => group.CityID)
                .Select(x => x
                    .OrderByDescending(g => g.Date)
                    .FirstOrDefault()
                )
                .ToList();

            var model = cities
                .Join(
                    weatherReadings,
                    city => city.Id,
                    weatherReading => weatherReading.CityID,
                    (city, weatherReading) => new WeatherViewModel
                    {
                        CityID = city.Id,
                        CityName = city.Name,
                        Country = city.Country,
                        Temperature = weatherReading.Temperature,
                        Humidity = weatherReading.Humidity
                    }
                )
                .ToList();

            return View(model);
        }

        public ViewResult Histogram(int cityId)
        {
            var city = _cityRepository.Cities.SingleOrDefault(x => x.Id == cityId);
            IEnumerable<WeatherReading> weatherReadings = _weatherRepository.WeatherReadings.Where(x => x.CityID == cityId);

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
    }
}