using Microsoft.EntityFrameworkCore;
using web_petshelter.Models;

namespace web_petshelter.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Shelter> Shelters => Set<Shelter>();
    public DbSet<Animal> Animals => Set<Animal>();
    public DbSet<Adoption> Adoptions => Set<Adoption>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.Entity<Shelter>()
            .HasMany(s => s.Animals)
            .WithOne(a => a.Shelter!)
            .HasForeignKey(a => a.ShelterId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<Adoption>()
            .HasOne(x => x.Animal)
            .WithMany(a => a.Adoptions)
            .HasForeignKey(x => x.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
