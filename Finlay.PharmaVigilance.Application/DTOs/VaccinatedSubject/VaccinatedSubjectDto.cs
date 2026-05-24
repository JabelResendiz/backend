using System.ComponentModel.DataAnnotations;
using Finlay.PharmaVigilance.Application.Validators.Attribute;
using Finlay.PharmaVigilance.Domain.Enum;

public class VaccinatedSubjectDto
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(150, MinimumLength = 1, ErrorMessage = "Full name must be between 1 and 150 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Identity number is required.")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "Identity number must be between 5 and 20 characters.")]
    public string IdentityNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Gender is required.")]
    public Gender? Gender { get; set; }

    public bool? IsPregnant { get; set; }

    [Range(1, 16, ErrorMessage = "A valid Province ID (1-16) is required.")]
    public int ProvinceId { get; set; }

    [Range(1, 200, ErrorMessage = "A valid Municipality ID is required.")]
    public int MunicipalityId { get; set; }

    [StringLength(100, ErrorMessage = "Health area cannot exceed 100 characters.")]
    public string? HealthArea { get; set; }

    [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
    public string? Address { get; set; }

    [Phone(ErrorMessage = "Phone number format is not valid.")]
    public string? PhoneNumber { get; set; }

    [EmailAddress(ErrorMessage = "Email format is not valid.")]
    [EmailValidation]
    public string? Email { get; set; }

    [StringLength(250, ErrorMessage = "Current Medications cannot exceed 250 characters")]
    public string? CurrentMedications { get; set; }


    [StringLength(250, ErrorMessage = "Allergies cannot exceed 250 characters")]
    public string? Allergies { get; set; }


    [StringLength(400, ErrorMessage = "Medical History cannot exceed 400 characters")]
    public string? MedicalHistory { get; set; }
}