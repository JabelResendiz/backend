using System.ComponentModel.DataAnnotations;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class VaccinationDto
{
    [Required(ErrorMessage = "Vaccine information is required.")]
    public Guid VaccineId { get; set; }

    [Required(ErrorMessage = "Batch number is required.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Batch number must be between 1 and 50 characters.")]
    public string BatchNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Administration site is required.")]
    public AdministrationSite? Site { get; set; }

    [Range(1, 50, ErrorMessage = "Dose number must be greater than 0.")]
    public int DoseNumber { get; set; }

    [Required(ErrorMessage = "Administration date is required.")]
    public DateTime? AdministrationDate { get; set; }

    public string? VaccinationCenter { get; set; }
}