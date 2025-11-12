using Microsoft.AspNetCore.Mvc;

namespace web_petshelter.Controllers
{
    public class VolunteerTasksController : Controller
    {
        [HttpGet]
        public IActionResult Local() => View();
    }
}
