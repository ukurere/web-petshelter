using System.ComponentModel.DataAnnotations;

namespace web_petshelter.Models;

public class Shelter
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = "";

    [StringLength(200)]
    public string? Address { get; set; }

    [StringLength(120)]
    public string? City { get; set; }

    public List<Animal> Animals { get; set; } = new();
}
