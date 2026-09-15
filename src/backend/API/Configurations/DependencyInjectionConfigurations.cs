using EnterpriseFlow.Domain.SeedWork;
using EnterpriseFlow.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

namespace EnterpriseFlow.API.Configurations;

/// <summary>
/// Extensões do IServiceCollection para configuração de injeção de dependências.
/// </summary>
public static class DependencyInjectionConfigurations
{
    /// <summary>
    /// Registra o DbContext, o MediatR e as demais dependências da aplicação.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Information("Starting DependencyInjectionConfigurations.ConfigureDependencyInjection");

        Assembly domainAssembly = typeof(IAggregateRoot).Assembly;

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(domainAssembly));

        Log.Information("Finishing DependencyInjectionConfigurations.ConfigureDependencyInjection");

        return services;
    }
}
