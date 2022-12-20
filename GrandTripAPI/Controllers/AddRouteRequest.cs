using System.Collections.Generic;
using System.Linq;
using GrandTripAPI.Models.JSON;
using Newtonsoft.Json;

#nullable enable

namespace GrandTripAPI.Controllers
{
    public class AddRouteRequest
    {
        public string RouteName { get; set; } = "Без названия";
        public string Description { get; set; } = "Без описания";
        public string Theme { get; set; } = "none";
        public string Season { get; set; } = "none";
        public string[]? Dots { get; set; }
        public string[]? Lines { get; set; }

        public (IEnumerable<DotJson>, IEnumerable<LineJson>) Deserialize()
        {
            var dots = Dots?.Select(JsonConvert.DeserializeObject<DotJson>).ToList()
                ?? new List<DotJson>();
            var lines = Lines?.Select(JsonConvert.DeserializeObject<LineJson>).ToList()
                ?? new List<LineJson>();
            return (dots, lines);
        }
    }
}