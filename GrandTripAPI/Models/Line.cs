using System.ComponentModel.DataAnnotations;
using GrandTripAPI.Models.JSON;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GrandTripAPI.Models
{
    public class Line
    {
        [Key]
        public int InstanceId { get; set; } // Первичный ключ
        public int LineId { get; set; } // Номер линии в наборе линий
        public string LatLngs { get; set; }
        public int RouteId { get; set; }
        public Route Route { get; set; }

        public LineJson ToJson()
        {
            return new LineJson
            {
                Id = LineId,
                LatLngs = JsonConvert.DeserializeObject<double[][]>(LatLngs)
            };
        }
    }
}