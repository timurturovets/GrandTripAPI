using System.Collections.Generic;
using GrandTripAPI.Models;
using GrandTripAPI.Models.JSON;
using RouteSeason = GrandTripAPI.Models.RouteSeason;

namespace GrandTripAPI.Controllers
{
    public class RouteUpdateData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Dot> Dots { get; set; }
        public IEnumerable<Line> Lines { get; set; }
        public RouteTheme Theme { get; set; }
        public RouteSeason Season { get; set; }
    }
}