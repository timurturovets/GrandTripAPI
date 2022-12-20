using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using GrandTripAPI.Data.Repositories;
using GrandTripAPI.Models;
using Microsoft.Extensions.Logging;

namespace GrandTripAPI.Controllers
{
    //[ApiController]
    [Route("api/[controller]")]
    public class RouteController : ControllerBase
    {
        private readonly RouteRepository _routeRepo;
        private readonly UserRepository _userRepo;
        public RouteController(RouteRepository routeRepo, UserRepository userRepo)
        {
            _routeRepo = routeRepo;
            _userRepo = userRepo;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRoute([FromForm] AddRouteRequest data)
        {
            var logger = HttpContext.L<RouteController>();

            logger.LogInformation($"{data.RouteName}-{data.Dots?.Length}-{data.Lines?.Length}");
            var existingRoute = await _routeRepo.GetBy(r => r.RouteName == data.RouteName);
            if (existingRoute is not null) return BadRequest(new
            {
                err = "Маршрут с таким названием уже существует"
            });
            
            var theme = await _routeRepo.GetTheme(data.Theme) 
                        ?? await _routeRepo.GetTheme("Без тематики");

            var season = await _routeRepo.GetSeason(data.Season) 
                         ?? await _routeRepo.GetSeason("Все сезоны");

            var (dataDots, dataLines) = data.Deserialize();
            var dots = dataDots.Select(d=>d.ToDomain()).ToList();
            var lines = dataLines.Select(l=>l.ToDomain()).ToList();

            var user = await _userRepo.GetBy(u => u.Id == HttpContext.GetId());
            if (user is null) return BadRequest();

            logger.LogCritical($"user: {user.Username ?? "Без имени"}");
            var route = Route.NewRoute(
                data.RouteName, 
                data.Description, 
                dots, lines, 
                theme, season, 
                user);

            var id = await _routeRepo.AddRoute(route);
            var r = await _routeRepo.GetBy(x => x.RouteId == id);
            logger.LogCritical($"added route: {r?.RouteName ?? "no"}");
            return Ok(id);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetRoutes(GetRouteRequest filters)
        {
            var l = HttpContext.L<RouteController>();
            l.LogCritical($"Theme: 1{filters.Theme}1, season: 1{filters.Season}1");
            var routes = await _routeRepo.GetAll(filters.Theme, filters.Season);
            foreach (var route in routes.Where(route => route.Theme == null || route.Season == null))
            {
                l.LogCritical($"null at {route.RouteName}");
            }
            return Ok(new { routes = routes.Select(r=>r.ToJson()) });
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetRoute([FromQuery] [FromBody] [FromForm] int id)
        {
            var route = await _routeRepo.GetByWith(r => r.RouteId == id, 
                r=>r.Dots, r=>r.Lines);
            if (route is null) return NotFound();
            
            return Ok(new { route = route.ToJson() });
        }
        
        [HttpPost("update")]
        public async Task<IActionResult> UpdateRoute([FromForm] UpdateRouteRequest request)
        {
            var route = await _routeRepo.GetBy(r => r.RouteId == request.Id);
            if (route is null) return NotFound();
    
            var updateData = await request.ToData(_routeRepo);
  
            route.Update(updateData);
            
            await _routeRepo.UpdateRoute(route);
            
            return Ok();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteRoute([FromForm] int id)
        {
            var route = await _routeRepo.GetBy(r => r.RouteId == id);
            if (route is null) return NotFound();

            await _routeRepo.DeleteRoute(route);
            return Ok();
        }
    }
}