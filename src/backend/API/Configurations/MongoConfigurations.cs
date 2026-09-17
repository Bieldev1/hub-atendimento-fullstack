using EnterpriseFlow.Domain.AggregatesModel.RastreamentoAggregate;
using EnterpriseFlow.Infra.Data;
using EnterpriseFlow.Infra.Data.Options;
using EnterpriseFlow.Infra.Data.Repositories;
using Serilog;

namespace EnterpriseFlow.API.Configurations;

/// <summary>
/// Extensões do IServiceCollection para configuração do MongoDB (histórico de rastreamento).
/// </summary>
public static class MongoConfigurations
{
    public static IServiceCollection ConfigureMongo(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Information("Starting MongoConfigurations.ConfigureMongo");

        services.Configure<MongoOptions>(options => configuration.GetSection("Mongo").Bind(options));

        services.AddSingleton<MongoContext>();
        services.AddScoped<IRastreamentoRepository, RastreamentoRepository>();

        Log.Information("Finishing MongoConfigurations.ConfigureMongo");

        return services;
    }
}
