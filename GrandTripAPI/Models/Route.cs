using System.Linq;
using System.Collections.Generic;
using GrandTripAPI.Controllers;
using GrandTripAPI.Models.JSON;

namespace GrandTripAPI.Models
{
    public class Route
    {
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public string Description { get; set; }
        public IEnumerable<Dot> Dots { get; set; }
        public IEnumerable<Line> Lines { get; set; }
        public RouteTheme Theme { get; set; }
        public RouteSeason Season { get; set; }
        
        #region Methods

        public static Route NewRoute(string name, string description,
            List<Dot> dots, List<Line> lines, 
            RouteTheme theme, RouteSeason season)
        {
            return new Route
            {
                RouteName = name,
                Description = description,
                Dots = dots,
                Lines = lines,
                Theme = theme,
                Season = season
            };
        }

        public void Update(UpdateRouteRequest data)
        {
            RouteName = data.Name;
            Description = data.Description;
            Dots = data.Dots.Select(d => d.ToDomain());
            Lines = data.Lines.Select(l => l.ToDomain());
        }
        #endregion
    }
}