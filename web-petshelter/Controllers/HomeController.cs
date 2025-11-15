using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using web_petshelter.Data;
using web_petshelter.Models;
using web_petshelter.Models.ViewModels;
using web_petshelter.Features.Statistics;

namespace web_petshelter.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AppDbContext db, ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // ??????? ? ??????? + ?????? ??????????
        public async Task<IActionResult> Landing()
        {
            var vm = new StatsVm
            {
                Animals = await _db.Animals.CountAsync(),
                Shelters = await _db.Shelters.CountAsync(),
                Adoptions = await _db.Adoptions.CountAsync(),
                Adopted = await _db.Adoptions
                    .Select(a => a.AnimalId)
                    .Distinct()
                    .CountAsync()
            };

            ViewData["BodyClass"] = "index-page";
            return View(vm);
        }

        public IActionResult Index() => View();

        public IActionResult Privacy() => View();

        // ???????? ?? ??????????? + ?????? ?? ????? ?? ??????
        [HttpGet]
        public async Task<IActionResult> Statistics(DateTime? from, DateTime? to)
        {
            // ???? ?????????? ?????????? ??????? from/to – ??????? ??
            if (from.HasValue && to.HasValue && from > to)
                (from, to) = (to, from);

            // ?????? ??????
            var sheltersQuery = _db.Shelters.AsNoTracking();
            var animalsQuery = _db.Animals.AsNoTracking();
            var adoptionsQuery = _db.Adoptions.AsNoTracking();

            // !!! ????? !!!
            // ??? ?????????? ???? ???? ? Adoption, ??? ? ? ????? ??????.
            // ?????? AdoptionDate ?? ???? ??????????? (AdoptedAt, CreatedAt ????).
            if (from.HasValue)
            {
                adoptionsQuery = adoptionsQuery
                    .Where(a => a.AdoptedAt >= from.Value); // <-- ?????? AdoptionDate, ???? ?????
            }

            if (to.HasValue)
            {
                var endExclusive = to.Value.Date.AddDays(1); // ??????? ?? ????? ??? "to"
                adoptionsQuery = adoptionsQuery
                    .Where(a => a.AdoptedAt < endExclusive); // <-- ?????? AdoptionDate, ???? ?????
            }

            // ????????
            var totalShelters = await sheltersQuery.CountAsync();
            var totalAnimals = await animalsQuery.CountAsync();
            var adoptionsInPeriod = await adoptionsQuery.CountAsync();

            // ?????????? ??????? ?? ???????
            var grouped = await adoptionsQuery
                .GroupBy(a => new { a.AdoptedAt.Year, a.AdoptedAt.Month }) // <-- ??? ??? ?????? ????
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            var labels = grouped
                .Select(g => new DateTime(g.Year, g.Month, 1).ToString("MMM yyyy"))
                .ToList();

            var data = grouped
                .Select(g => g.Count)
                .ToList();

            var vm = new StatisticsVm
            {
                From = from,
                To = to,
                TotalShelters = totalShelters,
                TotalAnimals = totalAnimals,
                AdoptionsThisPeriod = adoptionsInPeriod,
                Chart = new StatisticsChartVm
                {
                    Labels = labels,
                    Data = data,
                    DatasetLabel = "Adoptions"
                }
            };

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
    }
}
