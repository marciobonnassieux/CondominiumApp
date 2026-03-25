using Edifacil.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Edifacil.Infrastructure.Data;

public class EdifacilDbContext : DbContext
{
    public EdifacilDbContext(DbContextOptions<EdifacilDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Profile> Profiles { get; set; } = null!;
    public DbSet<Unit> Units { get; set; } = null!;
    public DbSet<Delivery> Deliveries { get; set; } = null!;
    public DbSet<CourierLog> CourierLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Profile>().HasKey(p => p.Id);
        
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>()
            .HasOne(u => u.Profile)
            .WithMany(p => p.Users)
            .HasForeignKey(u => u.ProfileId)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Unit>().HasKey(u => u.Id);
        modelBuilder.Entity<Unit>()
            .HasOne(u => u.Resident)
            .WithMany(r => r.Units)
            .HasForeignKey(u => u.ResidentId)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Delivery>().HasKey(d => d.Id);
        modelBuilder.Entity<Delivery>()
            .HasOne(d => d.Unit)
            .WithMany(u => u.Deliveries)
            .HasForeignKey(d => d.UnitId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<Delivery>()
            .HasOne(d => d.Janitor)
            .WithMany()
            .HasForeignKey(d => d.JanitorId)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<CourierLog>().HasKey(c => c.Id);
        modelBuilder.Entity<CourierLog>()
            .HasOne(c => c.Delivery)
            .WithOne(d => d.CourierLog)
            .HasForeignKey<CourierLog>(c => c.DeliveryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

