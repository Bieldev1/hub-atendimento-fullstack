using Serilog;

namespace EnterpriseFlow.API.Configurations;

/// <summary>
/// Extensões para configuração de logging estruturado (Serilog).
/// </summary>
public static class LogConfigurations
{
    /// <summary>
    /// Configura o Serilog como provedor de log da aplicação.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder ConfigureLog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "EnterpriseFlow.API")
                .WriteTo.Console();
        });

        return builder;
    }
}
