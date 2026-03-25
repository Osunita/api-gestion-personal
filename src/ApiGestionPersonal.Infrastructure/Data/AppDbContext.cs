using Microsoft.EntityFrameworkCore;
using ApiGestionPersonal.Domain.Entities;

namespace ApiGestionPersonal.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply all configurations from the Entities folder
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        // Global query filter for soft delete
        modelBuilder.Entity<TaskItem>().HasQueryFilter(t => t.DeletedAt == null);
        modelBuilder.Entity<Note>().HasQueryFilter(n => n.DeletedAt == null);
        modelBuilder.Entity<Category>().HasQueryFilter(c => c.DeletedAt == null);
        modelBuilder.Entity<User>().HasQueryFilter(u => u.DeletedAt == null);
    }
}