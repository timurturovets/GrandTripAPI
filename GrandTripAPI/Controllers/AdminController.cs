using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using GrandTripAPI.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace GrandTripAPI.Controllers
{
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly RouteRepository _routeRepo;
        private readonly UserRepository _userRepo;

        public AdminController(RouteRepository routeRepo, UserRepository userRepo)
        {
            _routeRepo = routeRepo;
            _userRepo = userRepo;
        }

        [HttpPost("changerole")]
        public async Task<IActionResult> ChangeUserRole(int userId, string role)
        {
            var currentUser = await HttpContext.GetUser();
            if (currentUser is null || currentUser.Role != "Admin") return BadRequest();

            var l = HttpContext.L<AdminController>();
            l.LogCritical($"UserId: {userId}, role: {role}");
            var user = await _userRepo.GetBy(u => u.Id == userId);
            if (user is null) return BadRequest();
            
            user.Role = role;
            await _userRepo.Update(user);

            return Ok();
        }

        [HttpPost("changeauthor")]
        public async Task<IActionResult> ChangeRouteAuthour([FromForm] int routeId, [FromForm] int authorId)
        {
            var currentUser = await HttpContext.GetUser();
            if (currentUser is null || currentUser.Role != "Admin") return BadRequest();

            var route = await _routeRepo.GetBy(r => r.RouteId == routeId);
            if (route is null) return NotFound();

            var newAuthor = await _userRepo.GetBy(u => u.Id == authorId);
            if (newAuthor is null) return NotFound();

            route.Creator = newAuthor;
            
            await _routeRepo.UpdateRoute(route);
            return Ok();
        }
    }
}