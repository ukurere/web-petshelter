using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web_petshelter.Data;
using web_petshelter.Models;
using web_petshelter.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;


namespace web_petshelter.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly AppDbContext _db;

        public AnimalsController(AppDbContext db) => _db = db;

        // Список з фільтрами + пагінація (GET /Animals)
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] AnimalFilterVm f)
        {
            // базовий запит
            var q = _db.Animals
                .AsNoTracking()
                .Include(a => a.Breed)
                .Include(a => a.Shelter)
                .AsQueryable();

            // фільтри
            if (!string.IsNullOrWhiteSpace(f.Species)) q = q.Where(a => a.Species == f.Species);
            if (f.BreedId.HasValue) q = q.Where(a => a.BreedId == f.BreedId.Value);
            if (f.Gender.HasValue) q = q.Where(a => a.Gender == f.Gender.Value);
            if (f.MinAge.HasValue) q = q.Where(a => a.AgeYears >= f.MinAge.Value);
            if (f.MaxAge.HasValue) q = q.Where(a => a.AgeYears <= f.MaxAge.Value);
            if (f.ShelterId.HasValue) q = q.Where(a => a.ShelterId == f.ShelterId.Value);

            // !!! adopted -> derived по наявності записів в Adoptions
            if (f.Adopted.HasValue)
            {
                q = f.Adopted.Value
                    ? q.Where(a => a.Adoptions.Any())
                    : q.Where(a => !a.Adoptions.Any());
            }

            if (!string.IsNullOrWhiteSpace(f.Search)) q = q.Where(a => a.Name.Contains(f.Search));

            // пагінація
            var safePage = Math.Max(1, f.Page);
            var safePageSize = Math.Clamp(f.PageSize, 1, 100);

            f.Total = await q.CountAsync();
            f.Items = await q
                .OrderBy(a => a.Id)
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .ToListAsync();

            // лукапи для селектів
            f.Breeds = await _db.Breeds.AsNoTracking().OrderBy(b => b.Name).ToListAsync();
            f.Shelters = await _db.Shelters.AsNoTracking().OrderBy(s => s.Name).ToListAsync();

            return View(f);
        }

        // Деталі
        public async Task<IActionResult> Details(int id)
        {
            var animal = await _db.Animals
                .Include(a => a.Breed)
                .Include(a => a.Shelter)
                .FirstOrDefaultAsync(a => a.Id == id);

            return animal is null ? NotFound() : View(animal);
        }

        // Create (GET)
        public async Task<IActionResult> Create()
        {
            await LoadLookups();
            return View();
        }

        // Create (POST)
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Animal animal)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookups();
                return View(animal);
            }

            _db.Add(animal);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Edit (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var animal = await _db.Animals.FindAsync(id);
            if (animal is null) return NotFound();

            await LoadLookups();
            return View(animal);
        }

        // Edit (POST)
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Animal animal)
        {
            if (id != animal.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadLookups();
                return View(animal);
            }

            try
            {
                _db.Update(animal);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.Animals.AnyAsync(a => a.Id == id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // Delete (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var animal = await _db.Animals
                .Include(a => a.Breed)
                .Include(a => a.Shelter)
                .FirstOrDefaultAsync(a => a.Id == id);

            return animal is null ? NotFound() : View(animal);
        }

        // Delete (POST)
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animal = await _db.Animals.FindAsync(id);
            if (animal is not null)
            {
                _db.Animals.Remove(animal);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadLookups()
        {
            ViewData["BreedId"] = new SelectList(
                await _db.Breeds.AsNoTracking().OrderBy(b => b.Name).ToListAsync(), "Id", "Name");

            ViewData["ShelterId"] = new SelectList(
                await _db.Shelters.AsNoTracking().OrderBy(s => s.Name).ToListAsync(), "Id", "Name");

            ViewData["GenderList"] = new SelectList(
                Enum.GetValues(typeof(Gender)).Cast<Gender>().Select(g => new { Id = (int)g, Name = g.ToString() }),
                "Id", "Name");
        }
    }
}
