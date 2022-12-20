using System.Collections.Generic;

namespace GrandTripAPI.Models
{
    public class RouteTheme
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public List<Route> Routes { get; set; }
    }
}