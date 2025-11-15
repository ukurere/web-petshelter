using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using web_petshelter.Models;

namespace web_petshelter.Infrastructure
{
    public static class IdentitySeed
    {
        private const string AdminEmail = "yevgieniia.riabichenko@gmail.com";
        private const string AdminRoleName = "Admin";

        // Якщо треба автоматично створити користувача – задай тут пароль
        private const string DefaultAdminPassword = "Admin123$"; // можна змінити або прибрати

        public static async Task SeedAdminAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. Створюємо роль Admin, якщо її ще немає
            if (!await roleManager.RoleExistsAsync(AdminRoleName))
            {
                await roleManager.CreateAsync(new IdentityRole(AdminRoleName));
            }

            // 2. Знаходимо користувача з цим емейлом
            var adminUser = await userManager.FindByEmailAsync(AdminEmail);

            // Якщо такого користувача ще немає — створимо
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = AdminEmail,
                    Email = AdminEmail,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(adminUser, DefaultAdminPassword);

                if (!createResult.Succeeded)
                {
                    // Якщо створити не вдалось — просто виходимо, щоб не падало
                    return;
                }
            }

            // 3. Додаємо йому роль Admin, якщо ще не додана
            if (!await userManager.IsInRoleAsync(adminUser, AdminRoleName))
            {
                await userManager.AddToRoleAsync(adminUser, AdminRoleName);
            }
        }
    }
}
