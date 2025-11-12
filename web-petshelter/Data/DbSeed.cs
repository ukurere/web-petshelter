// Data/DbSeed.cs
using Microsoft.EntityFrameworkCore;
using web_petshelter.Models;

namespace web_petshelter.Data;

public static class DbSeed
{
    public static async Task SeedSheltersAsync(AppDbContext db)
    {
        if (await db.Shelters.AnyAsync()) return;

        db.Shelters.AddRange(
            new Shelter { Name = "Kyiv Paws", City = "Kyiv", Address = "Khreshchatyk 1", Lat = 50.4501, Lng = 30.5234, Phone = "+380441112233" },
            new Shelter { Name = "Lviv Tails", City = "Lviv", Address = "Svobody Ave 10", Lat = 49.8397, Lng = 24.0297 },
            new Shelter { Name = "Odesa Care", City = "Odesa", Address = "Deribasivska 5", Lat = 46.4825, Lng = 30.7233 },
            new Shelter { Name = "Kharkiv Home", City = "Kharkiv", Address = "Sumska 20", Lat = 49.9935, Lng = 36.2304 },
            new Shelter { Name = "Dnipro Help", City = "Dnipro", Address = "Dmytra Yavornytskoho 12", Lat = 48.4647, Lng = 35.0462 },
            new Shelter { Name = "Vinnytsia Friends", City = "Vinnytsia", Address = "Soborna 25", Lat = 49.2331, Lng = 28.4682 },
            new Shelter { Name = "Ternopil Shelter", City = "Ternopil", Address = "Ruska 15", Lat = 49.5535, Lng = 25.5948 },
            new Shelter { Name = "Rivne Paws", City = "Rivne", Address = "Soborna 30", Lat = 50.6199, Lng = 26.2516 },
            new Shelter { Name = "Zaporizhzhia Care", City = "Zaporizhzhia", Address = "Sobornyi Ave 100", Lat = 47.8388, Lng = 35.1396 },
            new Shelter { Name = "Chernihiv Tails", City = "Chernihiv", Address = "Hoholya 3", Lat = 51.4982, Lng = 31.2893 }
        );

        await db.SaveChangesAsync();
    }
}
