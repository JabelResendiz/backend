
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.DTO.Authentication;

namespace Finlay.PharmaVigilance.Application.IServices.Authentication;

/// <summary>
/// Defines operations related to user identity management, such as registration and login.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="userDto">The DTO containing the user's registration details.</param>
    /// <returns>A Task representing the asynchronous operation, returning the generated token as a string.</returns>
    //Task<string> RegisterUserAsync(RegisterUserDto userDto);

    /// <summary>
    /// Authenticates a user with the provided login credentials.
    /// </summary>
    /// <param name="userDto">The DTO containing the user's login details.</param>
    /// <returns>A Task representing the asynchronous operation, returning the generated token as a string if login is successful, otherwise null.</returns>


    //Task<string> LoginUserAsync(LoginUserDto userDto);
    Task<UserResponseDTO> LoginUserAsync(LoginUserDto loginDto);

    Task<string> RegisterAdminAsync(RegisterUserDto registerAdminDto);

    Task<RefreshTokenResponseDto> RefreshTokenAsync(string refreshToken);

    Task LogoutAsync(string refreshToken);

    Task CompleteRegistrationAsync(
        CompleteRegistrationDto dto);

}
