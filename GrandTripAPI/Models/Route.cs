using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using GrandTripAPI.Controllers;
using GrandTripAPI.Models.JSON;

namespace GrandTripAPI.Models
{
    public class Route
    {
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public string Description { get; set; }
        public IEnumerable<Dot> Dots { get; set; } = new List<Dot>();
        public IEnumerable<Line> Lines { get; set; } = new List<Line>();
        public RouteTheme Theme { get; set; }
        public RouteSeason Season { get; set; }
        public string City { get; set; }
        public string Duration {get;set;}
        public User Creator { get; set; }
        //public IEnumerable<User> Preferers { get; set; }

        #region Methods

        public static Route NewRoute(string name, string description,
            IEnumerable<Dot> dots, IEnumerable<Line> lines, 
            RouteTheme theme, RouteSeason season, string city, string duration,
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
                City = city,
                Duration = duration,
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
            City = data.City;
            Duration = data.Duration;
        }

        public RouteJson ToJson()
        {
            return new RouteJson
            {
                Id = RouteId,
                Name = RouteName,
                Description = Description,
                Theme = Theme?.Name ?? "Без тематики",
                Season = Season?.Name ?? "Все сезоны",
                City = City ?? "spb",
                Duration = Duration ?? "none",
                Dots = Dots.Select(d => d.ToJson()).ToList(),
                Lines = Lines.Select(l=>l.ToJson()).ToList(),
                Author = Creator.Username
            };
        }
        #endregion
    }
}