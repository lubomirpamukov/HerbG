namespace Herbg.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Herbg.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

public static class ApplicationDbInitializer
{
    public static void SeedAdminUser(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string adminRole = "Admin";
        string adminEmail = "admin@herbg.com";
        string adminPassword = "Admin@123";

        // Check if the Admin role exists; if not, create it
        if (!roleManager.RoleExistsAsync(adminRole).Result)
        {
            roleManager.CreateAsync(new IdentityRole(adminRole)).Wait();
        }

        // Check if the admin user exists; if not, create it
        var adminUser = userManager.FindByEmailAsync(adminEmail).Result;
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = userManager.CreateAsync(adminUser, adminPassword).Result;
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(adminUser, adminRole).Wait();
            }
        }
    }

}
