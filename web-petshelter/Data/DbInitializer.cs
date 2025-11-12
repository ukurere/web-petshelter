using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using web_petshelter.Models;

namespace web_petshelter.Data;

public static class DbInitializer
{
    private const string AdminEmail = "admin@example.com";
    private const string AdminPassword = "Passw0rd!"; // відповідає дефолтним вимогам

    public static async Task SeedAsync(IServiceProvider services, IHostEnvironment env)
    {
        using var scope = services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var users = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roles = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // 1) Міграції
        // Якщо під час деву отримуєш помилку про зміну IDENTITY — це наслідок несумісних попередніх міграцій.
        // У дев-оточенні дозволяємо «скидати» БД (опційно зніми, якщо не треба).
        try
        {
            await ctx.Database.MigrateAsync();
        }
        catch (Exception) when (env.IsDevelopment())
        {
            // ⚠️ Лише локально: дроп і повна міграція з нуля, щоб уникнути
            // "To change the IDENTITY property ..." на зламаних історіях.
            await ctx.Database.EnsureDeletedAsync();
            await ctx.Database.MigrateAsync();
        }

        // 2) Ролі
        var requiredRoles = new[] { "Admin" };
        foreach (var r in requiredRoles)
            if (!await roles.RoleExistsAsync(r))
                await roles.CreateAsync(new IdentityRole(r));

        // 3) Адмін-користувач
        var admin = await users.FindByEmailAsync(AdminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = AdminEmail,
                Email = AdminEmail,
                EmailConfirmed = true
            };
            var createRes = await users.CreateAsync(admin, AdminPassword);
            if (!createRes.Succeeded)
            {
                var msg = string.Join("; ", createRes.Errors.Select(e => $"{e.Code}:{e.Description}"));
                throw new InvalidOperationException($"Failed to create admin: {msg}");
            }
            await users.AddToRoleAsync(admin, "Admin");
        }

        // 4) Тут — інші сид-дані (Shelters, Breeds, Animals, ...)
        // Додавай лише якщо їх немає, щоб сид був ідемпотентний.
        // if (!await ctx.Shelters.AnyAsync()) { ... }

        await ctx.SaveChangesAsync();
    }
}
