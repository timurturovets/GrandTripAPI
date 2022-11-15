using System.Collections.Generic;

namespace GrandTripAPI.Models.JSON
{
    public class RouteSeason
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Route> Routes { get; set; }
    }
}