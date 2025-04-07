using BaseEntity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BaseEntity.Configurations
{
    public static class AdminConfigurations
    {
        public static async Task CreateAdminUser(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            var adminSection = config.GetSection("AdminUser");
            string adminEmail = adminSection["Email"] ?? throw new Exception("Admin email is missing in appsettings.");
            string adminPassword = adminSection["Password"] ?? throw new Exception("Admin password is missing in appsettings.");
            string adminRole = adminSection["Role"] ?? "admin";
            string fullName = "Admin";


            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }


            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail,FullName = fullName };
                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                    Console.WriteLine("Admin user created successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to create admin user: " + string.Join(", ", result.Errors));
                }
            }
            else
            {
                Console.WriteLine("Admin user already exists.");
            }
        }
    }
}
