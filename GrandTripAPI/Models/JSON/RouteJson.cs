using System.Collections.Generic;

namespace GrandTripAPI.Models.JSON
{
    public class RouteJson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DotJson> Dots { get; set; }
        public List<LineJson> Lines { get; set; }
    }
}