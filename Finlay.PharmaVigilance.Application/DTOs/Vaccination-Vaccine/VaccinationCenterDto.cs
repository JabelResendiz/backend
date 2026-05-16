using System.ComponentModel.DataAnnotations;

namespace Finlay.PharmaVigilance.Application.DTO;

public class VaccinationCenterDto
{
    [Required(ErrorMessage = "Vaccination Center Name is required.")]
    [StringLength(150, MinimumLength = 1, ErrorMessage = "VaccinationCenter Name must be between 1 and 150 characters.")]

    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vaccination Center Address is required.")]
    [StringLength(30, MinimumLength = 1, ErrorMessage = "Vaccination Center Address  must be between 1 and 300 characters.")]

    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Municipality is required.")]
    public int MunicipalityId { get; set; }

    [Required(ErrorMessage = "Province is required.")]
    public int ProvinceId { get; set; }
}


public class VaccinationCenterResponseDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required string Address { get; set; } = string.Empty;

}