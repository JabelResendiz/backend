namespace Finlay.PharmaVigilance.Application.IServices.Common;

/// <summary>
/// Defines operations to access the current authenticated user's context information.
/// This service extracts user data from the HTTP context and claims.
/// </summary>
public interface IUserContextService
{
    /// <summary>
    /// Retrieves the ID of the currently authenticated user from the HTTP context.
    /// </summary>
    /// <returns>The user ID as an guid.</returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown when no user is authenticated or the user ID claim is not found in the token.
    /// </exception>
    Guid GetUserId();
    Guid? GetUserIdOrNull();
    string? IPAddress { get; }
}
