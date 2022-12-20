using System.ComponentModel.DataAnnotations;
using GrandTripAPI.Models.JSON;

namespace GrandTripAPI.Models
{
    public class Dot
    {
        [Key]
        public int InstanceId { get; set; } // Первичный ключv
        public int DotId { get; set; } //Номер точки в наборе точек
        public string DotName { get; set; }
        public string DotDescription { get; set; }
        public string Link { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public int RouteId { get; set; }
        public Route Route { get; set; }

        public DotJson ToJson()
        {
            return new DotJson
            {
                Id = DotId.ToString(),
                Name = DotName,
                Description = DotDescription,
                PositionX = PositionX,
                PositionY = PositionY
            };
        }
    }
}