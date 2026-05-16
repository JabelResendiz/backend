// using Microsoft.AspNetCore.RateLimiting;
// using System.Threading.RateLimiting;

// namespace Finlay.PharmaVigilance.Api.Middleware;

// public static class RateLimitingMiddleware
// {
//     public static IServiceCollection AddRateLimitingConfiguration(
//         this IServiceCollection services)
//     {
//         services.AddRateLimiter(options =>
//         {
//             options.RejectionStatusCode =
//                 StatusCodes.Status429TooManyRequests;

//             options.OnRejected = async (context, token) =>
//             {
//                 context.HttpContext.Response.ContentType =
//                     "application/json";

//                 await context.HttpContext.Response.WriteAsync(
//                     """
//                     {
//                         "message": "Too many requests",
//                         "type": "RateLimit"
//                     }
//                     """,
//                     token);
//             };

//             // 🔥 LIMITACIÓN POR IP
//             options.GlobalLimiter =
//                 PartitionedRateLimiter.Create<HttpContext, string>(
//                     httpContext =>
//                     {
//                         var ipAddress =
//                             httpContext.Connection.RemoteIpAddress?
//                                 .ToString() ?? "unknown";

//                         return RateLimitPartition
//                             .GetFixedWindowLimiter(
//                                 partitionKey: ipAddress,

//                                 factory: _ =>
//                                     new FixedWindowRateLimiterOptions
//                                     {
//                                         PermitLimit = 10,

//                                         Window =
//                                             TimeSpan.FromMinutes(1),

//                                         QueueProcessingOrder =
//                                             QueueProcessingOrder
//                                                 .OldestFirst,

//                                         QueueLimit = 0
//                                     });
//                     });
//         });

//         return services;
//     }
// }