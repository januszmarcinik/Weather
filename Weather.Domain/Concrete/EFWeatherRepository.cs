using System;
using System.Collections.Generic;
using Weather.Domain.Abstract;
using Weather.Domain.Entities;

namespace Weather.Domain.Concrete
{
    public class EFWeatherRepository : IWeatherRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<WeatherReading> WeatherReadings
        {
            get { return context.WeatherReadings; }
        }

        public void AddWeatherReading(WeatherReading weatherReading)
        {
            context.WeatherReadings.Add(weatherReading);
            context.SaveChanges();
        }
    }
}