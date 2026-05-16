using System.ComponentModel.DataAnnotations;

namespace Finlay.PharmaVigilance.Application.DTO;

public class ManufacturerDto
{
    [Required(ErrorMessage = "Manufacturer name is required.")]
    [StringLength(150, MinimumLength = 1, ErrorMessage = "Manufacturer name must be between 1 and 150 characters.")]
    public string Name { get; set; } = string.Empty;
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "IsNew is required.")]
    public bool IsNew { get; set; }

    [Required(ErrorMessage = "Country is required.")]
    [StringLength(150, MinimumLength = 1, ErrorMessage = "Country must be between 1 and 150 characters.")]
    public string Country { get; set; } = string.Empty;
}