using Microsoft.AspNetCore.Identity;
using TaskManager.Models;

namespace TaskManager.Data
{
    /// <summary>
    /// Seeds predefined application roles and creates default SuperAdmin and Admin users.
    /// </summary>
    public class RoleSeeder
    {
        /// <summary>
        /// Seeds application roles (SuperAdmin, Admin, User) and creates default SuperAdmin and Admin accounts if they don't exist.
        /// </summary>
        /// <param name="roleManager">Provides APIs for managing roles.</param>
        /// <param name="userManager">Provides APIs for managing users.</param>
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {   
            string[] roles = { "SuperAdmin", "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            string superAdminEmail = "superadmin@taskmanager.com";
            string superAdminPassword = "SuperAdmin@123";

            var superAdmin = await userManager.FindByEmailAsync(superAdminEmail);
            if (superAdmin == null)
            {
                var newSuperAdmin = new ApplicationUser
                {
                    UserName = superAdminEmail,
                    Email = superAdminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(newSuperAdmin, superAdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newSuperAdmin, "SuperAdmin");
                }
            }

            string adminEmail = "admin@taskmanager.com";
            string adminPassword = "Admin@123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(newAdmin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
