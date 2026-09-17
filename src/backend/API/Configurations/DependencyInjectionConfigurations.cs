using EnterpriseFlow.Domain.AggregatesModel.PedidoAggregate;
using EnterpriseFlow.Domain.SeedWork;
using EnterpriseFlow.Infra.Data;
using EnterpriseFlow.Infra.Data.Repositories;
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
        Assembly apiAssembly = typeof(DependencyInjectionConfigurations).Assembly;

        services.AddDbContext<EnterpriseFlowDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();

        // Commands, Queries e seus Handles/EventHandlers ficam em API/Application (mesmo assembly da API).
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(domainAssembly, apiAssembly));

        Log.Information("Finishing DependencyInjectionConfigurations.ConfigureDependencyInjection");

        return services;
    }
}
