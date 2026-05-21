

using System.ComponentModel.DataAnnotations;
using Finlay.PharmaVigilance.Application.Validators.Attribute;

namespace Finlay.PharmaVigilance.Application.DTO.Authentication;

public class LoginUserDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "The email is not in a valid format.")]
    [EmailValidation]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Captcha Token information is required.")]
    public string Token { get; set; } = string.Empty;

}