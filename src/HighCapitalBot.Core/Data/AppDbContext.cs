using HighCapitalBot.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HighCapitalBot.Core.Data;

public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
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
        modelBuilder.Entity<User>()
            .HasMany(u => u.Bots)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // Configurações adicionais para o Identity
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            entity.Property(u => u.UserName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.PasswordHash).IsRequired();
        });
    }
}
