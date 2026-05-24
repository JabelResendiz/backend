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

    [Required(ErrorMessage = "IsActive status is required.")]
    public bool IsActive { get; set; }

    [StringLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
    public string? Description { get; set; }
    public DateTime? ApprovalDate { get; set; }

    [Required(ErrorMessage = "Target Pathology is required")]
    public string TargetPathology { get; set; } = string.Empty;

    [Required(ErrorMessage = "Manufacturer is required.")]
    public ManufacturerDto ManufacturerDto { get; set; } = null!;
}

