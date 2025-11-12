using System.ComponentModel.DataAnnotations;

namespace web_petshelter.Models;

public class Adoption
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Animal")]
    public int AnimalId { get; set; }

    public Animal Animal { get; set; } = null!;

    [Required, StringLength(120)]
    public string AdopterName { get; set; } = string.Empty;

    [EmailAddress, StringLength(120)]
    public string? AdopterEmail { get; set; }

    public DateTime AdoptedAt { get; set; } = DateTime.UtcNow;
}
