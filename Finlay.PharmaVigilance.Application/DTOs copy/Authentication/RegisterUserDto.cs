using System.ComponentModel.DataAnnotations;

namespace Finlay.PharmaVigilance.Application.DTO.Authentication;

public class RegisterUserDto
{
    [Required(ErrorMessage = "Username is required")]
    [MinLength(1, ErrorMessage = "Username cannot be empty or whitespace.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "The email is not in a valid format.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Phone number of the Medical Reviewer.
    /// </summary>
    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Phone number is not in a valid format.")]
    public string PhoneNumber { get; set; } = string.Empty;

}
