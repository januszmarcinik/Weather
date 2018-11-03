using DotNet.Highcharts;

namespace Weather.WebUI.Models
{
    public class HistogramViewModel
    {
        public string City { get; set; }
        public Highcharts TemperatureChart { get; set; }
        public Highcharts HumidityChart { get; set; }
    }
}