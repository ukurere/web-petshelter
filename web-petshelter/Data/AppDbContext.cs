using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using web_petshelter.Models;

namespace web_petshelter.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Animal> Animals => Set<Animal>();
    public DbSet<Breed> Breeds => Set<Breed>();
    public DbSet<Shelter> Shelters => Set<Shelter>();
    public DbSet<Adoption> Adoptions => Set<Adoption>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<VolunteerTask> VolunteerTasks => Set<VolunteerTask>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.Entity<Animal>()
            .HasOne(a => a.Shelter)
            .WithMany(s => s.Animals)
            .HasForeignKey(a => a.ShelterId)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<Animal>()
            .HasOne(a => a.Breed)
            .WithMany()            
            .HasForeignKey(a => a.BreedId)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<Adoption>()
            .HasOne(a => a.Animal)
            .WithMany(an => an.Adoptions)   // <- прив’язуємо до колекції на Animal
            .HasForeignKey(a => a.AnimalId)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<Shelter>(e =>
        {
            e.Property(x => x.Name).HasMaxLength(120).IsRequired();
            e.Property(x => x.Address).HasMaxLength(200);
            e.Property(x => x.City).HasMaxLength(120);
            e.Property(x => x.Phone).HasMaxLength(120);
            e.Property(x => x.PhotoUrl).HasMaxLength(400);
        });

        b.Entity<TaskItem>(e =>
        {
            e.HasIndex(x => new { x.Status, x.DueAt });
            e.HasIndex(x => new { x.AssigneeUserId, x.Status });
            e.Property(x => x.Title).IsRequired().HasMaxLength(160);
            e.Property(x => x.Description).HasMaxLength(2000);
        });
    }
}
