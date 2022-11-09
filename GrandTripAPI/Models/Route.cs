using System.Collections.Generic;

namespace GrandTripAPI.Models
{
    public class Route
    {
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public string Description { get; set; }
        public List<Dot> Dots { get; set; }
        public List<Line> Lines { get; set; }
    }
}