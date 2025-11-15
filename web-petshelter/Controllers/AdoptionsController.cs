using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web_petshelter.Data;
using web_petshelter.Models;

namespace web_petshelter.Controllers
{
    [Authorize]
    public class AdoptionsController : Controller
    {
        private readonly AppDbContext _context;
        public AdoptionsController(AppDbContext context) => _context = context;

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var list = await _context.Adoptions
                                     .Include(a => a.Animal)
                                     .AsNoTracking()
                                     .ToListAsync();
            return View(list);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();
            var adoption = await _context.Adoptions
                                         .Include(a => a.Animal)
                                         .FirstOrDefaultAsync(m => m.Id == id);
            if (adoption is null) return NotFound();
            return View(adoption);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["AnimalId"] = new SelectList(_context.Animals.AsNoTracking(), "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AnimalId,AdopterName,AdopterEmail,AdoptedAt")] Adoption adoption)
        {
            if (!ModelState.IsValid)
            {
                ViewData["AnimalId"] = new SelectList(_context.Animals.AsNoTracking(), "Id", "Name", adoption.AnimalId);
                return View(adoption);
            }
            _context.Adoptions.Add(adoption);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();
            var adoption = await _context.Adoptions.FindAsync(id);
            if (adoption is null) return NotFound();
            ViewData["AnimalId"] = new SelectList(_context.Animals.AsNoTracking(), "Id", "Name", adoption.AnimalId);
            return View(adoption);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AnimalId,AdopterName,AdopterEmail,AdoptedAt")] Adoption adoption)
        {
            if (id != adoption.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewData["AnimalId"] = new SelectList(_context.Animals.AsNoTracking(), "Id", "Name", adoption.AnimalId);
                return View(adoption);
            }

            try
            {
                _context.Update(adoption);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Adoptions.AnyAsync(e => e.Id == id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();
            var adoption = await _context.Adoptions
                                         .Include(a => a.Animal)
                                         .FirstOrDefaultAsync(m => m.Id == id);
            if (adoption is null) return NotFound();
            return View(adoption);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adoption = await _context.Adoptions.FindAsync(id);
            if (adoption != null)
            {
                _context.Adoptions.Remove(adoption);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
