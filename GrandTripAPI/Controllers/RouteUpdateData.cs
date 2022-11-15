using System.Collections.Generic;

using GrandTripAPI.Models.JSON;

namespace GrandTripAPI.Controllers
{
    public class RouteUpdateData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DotJson> Dots { get; set; }
        public List<LineJson> Lines { get; set; }
        public string Theme { get; set; }
        public string Season { get; set; }
    }
}