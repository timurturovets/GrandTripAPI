using System.ComponentModel.DataAnnotations;

namespace GrandTripAPI.Models
{
    public class Dot
    {
        [Key]
        public int InstanceId { get; set; } // Первичный ключ

        public int DotId { get; set; } //Номер точки в наборе точек
        public string DotName { get; set; }
        public string DotDescription { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public int RouteId { get; set; }
        public Route Route { get; set; }
    }
}