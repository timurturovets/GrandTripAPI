using Microsoft.AspNetCore.Mvc;

namespace GrandTripAPI.Controllers
{
    public class RouteController : ControllerBase
    {

        public IActionResult Index()
        {
            return Ok();
        }
    }
}