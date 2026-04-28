
namespace Finlay.PharmaVigilance.Application.IServices;

public interface IUserCommandServices 
{
    /// <summary>
    /// Deletes a user by its identifier.
    /// </summary>
    /// <param name="userId">The identifier of the user to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>A Task representing the asynchronous delete operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the user does not exist.</exception>
    Task DeleteAsync(int userId, CancellationToken cancellationToken = default);
}