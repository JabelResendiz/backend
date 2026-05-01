using System.ComponentModel.DataAnnotations;
using Finlay.PharmaVigilance.Application.Validators.Attribute;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class ReporterDto
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(150, MinimumLength = 1, ErrorMessage = "Full name must be between 1 and 150 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Reporter Relationship is required.")]
    public ReporterRelationship? ReporterRelationship { get; set; }

    [Required(ErrorMessage = "Identity number is required.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "Identity number must be exactly 11 digits.")]
    public string IdentityNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date Of Birth is required.")]
    public DateTime? DateOfBirth { get; set; }

    [Required(ErrorMessage = "Province Id is required.")]
    public int ProvinceId { get; set; }

    [Required(ErrorMessage = "Municipality Id is required.")]
    public int MunicipalityId { get; set; }

    [Required(ErrorMessage = "Phone Number is required.")]
    [StringLength(20, MinimumLength = 1, ErrorMessage = "Phone Number must be between 1 and 20 characters.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Email must be between 1 and 100 characters.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [EmailValidation]
    public string Email { get; set; } = string.Empty;

    [StringLength(30, MinimumLength = 1, ErrorMessage = "Professional License must be between 1 and 30 characters.")]
    public string? ProfessionalLicense { get; set; }

    [StringLength(100, MinimumLength = 1, ErrorMessage = "Institution must be between 1 and 100 characters.")]
    public string? Institution { get; set; }

}