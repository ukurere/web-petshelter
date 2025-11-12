namespace web_petshelter.Models
{   public class VolunteerTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public bool IsDone { get; set; }
        public int? ShelterId { get; set; }
        public string? Assignee { get; set; } 
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
    }

}
