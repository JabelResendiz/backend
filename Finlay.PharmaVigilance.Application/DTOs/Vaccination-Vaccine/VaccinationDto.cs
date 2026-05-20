using System.ComponentModel.DataAnnotations;
using Finlay.PharmaVigilance.Domain.Enum;
using Finlay.PharmaVigilance.Application.Validators.Attribute;

namespace Finlay.PharmaVigilance.Application.DTO;

public class VaccinationDto
{
    [Required(ErrorMessage = "Vaccine information is required.")]
    public Guid VaccineId { get; set; }

    [Required(ErrorMessage = "Lot number is required.")]
    public Guid LotId { get; set; }

    [Required(ErrorMessage = "Administration site is required.")]
    public AdministrationSite? Site { get; set; }

    [Required(ErrorMessage = "Dose Number is required.")]
    [EnumValidation(typeof(DoseNumber), ErrorMessage = "Invalid Dose Number")]
    public string DoseNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Administration date is required.")]
    public DateTime? AdministrationDate { get; set; }

    [Required(ErrorMessage = "Vaccination Center is required")]
    public Guid VaccinationCenterId { get; set; }
}