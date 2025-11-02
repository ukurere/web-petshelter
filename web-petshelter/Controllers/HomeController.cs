using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using web_petshelter.Models;
using Microsoft.EntityFrameworkCore;
using web_petshelter.Data;

namespace web_petshelter.Controllers
{
    public class StatsVm { public int Animals; public int Shelters; public int Adoptions; public int Adopted; }

    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db) { _db = db; }

        public async Task<IActionResult> Landing()
        {
            var vm = new StatsVm
            {
                Animals = await _db.Animals.CountAsync(),
                Shelters = await _db.Shelters.CountAsync(),
                Adoptions = await _db.Adoptions.CountAsync(),
                Adopted = await _db.Adoptions.Select(a => a.AnimalId).Distinct().CountAsync()
            };
            ViewBag.Stats = vm;
            return View();
        }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
