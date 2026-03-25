using Microsoft.EntityFrameworkCore;
using ApiGestionPersonal.Infrastructure.Data;

namespace ApiGestionPersonal.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=ApiGestionPersonal.db");
        return new AppDbContext(optionsBuilder.Options);
    }
}