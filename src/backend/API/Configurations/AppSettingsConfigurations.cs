using Serilog;

namespace EnterpriseFlow.API.Configurations;

/// <summary>
/// Extensões do IConfigurationBuilder para configuração dos arquivos de appsettings e variáveis de ambiente.
/// </summary>
public static class AppSettingsConfigurations
{
    /// <summary>
    /// Adiciona os arquivos de appsettings por ambiente e as variáveis de ambiente à configuração.
    /// </summary>
    /// <param name="configurationBuilder"></param>
    /// <param name="environment"></param>
    /// <returns></returns>
    public static IConfigurationBuilder ConfigureAppSettings(this IConfigurationBuilder configurationBuilder, IHostEnvironment environment)
    {
        Log.Information("Starting AppSettingsConfigurations.ConfigureAppSettings");

        configurationBuilder
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        Log.Information("Finishing AppSettingsConfigurations.ConfigureAppSettings");

        return configurationBuilder;
    }
}
