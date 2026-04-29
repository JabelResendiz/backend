using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;

namespace Finlay.PharmaVigilance.Application.Services;

/// <summary>
/// Implementation of command services for User entity.
/// Provides write operations such as delete and update.
/// Follows the generic service pattern and uses Unit of Work.
/// </summary>
public class UserCommandService : IUserCommandServices
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserCommandService"/> class.
    /// </summary>
    /// <param name="unitOfWork">
    /// The Unit of Work instance used to manage repositories and transactions.
    /// </param>
    public UserCommandService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to delete.</param>
    /// <param name="cancellationToken">
    /// Token used to cancel the asynchronous operation.
    /// </param>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when the user does not exist.
    /// </exception>
    public async Task DeleteAsync(int userId, CancellationToken cancellationToken = default)
    {
        // Ensure the user exists before attempting deletion
        var userRepository = _unitOfWork.UserRepository;
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);

        if (user == null)
            throw new KeyNotFoundException($"User with ID {userId} was not found.");

        // Delete the user
        await userRepository.DeleteByIdAsync(userId);

        // Persist changes
        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}