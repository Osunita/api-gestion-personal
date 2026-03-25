using Microsoft.Extensions.DependencyInjection;
using ApiGestionPersonal.Application.Common.Interfaces;

namespace ApiGestionPersonal.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        
        return services;
    }
}