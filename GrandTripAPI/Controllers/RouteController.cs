using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

using GrandTripAPI.Data.Repositories;
using GrandTripAPI.Models;

namespace GrandTripAPI.Controllers
{
    public class RouteController : ControllerBase
    {
        private readonly RouteRepository _routeRepo;
        private readonly UserRepository _userRepo;
        public RouteController(RouteRepository routeRepo, UserRepository userRepo)
        {
            _routeRepo = routeRepo;
            _userRepo = userRepo;
        }
        public async Task<IActionResult> AddRoute(AddRouteRequest data)
        {
            var existingRoute = await _routeRepo.GetBy(r => r.RouteName == data.RouteName);
            if(existingRoute is not null) return BadRequest(new
            {
                err = "Маршрут с таким названием уже существует"
            });
            
            
            var theme = await _routeRepo.GetTheme(data.Theme);
            theme ??= await _routeRepo.GetTheme("Без тематики");

            var season = await _routeRepo.GetSeason(data.Season);
            season ??= await _routeRepo.GetSeason("Все сезоны"); 
                
            var dots = data.Dots.Select(d => new Dot
            {
                DotId = d.Id,
                DotName = d.Name,
                DotDescription = d.Description,
                PositionX = d.PositionX,
                PositionY = d.PositionY,
            }).ToList();

            var lines = data.Lines.Select(l => new Line
            {
                LineId = l.Id,
                LatLngs = l.LatLngs
            }).ToList();

            var user = await _userRepo.GetBy(u => u.Id == HttpContext.GetId());
            if (user is null) return Forbid();
            
            var route = Route.NewRoute(
                data.RouteName, 
                data.Description, 
                dots, lines, 
                theme, season,
                user);

            var id = await _routeRepo.AddRoute(route);
            return Ok(id);
        }

        public async Task<IActionResult> GetRoute(GetRouteRequest filters)
        {
            var route = await _routeRepo.GetByWith(r =>
                r.Theme.Name == filters.Theme &&
                r.Season.Name == filters.Season, 
                r=>r.Theme, r=>r.Season);
            if (route is null) return NotFound();
            return Ok(new {route});
        }

        public async Task<IActionResult> UpdateRoute(UpdateRouteRequest data)
        {
            var route = await _routeRepo.GetBy(r => r.RouteId == data.Id);
            if (route is null) return NotFound();

            var updateData = new RouteUpdateData
            {
                Name = data.Name,
                Description = data.Description,
                Dots = data.Dots.Select(d => d.ToDomain()),
                Lines = data.Lines.Select(l => l.ToDomain()),
                Theme = await _routeRepo.GetTheme(data.Theme),
                Season = await _routeRepo.GetSeason(data.Season)
            };
            
            route.Update(updateData);
            _routeRepo.UpdateRoute(route);
            
            return Ok();
        }

        public async Task<IActionResult> DeleteRoute([FromQuery] int id)
        {
            var route = await _routeRepo.GetBy(r => r.RouteId == id);
            if (route is null) return NotFound();

            _routeRepo.DeleteRoute(route);
            return Ok();
        }
    }
}