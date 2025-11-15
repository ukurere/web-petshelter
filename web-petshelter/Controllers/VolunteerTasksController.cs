using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace web_petshelter.Controllers
{
    public class VolunteerTasksController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Local()
        {
            ViewData["BodyClass"] = "light-background";
            return View();
        }
    }
}
