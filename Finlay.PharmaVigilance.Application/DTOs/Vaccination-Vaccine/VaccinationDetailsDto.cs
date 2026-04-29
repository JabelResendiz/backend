

using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class VaccinationDetailsDto
{
    public required string VaccineName { get; set; }
    public required string BatchNumber { get; set; }
    public required AdministrationSite AdministrationSite { get; set; }
    public required int DoseNumber { get; set; }
    public required DateTime AdministrationDate { get; set; }
    public string? VaccinationCenter { get; set; }
}