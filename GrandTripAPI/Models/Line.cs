using System.ComponentModel.DataAnnotations;

namespace GrandTripAPI.Models
{
    public class Line
    {
        [Key]
        public int InstanceId { get; set; } // Первичный ключ
        public int LineId { get; set; } // Номер линии в наборе линий
        public double[][] LatLngs { get; set; }
        public int RouteId { get; set; }
    }
}