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
        public IEnumerable<DotJson> Dots { get; set; }
        public IEnumerable<LineJson> Lines { get; set; }
        public string Theme { get; set; }
        public string Season { get; set; }

    }
}