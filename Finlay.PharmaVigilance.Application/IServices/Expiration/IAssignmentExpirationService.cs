namespace Finlay.PharmaVigilance.Application.IServices;

public interface IAssignmentExpirationService
{
    Task ProcessExpiredAssignmentsAsync(
        CancellationToken cancellationToken = default);
}