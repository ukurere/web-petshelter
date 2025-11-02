using System.ComponentModel.DataAnnotations;

namespace web_petshelter.Models;

public class Animal
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = "";

    [Required, StringLength(50)]
    public string Species { get; set; } = ""; // Cat/Dog…

    public int AgeYears { get; set; }

    [Display(Name = "Photo")]
    public string? PhotoPath { get; set; } // /uploads/.. у wwwroot

    [Display(Name = "Shelter")]
    public int ShelterId { get; set; }
    public Shelter? Shelter { get; set; }

    public List<Adoption> Adoptions { get; set; } = new();
}
