using System.ComponentModel.DataAnnotations;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class VaccineDto
{
    [Required(ErrorMessage = "Vaccine name is required.")]
    [StringLength(150, MinimumLength = 1, ErrorMessage = "Vaccine name must be between 1 and 150 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vaccine type is required.")]
    public VaccineType Type { get; set; }

    [StringLength(50, MinimumLength = 1, ErrorMessage = "Code must be between 1 and 50 characters.")]
    public string? Code { get; set; }

    [Required(ErrorMessage = "IsActive status is required.")]
    public bool IsActive { get; set; }

    [StringLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
    public string? Description { get; set; }
    public DateTime? ApprovalDate { get; set; }
}