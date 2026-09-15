using Serilog;

namespace EnterpriseFlow.API.Configurations;

/// <summary>
/// Extensões do IServiceCollection para configuração de CORS (Cross-Origin Resource Sharing).
/// https://docs.microsoft.com/en-US/aspnet/core/security/cors
/// </summary>
public static class CorsConfigurations
{
    public const string DefaultPolicyName = "EnterpriseFlowDefault";

    private static readonly string[] DefaultAllowedOrigins =
    {
        "http://localhost:5173", // Frontend (Vite - dev local)
        "http://localhost:3000"  // Frontend (Docker)
    };

    /// <summary>
    /// Configura a política de CORS liberando apenas as origens conhecidas do front-end.
    /// As origens podem ser sobrescritas via configuração (seção "Cors:AllowedOrigins").
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Information("Starting CorsConfigurations.ConfigureCors");

        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
        allowedOrigins = allowedOrigins is { Length: > 0 } ? allowedOrigins : DefaultAllowedOrigins;

        services.AddCors(options =>
        {
            options.AddPolicy(DefaultPolicyName, policy => policy
                .WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader());
        });

        Log.Information("Finishing CorsConfigurations.ConfigureCors. Allowed origins: {AllowedOrigins}", string.Join(", ", allowedOrigins));

        return services;
    }
}
