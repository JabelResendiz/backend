using System.ComponentModel.DataAnnotations;

namespace Finlay.PharmaVigilance.Application.DTO.Authentication;

public class RegisterUserDto
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "The email is not in a valid format.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "User role is required")]
    public string UserRole { get; set; } = string.Empty;

}
