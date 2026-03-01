

using Finlay.PharmaVigilance.Infrastructure.Initializer;
using Finlay.PharmaVigilance.Api;
using Finlay.PharmaVigilance.Application;
using Finlay.PharmaVigilance.Infrastructure;
using Finlay.PharmaVigilance.Api.Middleware;


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


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await DatabaseInitializer.InitializeAsync(app.Services);
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // In production, also apply migrations
    try
    {
        await DatabaseInitializer.InitializeAsync(app.Services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration error: {ex.Message}");
        // Continue even if migration fails
    }
}

app.UseCors();
//app.UseCors("LocalhostPolicy");
        
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();