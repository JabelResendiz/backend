using Finlay.PharmaVigilance.Application.DTO;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IUserQueryServices
{
    /// <summary>
    /// Retrieves all users in the system.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A collection of <see cref="GetUserDto"/> representing all users.
    /// </returns>
    Task<IEnumerable<GetUserDto>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="cancellationToken">
    /// Token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A <see cref="GetUserDto"/> if the user exists; otherwise, null.
    /// </returns>
    Task<GetUserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="cancellationToken">
    /// Token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// A <see cref="GetUserDto"/> if the user exists; otherwise, null.
    /// </returns>
    Task<GetUserDto?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
}