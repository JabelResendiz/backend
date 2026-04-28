using System.Net;
using System.Text.Json;

namespace Finlay.PharmaVigilance.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        string type = "InternalServerError";
        string message = "An unexpected error occurred.";

        // Mapear tipos de excepción a HTTP Status Codes y tipos
        switch (exception)
        {
            case KeyNotFoundException:
                code = HttpStatusCode.NotFound;
                type = "NotFound";
                message = exception.Message;
                break;

            case ArgumentNullException:
            case ArgumentException:
                code = HttpStatusCode.BadRequest;
                type = "ValidationError";
                message = exception.Message;
                break;

            case InvalidOperationException:
                code = HttpStatusCode.BadRequest;
                type = "OperationError";
                message = exception.Message;
                break;

            default:
                // Para excepciones inesperadas, deja el mensaje genérico o agrega detalles según dev/prod
                message = exception.Message;
                break;
        }

        var result = JsonSerializer.Serialize(new
        {
            message,
            type
        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}