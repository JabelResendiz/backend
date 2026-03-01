using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Application.Common.Authentication;
using Finlay.PharmaVigilance.Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Finlay.PharmaVigilance.Application.Authentication;
using Finlay.PharmaVigilance.Infrastructure.Initializer;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Infrastructure.UnitOfWorkPattern;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


namespace Finlay.PharmaVigilance.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Adds application-specific services to the dependency injection container 
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)

    {

        // Database Configuration
        var connectionString = configuration.GetConnectionString("AppDbConnectionString");
        var db = services.AddDbContext<FinlayDbContext>(options => options.UseMySql(
                                                        connectionString, ServerVersion.AutoDetect(connectionString)));

        // Add HttpContextAccessor for accessing the current HTTP context
        services.AddHttpContextAccessor();


        // Authentication and Authorization
        services.AddAuth(configuration);

        //Identity configuration
        services.AddIdentity<User,Role>(options=>
                {
                    options.User.RequireUniqueEmail = true;

                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; 
                })
               .AddEntityFrameworkStores<FinlayDbContext>() // Configures EF for Identity
               .AddDefaultTokenProviders(); // Adds default token providers for things like password reset


        // Add custom repositories and services       
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        //services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IIdentityManager,IdentityManager>();
 
        //Register a service of type IHostedService in the dependency container
        services.AddHostedService<RoleInitializer>();



        return services;

    }


    /// <summary>
    /// Configures JWT authentication services for the application.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <param name="configuration">The configuration object for accessing application settings.</param>
    /// <returns>The updated IServiceCollection.</returns>
    public static IServiceCollection AddAuth(this IServiceCollection services,
                                              ConfigurationManager configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SECTION_NAME, jwtSettings);

        services.AddSingleton(jwtSettings); // Registro directo para dependencias que lo necesiten como instancia

        services.AddSingleton(Options.Create(jwtSettings));

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        // Configuración de autenticación JWT
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(jwtSettings.Secret))
                    };
                });

        return services;
    }

}


//scoped : se crea una nueva instacnia por solicitud HTTP.Se
//utiliza para servicios que necesitan tener estados dentro de una
// solicitud, como el accede a la BD

//singleton: viven durante toda la vida de la app
// servicios que no tienen estado o que son costosos de crear, como configuracion o cache


