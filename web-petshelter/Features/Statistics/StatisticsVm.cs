namespace web_petshelter.Features.Statistics;
public sealed class StatisticsVm
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int TotalShelters { get; set; }
    public int TotalAnimals { get; set; }
    public int AdoptionsThisPeriod { get; set; }
    public ChartVm Chart { get; set; } = new();
}