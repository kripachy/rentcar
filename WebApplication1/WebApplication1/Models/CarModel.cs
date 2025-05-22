using System;

namespace WebApplication1.Models
{
    public class CarModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public string Engine { get; set; }
        public string Transmission { get; set; }
        public string Horsepower { get; set; }
        public string Acceleration { get; set; }
        public string TopSpeed { get; set; }
        public string FuelConsumption { get; set; }
        public string[] Images { get; set; }
    }
} 