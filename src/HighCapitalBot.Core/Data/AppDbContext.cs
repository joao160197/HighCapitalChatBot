using HighCapitalBot.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HighCapitalBot.Core.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Bot> Bots { get; set; } = null!;
    public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configurações adicionais do modelo podem ser feitas aqui
        modelBuilder.Entity<Bot>()
            .HasMany(b => b.Messages)
            .WithOne(m => m.Bot)
            .HasForeignKey(m => m.BotId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
