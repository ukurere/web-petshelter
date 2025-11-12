using System.ComponentModel.DataAnnotations;

namespace web_petshelter.Models;

public class Shelter
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Address { get; set; }

    [StringLength(120)]
    public string? City { get; set; }

    public double? Lat { get; set; }
    public double? Lng { get; set; }

    [StringLength(120)]
    public string? Phone { get; set; }

    [StringLength(400)]
    public string? PhotoUrl { get; set; }

    public string? WorkHours { get; set; }

    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

    public List<Animal> Animals { get; set; } = new();
}
