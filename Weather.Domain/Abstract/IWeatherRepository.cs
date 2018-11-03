using System.Collections.Generic;
using Weather.Domain.Entities;

namespace Weather.Domain.Abstract
{
    public interface IWeatherRepository
    {
        IEnumerable<WeatherReading> WeatherReadings { get; }

        void AddWeatherReading(WeatherReading weatherReading);
    }
}