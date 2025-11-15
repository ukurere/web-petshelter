using System;
using System.Collections.Generic;

namespace web_petshelter.Features.Statistics
{
    public class StatisticsVm
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public int TotalShelters { get; set; }
        public int TotalAnimals { get; set; }
        public int AdoptionsThisPeriod { get; set; }

        public StatisticsChartVm Chart { get; set; } = new();
    }

    public class StatisticsChartVm
    {
        public List<string> Labels { get; set; } = new();
        public List<int> Data { get; set; } = new();
        public string DatasetLabel { get; set; } = "Adoptions";
    }
}
