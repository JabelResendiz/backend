using Finlay.PharmaVigilance.Application.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Finlay.PharmaVigilance.Infrastructure.BackgroundServices;

public class AssignmentExpirationBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AssignmentExpirationBackgroundService> _logger;
    public AssignmentExpirationBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<AssignmentExpirationBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var service = scope.ServiceProvider
                    .GetRequiredService<IAssignmentExpirationService>();

                await service.ProcessExpiredAssignmentsAsync(stoppingToken);

            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing expired assignments");
            }

            await Task.Delay(TimeSpan.FromHours(3), stoppingToken);
        }
    }
}