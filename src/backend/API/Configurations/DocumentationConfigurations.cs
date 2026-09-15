using Microsoft.OpenApi.Models;
using Serilog;

namespace EnterpriseFlow.API.Configurations;

/// <summary>
/// Extensões do IServiceCollection/IApplicationBuilder para configuração do Swagger.
/// https://docs.microsoft.com/en-US/aspnet/core/tutorials/web-api-help-pages-using-swagger
/// </summary>
public static class DocumentationConfigurations
{
    private const string UriFriendlyName = "v1";

    /// <summary>
    /// Adiciona e configura o gerador do Swagger, agrupando as operações por controller (tag)
    /// e habilitando o filtro de busca por tag na UI.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        Log.Information("Starting DocumentationConfigurations.ConfigureSwagger");

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(UriFriendlyName, new OpenApiInfo
            {
                Title = "EnterpriseFlow API",
                Version = UriFriendlyName,
                Description = "API de gestão de pedidos do EnterpriseFlow."
            });

            // Agrupa (tageia) cada operação pelo nome do controller, permitindo filtrar por ele na UI.
            options.TagActionsBy(api => new[] { api.ActionDescriptor.RouteValues["controller"] ?? "Default" });
            options.DocInclusionPredicate((_, _) => true);

            var xmlFile = $"{typeof(DocumentationConfigurations).Assembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath);

            options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Scheme = "Bearer",
                In = ParameterLocation.Header,
                Description = "Informe o token JWT no formato: Bearer {token}",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });

        Log.Information("Finishing DocumentationConfigurations.ConfigureSwagger");

        return services;
    }

    /// <summary>
    /// Habilita o middleware do Swagger e a UI, com o filtro de busca por tag habilitado.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseDocumentation(this IApplicationBuilder app)
    {
        Log.Information("Starting DocumentationConfigurations.UseDocumentation");

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/swagger/{UriFriendlyName}/swagger.json", "EnterpriseFlow API");
            options.DefaultModelsExpandDepth(-1);
            options.DisplayRequestDuration();
            options.EnableFilter();
        });

        Log.Information("Finishing DocumentationConfigurations.UseDocumentation");

        return app;
    }
}
