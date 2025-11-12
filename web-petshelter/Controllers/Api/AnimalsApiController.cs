using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_petshelter.Data;
using web_petshelter.Models;
using Microsoft.AspNetCore.Authorization;

namespace web_petshelter.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AnimalsApiController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<object>> GetAll(
            [FromQuery] string? species,
            [FromQuery] int? breedId,
            [FromQuery] Gender? gender,
            [FromQuery] int? minAge,
            [FromQuery] int? maxAge,
            [FromQuery] int? shelterId,
            [FromQuery] bool? adopted,
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 12)
        {
            var q = _db.Animals.AsNoTracking()
                               .Include(a => a.Breed)
                               .Include(a => a.Shelter)
                               .AsQueryable();

            if (!string.IsNullOrWhiteSpace(species)) q = q.Where(a => a.Species == species);
            if (breedId is not null) q = q.Where(a => a.BreedId == breedId);
            if (gender is not null) q = q.Where(a => a.Gender == gender);
            if (minAge is not null) q = q.Where(a => a.AgeYears >= minAge);
            if (maxAge is not null) q = q.Where(a => a.AgeYears <= maxAge);
            if (shelterId is not null) q = q.Where(a => a.ShelterId == shelterId);
            if (!string.IsNullOrWhiteSpace(search)) q = q.Where(a => a.Name.Contains(search));

            var total = await q.CountAsync();
            var items = await q.OrderBy(a => a.Id)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .Select(a => new
                               {
                                   a.Id,
                                   a.Name,
                                   a.Species,
                                   a.AgeYears,
                                   a.Gender,
                                   a.PhotoPath,
                                   Breed = a.Breed != null ? a.Breed.Name : null,
                                   Shelter = a.Shelter.Name,
                                   a.ShelterId
                               })
                               .ToListAsync();

            return Ok(new { total, page, pageSize, items });
        }
    }
}
