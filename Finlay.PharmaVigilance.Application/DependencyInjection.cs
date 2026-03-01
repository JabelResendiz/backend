using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Finlay.PharmaVigilance.Application.Services;
using Finlay.PharmaVigilance.Application.Services.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finlay.PharmaVigilance.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Adds application-specific services to the dependency injection container 
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static IServiceCollection AddAplication(this IServiceCollection services, ConfigurationManager configurationManager)
    {

        // Registers AutoMapper to enable mapping between DTOs and domain models.
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


        // Registers services related to Entities

        services.AddScoped<IIdentityService,IdentityService>();
        services.AddScoped<IEmployeeQueryServices,EmployeeQueryServices>();
        services.AddScoped<IEmployeeCommandServices,EmployeeCommandServices>();
        
        return services;


    }
}