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

        public async Task<IActionResult> Register(RegisterRequest data)
        {
            var exists = await _userRepo.GetBy(u => u.Username == data.Username) != null;
            if (exists) return Conflict();

            var token = await _userRepo.Register(data.Username, data.Password);
            return Ok(new { token });
        }

        public async Task<IActionResult> Login(LoginRequest data)
        {
            
        }
    }
}