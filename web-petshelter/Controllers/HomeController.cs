using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_petshelter.Data;
using web_petshelter.Models;
using web_petshelter.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Mono.TextTemplating;
using web_petshelter.Features.Statistics; // where we put the service/VM
using System.Threading.Tasks;



namespace web_petshelter.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    private readonly ILogger<HomeController> _logger;
    private readonly StatisticsService _stats; 

    public HomeController(AppDbContext db, ILogger<HomeController> logger, StatisticsService stats)
    {
        _db = db;
        _logger = logger;
        _stats = stats;
    }

    public async Task<IActionResult> Landing()
    {
        var vm = new StatsVm
        {
            Animals = await _db.Animals.CountAsync(),
            Shelters = await _db.Shelters.CountAsync(),
            Adoptions = await _db.Adoptions.CountAsync(),
            Adopted = await _db.Adoptions.Select(a => a.AnimalId).Distinct().CountAsync()
        };

        ViewData["BodyClass"] = "index-page";
        return View(vm);
    }

    public IActionResult Index() => View();

    public IActionResult Privacy() => View();

    [HttpGet]
    public async Task<IActionResult> Statistics(DateTime? from, DateTime? to)
    {
        // Normalize: if only "to" provided, make "from" wide; swap if inverted
        if (from.HasValue && to.HasValue && from > to)
            (from, to) = (to, from);

        var model = await _stats.GetAsync(from, to);
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
        => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
