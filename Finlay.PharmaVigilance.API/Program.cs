using Finlay.PharmaVigilance.Infrastructure.Initializer;
using Finlay.PharmaVigilance.Api;
using Finlay.PharmaVigilance.Application;
using Finlay.PharmaVigilance.Infrastructure;
using Finlay.PharmaVigilance.Api.Middleware;
using Microsoft.AspNetCore.Identity;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;


var builder = WebApplication.CreateBuilder(args);

// Add environment variables configuration
builder.Configuration.AddEnvironmentVariables();

// Determine port: use PORT env variable if available (for Railway), otherwise use 5137
var port = Environment.GetEnvironmentVariable("PORT") ?? "5137";

//configuration so that API listens on all network interfaces
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Any, int.Parse(port));
});

var services = builder.Services;


services.AddPresentation();
services.AddAplication(builder.Configuration);
services.AddInfrastructure(builder.Configuration);


var app = builder.Build();


// Initialize database and roles BEFORE starting the application
try
{
    Console.WriteLine("===== Starting database initialization =====");
    
    // Apply migrations
    await DatabaseInitializer.InitializeAsync(app.Services);
    Console.WriteLine("✓ Database migrations completed successfully.");
    
    // Initialize roles after migrations are done
    using (var scope = app.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        Console.WriteLine("Creating roles...");
        
        foreach (var role in UserRoleHelper.AllRoles())
        {
            try
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new Role { Name = role });
                    if (result.Succeeded)
                    {
                        Console.WriteLine($"✓ Role '{role}' created successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"✗ Error creating role '{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    Console.WriteLine($"✓ Role '{role}' already exists.");
                }
            }
            catch (Exception roleEx)
            {
                Console.WriteLine($"✗ Exception while processing role '{role}': {roleEx.Message}");
            }
        }
    }
    Console.WriteLine("===== Role initialization completed =====");
}
catch (Exception ex)
{
    Console.WriteLine($"✗✗✗ FATAL ERROR during initialization: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
    // Don't exit - allow the app to start anyway
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
//app.UseCors("LocalhostPolicy");
        
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();