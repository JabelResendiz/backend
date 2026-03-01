

using Finlay.PharmaVigilance.Infrastructure.Initializer;
using Finlay.PharmaVigilance.Api;
using Finlay.PharmaVigilance.Application;
using Finlay.PharmaVigilance.Infrastructure;
using Finlay.PharmaVigilance.Api.Middleware;


var builder = WebApplication.CreateBuilder(args);

//configuration so that API listens on all network interfaces
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Any, 5137);  // Escucha en todas las interfaces en el puerto 5217
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

app.UseCors();
//app.UseCors("LocalhostPolicy");
        
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();