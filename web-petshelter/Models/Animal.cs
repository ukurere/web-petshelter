namespace web_petshelter.Models
{
    public enum Gender { Unknown = 0, Male = 1, Female = 2 }

    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Species { get; set; } = "";
        public int AgeYears { get; set; }
        public string? PhotoPath { get; set; }
        public Gender Gender { get; set; } = Gender.Unknown;
        public int ShelterId { get; set; }
        public Shelter Shelter { get; set; } = null!;
        public int? BreedId { get; set; }
        public Breed? Breed { get; set; }
        public ICollection<Adoption> Adoptions { get; set; } = new List<Adoption>();
    }
}
