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


        private const string DefaultAdminPassword = "Admin123$"; 
        public static async Task SeedAdminAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(AdminRoleName))
            {
                await roleManager.CreateAsync(new IdentityRole(AdminRoleName));
            }

            var adminUser = await userManager.FindByEmailAsync(AdminEmail);

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
                    return;
                }
            }

            if (!await userManager.IsInRoleAsync(adminUser, AdminRoleName))
            {
                await userManager.AddToRoleAsync(adminUser, AdminRoleName);
            }
        }
    }
}
