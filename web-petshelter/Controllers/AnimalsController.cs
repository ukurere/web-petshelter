using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web_petshelter.Data;
using web_petshelter.Models;

namespace web_petshelter.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AnimalsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Animals
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Animals.Include(a => a.Shelter);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Animals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var animal = await _context.Animals
                .Include(a => a.Shelter)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (animal == null) return NotFound();
            return View(animal);
        }

        // GET: Animals/Create
        public IActionResult Create()
        {
            ViewData["ShelterId"] = new SelectList(_context.Shelters, "Id", "Name");
            return View();
        }

        // POST: Animals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Species,AgeYears,ShelterId")] Animal animal, IFormFile? photo)
        {
            // handle file
            if (photo is not null && photo.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
                var filePath = Path.Combine(uploads, fileName);

                await using var stream = System.IO.File.Create(filePath);
                await photo.CopyToAsync(stream);

                animal.PhotoPath = $"/uploads/{fileName}";
            }

            if (ModelState.IsValid)
            {
                _context.Add(animal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ShelterId"] = new SelectList(_context.Shelters, "Id", "Name", animal.ShelterId);
            return View(animal);
        }

        // GET: Animals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var animal = await _context.Animals.FindAsync(id);
            if (animal == null) return NotFound();

            ViewData["ShelterId"] = new SelectList(_context.Shelters, "Id", "Name", animal.ShelterId);
            return View(animal);
        }

        // POST: Animals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Species,AgeYears,ShelterId,PhotoPath")] Animal formModel, IFormFile? photo)
        {
            if (id != formModel.Id) return NotFound();

            var animal = await _context.Animals.FirstOrDefaultAsync(a => a.Id == id);
            if (animal == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["ShelterId"] = new SelectList(_context.Shelters, "Id", "Name", formModel.ShelterId);
                return View(formModel);
            }

            // update scalar fields
            animal.Name = formModel.Name;
            animal.Species = formModel.Species;
            animal.AgeYears = formModel.AgeYears;
            animal.ShelterId = formModel.ShelterId;

            // if a new file was uploaded – replace path
            if (photo is not null && photo.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
                var filePath = Path.Combine(uploads, fileName);

                await using var stream = System.IO.File.Create(filePath);
                await photo.CopyToAsync(stream);

                animal.PhotoPath = $"/uploads/{fileName}";
            }
            // else: keep existing animal.PhotoPath as-is

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Animals.Any(e => e.Id == id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Animals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var animal = await _context.Animals
                .Include(a => a.Shelter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null) return NotFound();

            return View(animal);
        }

        // POST: Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal != null) _context.Animals.Remove(animal);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
