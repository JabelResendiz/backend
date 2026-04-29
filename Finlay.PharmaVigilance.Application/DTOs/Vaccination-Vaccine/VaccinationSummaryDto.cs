

namespace Finlay.PharmaVigilance.Application.DTO;


public class VaccinationSummaryDto
{
    public required string VaccineName { get; set; }
    public string? VaccinationCenter { get; set; }
}