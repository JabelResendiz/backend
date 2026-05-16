using System.ComponentModel.DataAnnotations;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class VaccinationDto
{
    [Required(ErrorMessage = "Vaccine information is required.")]
    public Guid VaccineId { get; set; }

    [Required(ErrorMessage = "Lot number is required.")]
    public Guid LotId { get; set; }

    [Required(ErrorMessage = "Administration site is required.")]
    public AdministrationSite? Site { get; set; }

    [Range(1, 50, ErrorMessage = "Dose number must be greater than 0.")]
    public int DoseNumber { get; set; }

    [Required(ErrorMessage = "Administration date is required.")]
    public DateTime? AdministrationDate { get; set; }

    [Required(ErrorMessage = "Vaccination Center is required")]
    public Guid VaccinationCenterId { get; set; }
}