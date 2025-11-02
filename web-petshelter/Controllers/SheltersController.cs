using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web_petshelter.Data;
using web_petshelter.Models;

namespace web_petshelter.Controllers
{
    public class SheltersController : Controller
    {
        private readonly AppDbContext _context;

        public IActionResult Map() => View();

        public SheltersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Shelters
        public async Task<IActionResult> Index()
        {
            return View(await _context.Shelters.ToListAsync());
        }

        // GET: Shelters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelter = await _context.Shelters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shelter == null)
            {
                return NotFound();
            }

            return View(shelter);
        }

        // GET: Shelters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shelters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,City")] Shelter shelter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shelter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shelter);
        }

        // GET: Shelters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelter = await _context.Shelters.FindAsync(id);
            if (shelter == null)
            {
                return NotFound();
            }
            return View(shelter);
        }

        // POST: Shelters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,City")] Shelter shelter)
        {
            if (id != shelter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shelter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShelterExists(shelter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(shelter);
        }

        // GET: Shelters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelter = await _context.Shelters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shelter == null)
            {
                return NotFound();
            }

            return View(shelter);
        }

        // POST: Shelters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shelter = await _context.Shelters.FindAsync(id);
            if (shelter != null)
            {
                _context.Shelters.Remove(shelter);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShelterExists(int id)
        {
            return _context.Shelters.Any(e => e.Id == id);
        }
    }
}
