using System.ComponentModel.DataAnnotations;

namespace GrandTripAPI.Controllers
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Вы не ввели логин.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Вы не ввели пароль")]
        public string Password { get; set; }
    }
}