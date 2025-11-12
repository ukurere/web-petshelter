namespace web_petshelter.Features.Statistics
{
    public sealed class ChartVm
    {
        public List<string> Labels { get; set; } = new();
        public List<int> Data { get; set; } = new();
        public string DatasetLabel { get; set; } = "Adoptions";
    }
}
