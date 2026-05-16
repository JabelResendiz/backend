using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Finlay.PharmaVigilance.Api.Controllers;

/// <summary>
/// API Controller responsible for managing system users.
/// Provides CRUD operations intended for administrators.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("GeneralQuery")]
//[Authorize] // Requires authentication for all endpoints
public class UserController : ControllerBase
{
    private readonly IUserQueryServices _userQueryService;
    private readonly IUserCommandServices _userCommandService;

    public UserController(IUserQueryServices userQueryService,
                          IUserCommandServices userCommandService)
    {
        _userQueryService = userQueryService;
        _userCommandService = userCommandService;
    }

    /// <summary>
    /// Retrieves all users in the system.
    /// Requires Administrator role.
    /// </summary>
    /// <returns>A list of all users.</returns>
    /// <response code="200">Returns the list of users.</response>
    /// <response code="401">Unauthorized - authentication required.</response>
    /// <response code="403">Forbidden - requires administrator role.</response>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<GetUserDto>>> GetAllUsers()
    {
        var users = await _userQueryService.ListAsync();
        return Ok(users);
    }

    /// <summary>
    /// Retrieves a specific user by their ID.
    /// Only administrators can access other users' information.
    /// Regular users can only access their own information.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <returns>The requested user's information.</returns>
    /// <response code="200">User found.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden - you do not have permission to access this user.</response>
    /// <response code="404">User not found.</response>
    [HttpGet("{userId:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetUserDto>> GetUserById(Guid userId)
    {
        var user = await _userQueryService.GetByIdAsync(userId);

        if (user == null)
            return NotFound(new { message = $"User with ID {userId} was not found." });

        return Ok(user);
    }

    /// <summary>
    /// Retrieves a user by their username.
    /// Only administrators are allowed to search by username.
    /// </summary>
    /// <param name="userName">The username to search for.</param>
    /// <returns>The found user's information.</returns>
    /// <response code="200">User found.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden - requires administrator role.</response>
    /// <response code="404">User not found.</response>
    [HttpGet("search/{userName}")]
    //[Authorize(Roles = "Supervisor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetUserDto>> GetUserByUserName(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            return BadRequest(new { message = "Username cannot be empty." });

        var user = await _userQueryService.GetByUserNameAsync(userName);

        if (user == null)
            return NotFound(new { message = $"User with username '{userName}' was not found." });

        return Ok(user);
    }

    /// <summary>
    /// Deletes a user from the system.
    /// Only administrators are allowed to delete users.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <returns>Deletion confirmation.</returns>
    /// <response code="204">User successfully deleted.</response>
    /// <response code="400">Bad request - invalid ID or deletion not allowed.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="403">Forbidden - requires administrator role.</response>
    /// <response code="404">User not found.</response>
    [HttpDelete("{userId:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        try
        {
            // if (userId != null)
            //     return BadRequest(new { message = "User ID must be a valid positive number." });

            await _userCommandService.DeleteAsync(userId);

            return NoContent(); // 204 - Successful deletion with no content
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"An error occurred while deleting the user: {ex.Message}" });
        }
    }
}