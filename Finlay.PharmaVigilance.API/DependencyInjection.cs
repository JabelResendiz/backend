
using Finlay.PharmaVigilance.Api.Common;
using Finlay.PharmaVigilance.Application.IServices.Common;
using System.Text.Json.Serialization;

namespace Finlay.PharmaVigilance.Api;


public static class DependencyInjection
{
    /// <summary>
    /// Configures services for the presentation layer, which includes setting up API controllers, 
    /// Swagger documentation, and Cross-Origin Resource Sharing (CORS) policies for the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which services are added.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> with the presentation layer services registered.</returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        // Add controllers to handle API requests
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter()
                );
            });

        // Add Swagger for API documentation
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Configure CORS to allow any origin, method, and headers
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin() // allows requests from any origin
                       .AllowAnyHeader() // allows HTTP headers
                       .AllowAnyMethod(); // allows any HTTP method (GET,POST,PUT,DELETE)
            });
        });

        services.AddScoped<IUserContextService, UserContextService>();

        return services;
    }
}