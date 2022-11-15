using System.Collections.Generic;
using GrandTripAPI.Data.Repositories;
using GrandTripAPI.Models.JSON;

namespace GrandTripAPI.Controllers
{
    public class UpdateRouteRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DotJson> Dots { get; set; }
        public List<LineJson> Lines { get; set; }
        public string Theme { get; set; }
        public string Season { get; set; }

        public RouteUpdateData ToData(RouteRepository repo)
        {
            return new RouteUpdateData
            {
                Name = Name,
                Description = Description,
            };
        }
    }
}