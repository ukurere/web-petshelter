using web_petshelter.Models;

namespace web_petshelter.Models.ViewModels;

public class AnimalFilterVm
{
    public string? Species { get; set; }
    public int? BreedId { get; set; }
    public Gender? Gender { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public int? ShelterId { get; set; }
    public bool? Adopted { get; set; }
    public string? Search { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;

    public IEnumerable<Breed> Breeds { get; set; } = Enumerable.Empty<Breed>();
    public IEnumerable<Shelter> Shelters { get; set; } = Enumerable.Empty<Shelter>();
    public IEnumerable<Animal> Items { get; set; } = Enumerable.Empty<Animal>();
    public int Total { get; set; }
}
