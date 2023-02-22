using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using GrandTripAPI.Data.Repositories;
using GrandTripAPI.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GrandTripAPI.Controllers
{
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

            var route = Route.NewRoute(
                data.RouteName, 
                data.Description, 
                dots, lines, 
                theme, season, data.City,
                user);

            var id = await _routeRepo.AddRoute(route);
            var r = await _routeRepo.GetBy(x => x.RouteId == id);
            return Ok(id);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetRoutes(GetRouteRequest filters)
        {
            var l = HttpContext.L<RouteController>();
            var routes = await _routeRepo.GetAll(filters.Theme, filters.Season);
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
            var l = HttpContext.L<RouteController>();
            route.Update(updateData);
            
            await _routeRepo.UpdateRoute(route, true);
            
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

        [HttpGet("created")]
        public async Task<IActionResult> GetCreatedRoutes([FromQuery] string ids)
        {
            var id = HttpContext.GetId();
            if (id is null) return BadRequest();

            var l = HttpContext.L<RouteController>();

            var user = await _userRepo.GetBy(u=>u.Id== id);
            if (user is null) return BadRequest();

            var createdRoutesIds = JsonConvert.DeserializeObject<int[]>(ids);
            var routes = await _routeRepo.GetByIds(createdRoutesIds);

            return Ok(new { routes = routes.Select(r=>r.ToJson()).ToArray() });
        }
    }
}