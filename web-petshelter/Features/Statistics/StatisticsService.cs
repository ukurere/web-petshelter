using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using web_petshelter.Data;

namespace web_petshelter.Features.Statistics
{
    public class StatisticsService
    {
        private readonly AppDbContext _db;

        public StatisticsService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<StatisticsVm> GetAsync(DateTime? from, DateTime? to)
        {
            var sheltersQuery = _db.Shelters.AsNoTracking();
            var animalsQuery = _db.Animals.AsNoTracking();
            var adoptionsQuery = _db.Adoptions.AsNoTracking();

            if (from.HasValue)
            {
                adoptionsQuery = adoptionsQuery
                    .Where(a => a.AdoptedAt >= from.Value); 
            }

            if (to.HasValue)
            {
                var endExclusive = to.Value.Date.AddDays(1);  
                adoptionsQuery = adoptionsQuery
                    .Where(a => a.AdoptedAt < endExclusive); 
            }

            var totalShelters = await sheltersQuery.CountAsync();
            var totalAnimals = await animalsQuery.CountAsync();
            var adoptionsInPeriod = await adoptionsQuery.CountAsync();

            var grouped = await adoptionsQuery
                .GroupBy(a => new { a.AdoptedAt.Year, a.AdoptedAt.Month })
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

            return new StatisticsVm
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
        }
    }
}
