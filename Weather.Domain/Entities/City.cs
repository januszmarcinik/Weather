namespace Weather.Domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        public int ExternalApiId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }
}