using System;
using System.Threading.Tasks;
using HighCapitalBot.Core.Data;
using HighCapitalBot.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HighCapitalBot.API.Data;

public static class DatabaseInitializer
{
    public static void SeedDatabase(IServiceProvider services)
    {
        try
        {
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var configuration = services.GetRequiredService<IConfiguration>();
            
            // Inicializa os dados iniciais de forma síncrona
            SeedRoles(roleManager);
            SeedAdminUser(userManager, configuration);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
    
    private static void SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "Admin", "User" };
        
        foreach (var role in roles)
        {
            if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
            }
        }
    }
    
    private static void SeedAdminUser(UserManager<AppUser> userManager, IConfiguration configuration)
    {
        var adminEmail = configuration["AdminUser:Email"] ?? "admin@highcapitalbot.com";
        var adminPassword = configuration["AdminUser:Password"] ?? "Admin@123";
        
        if (userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult() == null)
        {
            var adminUser = new AppUser
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true
            };
            
            var result = userManager.CreateAsync(adminUser, adminPassword).GetAwaiter().GetResult();
            
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
            }
        }
    }
}
