using EnterpriseFlow.Domain.Common.Results;
using System.Text.Json;

namespace EnterpriseFlow.API.Middlewares;

/// <summary>
/// Captura exceções não tratadas no pipeline e responde com um <see cref="Result"/> padronizado,
/// evitando vazar stack trace ao cliente.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var result = Result.Fail(ResultCode.GenericError, "Ocorreu um erro inesperado ao processar a requisição.");

        logger.LogError(exception, "Erro não tratado. Protocol: {Protocol}", result.Protocol);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)result.ResultCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            message = result.Message,
            protocol = result.Protocol
        }));
    }
}
