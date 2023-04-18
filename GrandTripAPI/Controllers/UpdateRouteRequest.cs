using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrandTripAPI.Data.Repositories;
using GrandTripAPI.Models;
using GrandTripAPI.Models.JSON;
using Newtonsoft.Json;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

#nullable enable
namespace GrandTripAPI.Controllers
{
    public class UpdateRouteRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string[]? Dots { get; set; }
        public string[]? Lines { get; set; }
        public string? Theme { get; set; }
        public string? Season { get; set; }
        public string? City { get; set; }
        public string? Duration { get; set; }

        public async Task<RouteUpdateData> ToData(RouteRepository repo)
        {
            return new RouteUpdateData
            {
                Name = Name,
                Description = Description,
                Dots = Dots?
                    .Select(JsonConvert.DeserializeObject<DotJson>)
                    .Distinct(new DotsLinesComparer())
                    .Select(d => d.ToDomain()).ToList() ?? new List<Dot>(),
                Lines = Lines?
                    .Select(JsonConvert.DeserializeObject<LineJson>)
                    .Distinct(new DotsLinesComparer())
                    .Select(l => l.ToDomain()).ToList() ?? new List<Line>(),
                Theme = await repo.GetTheme(Theme ?? "none"),
                Season = await repo.GetSeason(Season ?? "none"),
                Duration = Duration,
                City = City
            };
        }

        private class DotsLinesComparer : IEqualityComparer<DotJson>, IEqualityComparer<LineJson>
        {
            public bool Equals(DotJson? d1, DotJson? d2)
            {
                return d1 is null && d2 is null
                       || d2 is not null && d1 is not null
                       || d1?.Id == d2?.Id
                       || d1?.Name == d2?.Name;
            }
            public bool Equals(LineJson? l1, LineJson? l2) => l1?.Id == l2?.Id;
            public int GetHashCode(DotJson d) => d.Id.GetHashCode();
            public int GetHashCode(LineJson l) => l.Id.GetHashCode();
        }
    }
}
#nullable disable