namespace web_petshelter.Models;

public class Breed
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Species { get; set; } = ""; // Dog/Cat/etc.

    public ICollection<Animal> Animals { get; set; } = new List<Animal>();
}
