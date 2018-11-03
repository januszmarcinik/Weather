namespace Weather.WebUI.Models
{
    public class JsonViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Main Main { get; set; }
        public Sys Sys { get; set; }
    }

    public class Main
    {
        public double Temp { get; set; }
        public int Humidity { get; set; }
        public double Temp_min { get; set; }
        public double Temp_max { get; set; }
    }

    public class Sys
    {
        public string Country { get; set; }
    }
}