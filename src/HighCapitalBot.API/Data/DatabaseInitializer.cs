using HighCapitalBot.Core.Data;
using Microsoft.EntityFrameworkCore;

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
            
            // Aplica as migrações pendentes
            await context.Database.MigrateAsync();
            
            // Aqui você pode adicionar dados iniciais se necessário
            // await SeedDataAsync(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }
    
    // Método opcional para popular dados iniciais
    // private static async Task SeedDataAsync(AppDbContext context)
    // {
    //     // Adicione aqui a lógica para popular dados iniciais
    //     if (!await context.Bots.AnyAsync())
    //     {
    //         // Exemplo de bot inicial
    //         var defaultBot = new Bot
    //         {
    //             Name = "Assistente Padrão",
    //             Description = "Um assistente útil para responder suas perguntas",
    //             InitialContext = "Você é um assistente prestativo que responde de forma clara e concisa.",
    //             CreatedAt = DateTime.UtcNow
    //         };
    //         
    //         await context.Bots.AddAsync(defaultBot);
    //         await context.SaveChangesAsync();
    //     }
    // }
}
