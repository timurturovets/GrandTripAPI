using System.Collections.Generic;
using GrandTripAPI.Models.JSON;
namespace GrandTripAPI.Controllers
{
    public class AddRouteRequest
    {
        public string RouteName { get; set; } = "Без названия";
        public string Description { get; set; } = "Без описания";
        public string Theme { get; set; } = "Без тематики";
        public string Season { get; set; } = "Все сезоны";
        public List<DotJson> Dots { get; set; }
        public List<LineJson> Lines { get; set; }
    }
}