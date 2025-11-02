using System.ComponentModel.DataAnnotations;

namespace web_petshelter.Models;

public class Adoption
{
    public int Id { get; set; }

    [Display(Name = "Animal")]
    public int AnimalId { get; set; }
    public Animal? Animal { get; set; }

    [Required, StringLength(120)]
    public string AdopterName { get; set; } = "";

    [EmailAddress, StringLength(120)]
    public string? AdopterEmail { get; set; }

    public DateTime AdoptedAt { get; set; } = DateTime.UtcNow;
}
