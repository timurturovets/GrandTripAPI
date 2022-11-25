using System.Collections.Generic;

using GrandTripAPI.Controllers;

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
        public User Creator { get; set; }
        public IEnumerable<User> Preferers { get; set; }

        #region Methods

        public static Route NewRoute(string name, string description,
            IEnumerable<Dot> dots, IEnumerable<Line> lines, 
            RouteTheme theme, RouteSeason season,
            User creator)
        {
            return new Route
            {
                RouteName = name,
                Description = description,
                Dots = dots,
                Lines = lines,
                Theme = theme,
                Season = season,
                Creator = creator
            };
        }

        public void Update(RouteUpdateData data)
        {
            RouteName = data.Name;
            Description = data.Description;
            Dots = data.Dots;
            Lines = data.Lines;
            Theme = data.Theme;
            Season = data.Season;
        }
        
        #endregion
    }
}