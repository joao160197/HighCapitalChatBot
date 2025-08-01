using HighCapitalBot.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HighCapitalBot.Core.Data;

public class AppDbContext : IdentityDbContext<AppUser>
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
            
        // Configuração do relacionamento entre User e Bot
        modelBuilder.Entity<AppUser>()
            .HasMany(u => u.Bots)
            .WithOne(b => b.AppUser)
            .HasForeignKey(b => b.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        
    }
}
