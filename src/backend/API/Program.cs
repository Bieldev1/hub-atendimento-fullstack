using EnterpriseFlow.API.Configurations;
using EnterpriseFlow.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.ConfigureAppSettings(builder.Environment);
builder.ConfigureLog();

builder.Services.AddControllers();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureDependencyInjection(builder.Configuration);
builder.Services.ConfigureCors(builder.Configuration);
builder.Services.ConfigureHealthChecks();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
    app.UseDocumentation();

app.UseCors(CorsConfigurations.DefaultPolicyName);
app.UseAuthorization();
app.MapControllers();
app.UseHealthChecks();

app.Run();
