using System.ComponentModel.DataAnnotations;

namespace Finlay.PharmaVigilance.Application.DTO;

public class SymptomDto
{
    [Required(ErrorMessage = "Symptom name is required.")]
    [StringLength(150, MinimumLength = 1, ErrorMessage = "Vaccine name must be between 1 and 150 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Category must be between 1 and 100 characters.")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "IsActive status is required.")]
    public bool IsActive { get; set; }
    public string? Description { get; set; }
}