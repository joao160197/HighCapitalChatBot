using System.Text;
using HighCapitalBot.Core.Data;
using HighCapitalBot.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HighCapitalBot.API.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        
        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var configuration = services.GetRequiredService<IConfiguration>();
            
            // Aplica as migrações pendentes
            await context.Database.MigrateAsync();
            
            // Inicializa os dados iniciais
            await SeedRolesAsync(roleManager);
            await SeedAdminUserAsync(userManager, configuration);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }
    
    private static async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager)
    {
        string[] roles = { "Admin", "User" };
        
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole<int>(role));
        }
    }
    
    private static async Task SeedAdminUserAsync(UserManager<User> userManager, IConfiguration configuration)
    {
        var adminEmail = configuration["AdminUser:Email"] ?? "admin@highcapitalbot.com";
        var adminPassword = configuration["AdminUser:Password"] ?? "Admin@123";
        
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new User
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true
            };
            
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
}
