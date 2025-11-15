using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_petshelter.Data;
using web_petshelter.Models;

namespace web_petshelter.Controllers
{
    public class SheltersController : Controller
    {
        private readonly AppDbContext _db;
        public SheltersController(AppDbContext db) => _db = db;

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? q, string? city)
        {
            IQueryable<Shelter> query = _db.Shelters.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(s =>
                    s.Name.Contains(q) ||
                    (s.Address != null && s.Address.Contains(q)));

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(s => s.City == city);

            var items = await query.OrderBy(s => s.Name).ToListAsync();
            return View(items);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var shelter = await _db.Shelters
                .AsNoTracking()
                .Include(s => s.Animals)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shelter is null) return NotFound();
            return View(shelter);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Map()
        {
            ViewData["BodyClass"] = "light-background";
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> MapData()
        {
            var items = await _db.Shelters
                .Where(s => s.Lat != null && s.Lng != null)
                .Select(s => new ShelterMapDto(
                    s.Id,
                    s.Name,
                    s.Address,
                    s.Lat!.Value,
                    s.Lng!.Value))
                .ToListAsync();

            return Json(items);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View(new Shelter());

        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Address,City,Lat,Lng,Phone,PhotoUrl,WorkHours")]
            Shelter input)
        {
            if (!ModelState.IsValid) return View(input);

            _db.Shelters.Add(input);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var shelter = await _db.Shelters.FindAsync(id);
            if (shelter is null) return NotFound();
            return View(shelter);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,Address,City,Lat,Lng,Phone,PhotoUrl,WorkHours")]
            Shelter input)
        {
            if (id != input.Id) return NotFound();
            if (!ModelState.IsValid) return View(input);

            try
            {
                _db.Update(input);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _db.Shelters.AnyAsync(s => s.Id == id);
                if (!exists) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var shelter = await _db.Shelters
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
            if (shelter is null) return NotFound();
            return View(shelter);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shelter = await _db.Shelters.FindAsync(id);
            if (shelter != null)
            {
                _db.Shelters.Remove(shelter);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }

    public sealed record ShelterMapDto(
        int Id,
        string Name,
        string? Address,
        double Lat,
        double Lng
    );
}
