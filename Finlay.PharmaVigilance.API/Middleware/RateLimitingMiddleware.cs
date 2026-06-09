using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using System.Threading.RateLimiting;

namespace Finlay.PharmaVigilance.Api.Middleware;

public static class RateLimitingMiddleware
{
    public static IServiceCollection AddRateLimitingConfiguration(
        this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            // ==========================================
            // CONFIGURACIÓN GLOBAL
            // ==========================================
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            // ==========================================
            // MANEJO DE RECHAZO MEJORADO
            // ==========================================
            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.ContentType = "application/json";

                // Calcular tiempo de espera
                var retryAfter = TimeSpan.FromMinutes(1).TotalSeconds;
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfterValue))
                {
                    retryAfter = retryAfterValue.TotalSeconds;
                }

                // Headers informativos para el cliente
                context.HttpContext.Response.Headers["Retry-After"] = retryAfter.ToString();
                context.HttpContext.Response.Headers["X-RateLimit-Limit"] = "100";
                context.HttpContext.Response.Headers["X-RateLimit-Remaining"] = "0";
                context.HttpContext.Response.Headers["X-RateLimit-Reset"] =
                    DateTimeOffset.UtcNow.AddSeconds(retryAfter).ToUnixTimeSeconds().ToString();

                // Mensaje más descriptivo
                await context.HttpContext.Response.WriteAsync(
                    $$"""
                    {
                        "success": false,
                        "status": 429,
                        "message": "Demasiadas solicitudes. Intente de nuevo en {{retryAfter}} segundos.",
                        "type": "RateLimitExceeded",
                        "retryAfter": {{retryAfter}}
                    }
                    """,
                    token);
            };

            // ==========================================
            // POLÍTICAS ESPECÍFICAS POR TIPO DE ENDPOINT
            // ==========================================

            // Autenticación: muy restrictivo (5 req/min)
            options.AddFixedWindowLimiter("Auth", config =>
            {
                config.PermitLimit = 5000000;
                config.Window = TimeSpan.FromMinutes(1);
                config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                config.QueueLimit = 0;
            });

            // Endpoints críticos de farmacovigilancia (30 req/min)
            options.AddFixedWindowLimiter("PharmaCritical", config =>
            {
                config.PermitLimit = 3000000;
                config.Window = TimeSpan.FromMinutes(1);
                config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                config.QueueLimit = 5;
            });

            // Consultas generales: más permisivo (100 req/min)
            options.AddSlidingWindowLimiter("GeneralQuery", config =>
            {
                config.PermitLimit = 10000000;
                config.Window = TimeSpan.FromMinutes(1);
                config.SegmentsPerWindow = 3;
                config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                config.QueueLimit = 10;
            });

            // Comandos con límite diario (ej: completar revisiones médicas)
            options.AddSlidingWindowLimiter("CommandDaily", config =>
            {
                config.PermitLimit = 50000000;
                config.Window = TimeSpan.FromDays(1);
                config.SegmentsPerWindow = 24;
                config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                config.QueueLimit = 0;
            });

            // ==========================================
            // LIMITADOR GLOBAL POR IP (MEJORADO)
            // ==========================================
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
                httpContext =>
                {
                    // Usar ID de usuario si está autenticado, IP si es anónimo
                    var userId = httpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var partitionKey = userId ?? GetClientIp(httpContext);

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: partitionKey,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = userId != null ? 2000000 : 10000000, // Más permisivo para autenticados
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        });
                });
        });

        return services;
    }

    private static string GetClientIp(HttpContext context)
    {
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',')[0].Trim();
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}