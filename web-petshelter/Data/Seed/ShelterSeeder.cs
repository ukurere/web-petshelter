using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using web_petshelter.Data;
using web_petshelter.Models;

namespace web_petshelter.Data.Seed
{
    public static class ShelterSeeder
    {
        public static async Task EnsureSeededAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // вже є дані — виходимо
            if (await db.Shelters.AnyAsync()) return;

            var now = DateTime.UtcNow;

            var items = new List<Shelter>
            {
                new()
                {
                    Name = "Kyiv Paws",
                    Address = "Kyiv, H. Skovorody 12",
                    Lat = 50.4509, Lng = 30.5234,
                    Phone = "+380441112233",
                    WorkHours = "Mon–Fri 10:00–18:00; Sat 11:00–16:00",
                    IsDeleted = false, LastModifiedAt = now, LastModifiedBy = "seed"
                },
                new()
                {
                    Name = "Lviv Tail Haven",
                    Address = "Lviv, Zelena 15",
                    Lat = 49.8397, Lng = 24.0297,
                    Phone = "+380322004455",
                    WorkHours = "Daily 10:00–17:00",
                    IsDeleted = false, LastModifiedAt = now, LastModifiedBy = "seed"
                },
                new()
                {
                    Name = "Kharkiv Hope Shelter",
                    Address = "Kharkiv, Nauky Ave 90",
                    Lat = 49.9935, Lng = 36.2304,
                    Phone = "+380572300777",
                    WorkHours = "Mon–Fri 09:00–18:00",
                    IsDeleted = false, LastModifiedAt = now, LastModifiedBy = "seed"
                },
                new()
                {
                    Name = "Odesa SeaCats",
                    Address = "Odesa, Deribasivska 8",
                    Lat = 46.4825, Lng = 30.7233,
                    Phone = "+380482555990",
                    WorkHours = "Tue–Sun 11:00–19:00",
                    IsDeleted = false, LastModifiedAt = now, LastModifiedBy = "seed"
                },
                new()
                {
                    Name = "Dnipro Safe Paws",
                    Address = "Dnipro, Yavornytskoho 101",
                    Lat = 48.4670, Lng = 35.0400,
                    Phone = "+380567700221",
                    WorkHours = "Mon–Fri 10:00–18:30",
                    IsDeleted = false, LastModifiedAt = now, LastModifiedBy = "seed"
                },
                // ще кілька — опційно
                new()
                {
                    Name = "Zaporizhzhia Warm Home",
                    Address = "Zaporizhzhia, Sobornyi 120",
                    Lat = 47.8388, Lng = 35.1396,
                    Phone = "+380612203344",
                    WorkHours = "Daily 10:00–18:00",
                    IsDeleted = false, LastModifiedAt = now, LastModifiedBy = "seed"
                },
                new()
                {
                    Name = "Vinnytsia PetCare",
                    Address = "Vinnytsia, Soborna 45",
                    Lat = 49.2331, Lng = 28.4682,
                    Phone = "+380432600700",
                    WorkHours = "Mon–Sat 09:30–18:00",
                    IsDeleted = false, LastModifiedAt = now, LastModifiedBy = "seed"
                },
                new()
                {
                    Name = "Poltava PawFriends",
                    Address = "Poltava, Sobornosti 10",
                    Lat = 49.5883, Lng = 34.5514,
                    Phone = "+380532550880",
                    WorkHours = "Mon–Fri 10:00–17:00",
                    IsDeleted = false, LastModifiedAt = now, LastModifiedBy = "seed"
                },
            };

            await db.Shelters.AddRangeAsync(items);
            await db.SaveChangesAsync();
        }
    }
}
