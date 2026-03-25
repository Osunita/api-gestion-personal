using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ApiGestionPersonal.Application.Common.Interfaces;
using ApiGestionPersonal.Infrastructure.Data;
using ApiGestionPersonal.Infrastructure.Repositories;
using ApiGestionPersonal.Infrastructure.Services;

namespace ApiGestionPersonal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString,
        bool isDevelopment,
        string jwtKey,
        string jwtIssuer,
        string jwtAudience,
        int jwtExpiresInMinutes = 60)
    {
        // Add DbContext
        services.AddDbContext<AppDbContext>(options =>
        {
            if (isDevelopment)
            {
                options.UseSqlite(connectionString);
            }
            else
            {
                options.UseSqlServer(connectionString);
            }
        });

        // Add repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        
        // Add Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // Add Services
        services.AddScoped<IJwtService>(sp => 
            new JwtService(jwtKey, jwtIssuer, jwtAudience, jwtExpiresInMinutes));
        services.AddScoped<ICategorizationService, KeywordCategorizationService>();

        return services;
    }
}