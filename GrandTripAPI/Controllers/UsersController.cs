using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using GrandTripAPI.Data.Repositories;

namespace GrandTripAPI.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepo;

        public UsersController(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<IActionResult> Register(SigningRequest data)
        {
            var exists = await _userRepo.GetBy(u => u.Username == data.Username) != null;
            if (exists) return Conflict();

            var id = await _userRepo.AddUser(data.Username, data.Password);
            
            return Ok(new { token = _userRepo.GenerateToken(id) });
        }

        public async Task<IActionResult> Login(SigningRequest data)
        {
            if (!ModelState.IsValid) return BadRequest(new { data = ModelState } );
            var user = await _userRepo.GetBy(u => u.Username == data.Username);
            if (user == null) return NotFound();

            return Ok(new { token = _userRepo.GenerateToken(user.Id) });
        }

        public async Task<IActionResult> GetUserInfo()
        {
            var id = HttpContext.GetId();
            if (id is null) return BadRequest();

            var user = await _userRepo.GetBy(u => u.Id == id);
            if (user is null) return BadRequest();
            
            return Ok(new {info = user.GetInfo()});
        }
    }
}