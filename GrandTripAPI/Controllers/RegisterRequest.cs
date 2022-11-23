using System.ComponentModel.DataAnnotations;

namespace GrandTripAPI.Controllers
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}