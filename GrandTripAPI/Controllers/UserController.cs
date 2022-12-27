using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using GrandTripAPI.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace GrandTripAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepo;

        public UserController(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(SigningRequest data)
        {
            var exists = await _userRepo.GetBy(u => u.Username == data.Username) != null;
            if (exists) return Conflict();

            var id = await _userRepo.AddUser(data.Username, data.Password);
            
            return Ok(new { token = _userRepo.GenerateToken(id) });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(SigningRequest data)
        {
            var l = HttpContext.L<UserController>();

            l.LogCritical($"req name: {data.Username}, pw: {data.Password}");
            
            var user = await _userRepo.GetBy(u => u.Username == data.Username);
            
            if (user == null) return NotFound();
            if (!user.ValidatePassword(data.Password)) return BadRequest();
            
            return Ok(new { token = _userRepo.GenerateToken(user.Id) });
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetUserInfo()
        {
            var id = HttpContext.GetId();
            if (id is null) return NotFound();

            var user = await _userRepo
                .GetByWith(u => u.Id == id, 
                    u => u.CreatedRoutes);
            
            if (user is null) return NotFound();
            
            return Ok(new {info = user.GetInfo()});
        }
        
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllUsers()
        {
            var id = HttpContext.GetId();
            if (id is null) return BadRequest();

            var user = await _userRepo.GetBy(u => u.Id == id);
            if (user is null || user.Role != "Admin") return BadRequest();

            var users = (await _userRepo.GetAll(true)).Select(u=>u.GetInfo());
            return Ok(new { users });
        }
    }
}