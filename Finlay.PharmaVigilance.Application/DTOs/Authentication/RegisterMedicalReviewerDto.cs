using System.ComponentModel.DataAnnotations;
namespace Finlay.PharmaVigilance.Application.DTO.Authentication;

/// <summary>
/// DTO for registering a new Medical Reviewer user with their specific profile information.
/// </summary>
public class RegisterMedicalReviewerDto : RegisterUserDto
{
    /// <summary>
    /// Health area where the Medical Reviewer works.
    /// </summary>
    [Required(ErrorMessage = "Institution is required")]
    public string Institution { get; set; } = string.Empty;

    [Required(ErrorMessage = "Professional License is required")]
    public string ProfessionalLicense { get; set; } = string.Empty;

    [Required(ErrorMessage = "Identity number is required.")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "Identity number must be between 5 and 20 characters.")]
    public string IdentityNumber { get; set; } = null!;

    [Required(ErrorMessage = "Date Of Birth is required.")]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(100, MinimumLength = 1, ErrorMessage = "Specialty must be between 1 and 100 characters.")]
    public string? Specialty { get; set; }

}
