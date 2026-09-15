using EnterpriseFlow.Infra.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

namespace EnterpriseFlow.API.Configurations;

/// <summary>
/// Extensões do IServiceCollection/IApplicationBuilder para configuração de health checks.
/// </summary>
public static class HealthCheckConfigurations
{
    /// <summary>
    /// Registra os health checks da aplicação (banco de dados e, futuramente, cache/mensageria).
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureHealthChecks(this IServiceCollection services)
    {
        Log.Information("Starting HealthCheckConfigurations.ConfigureHealthChecks");

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>(name: "sql-server");

        Log.Information("Finishing HealthCheckConfigurations.ConfigureHealthChecks");

        return services;
    }

    /// <summary>
    /// Expõe o endpoint /health. Em Development retorna o detalhe de cada dependência verificada;
    /// nos demais ambientes retorna apenas o status geral, evitando expor nomes/topologia de
    /// dependências internas (SQL Server, Redis, RabbitMQ, etc.) publicamente.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseHealthChecks(this WebApplication app)
    {
        var options = app.Environment.IsDevelopment()
            ? new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse }
            : new HealthCheckOptions { ResponseWriter = WriteMinimalStatusAsync };

        app.MapHealthChecks("/health", options);

        return app;
    }

    private static Task WriteMinimalStatusAsync(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync($"{{\"status\":\"{report.Status}\"}}");
    }
}
