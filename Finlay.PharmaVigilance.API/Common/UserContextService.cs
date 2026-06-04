using System.Security.Claims;
using Finlay.PharmaVigilance.Application.IServices.Common;

namespace Finlay.PharmaVigilance.Api.Common;

/// <summary>
/// Provides access to the current authenticated user's context information.
/// Extracts user claims from the HTTP context to retrieve the user ID from JWT tokens.
/// </summary>
public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the UserContextService.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor to retrieve the current user's claims.</param>
    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Retrieves the ID of the currently authenticated user from the HTTP context.
    /// </summary>
    /// <returns>The user ID as an integer.</returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown when no user is authenticated or the user ID claim is not found in the token.
    /// </exception>
    public Guid GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null)
            throw new UnauthorizedAccessException("No authenticated user.");

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            throw new UnauthorizedAccessException("User ID not found in token.");

        return Guid.Parse(userId);
    }

    public Guid? GetUserIdOrNull()
    {
        var userId = _httpContextAccessor.HttpContext?
            .User?
            .FindFirst(ClaimTypes.NameIdentifier)?
            .Value;

        if (Guid.TryParse(userId, out var id))
            return id;

        return null;
    }

    public string? IPAddress =>
        _httpContextAccessor
            .HttpContext?
            .Connection?
            .RemoteIpAddress?
            .ToString();
}
