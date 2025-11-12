using Microsoft.EntityFrameworkCore;
using web_petshelter.Data;

namespace web_petshelter.Features.Statistics;

public class StatisticsService
{
    private readonly AppDbContext _db;
    public StatisticsService(AppDbContext db) => _db = db;

    public async Task<StatisticsVm> GetAsync(DateTime? from, DateTime? to)
    {
        // TEMP: без фільтра за датами — просто загальні тотали
        var totalShelters = await _db.Shelters.CountAsync();
        var totalAnimals = await _db.Animals.CountAsync();
        var adoptionsCount = await _db.Adoptions.CountAsync();

        // Порожній/заглушковий ряд для графіка на останні 6 місяців
        var end = DateTime.UtcNow.Date;
        var start = new DateTime(end.Year, end.Month, 1).AddMonths(-5);
        var labels = new List<string>();
        var data = new List<int>();

        var cursor = start;
        while (cursor <= end)
        {
            labels.Add(cursor.ToString("MMM yyyy"));
            data.Add(0); // поки що без реальних дат в БД
            cursor = cursor.AddMonths(1);
        }

        return new StatisticsVm
        {
            From = from,
            To = to,
            TotalShelters = totalShelters,
            TotalAnimals = totalAnimals,
            AdoptionsThisPeriod = adoptionsCount,
            Chart = new ChartVm { Labels = labels, Data = data, DatasetLabel = "Adoptions" }
        };
    }
}