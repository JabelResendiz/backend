

using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class VaccinatedSubjectSummaryDto
{
    public required string FullName { get; set; }
    public required int Age { get; set; }
    public required Gender Gender { get; set; }
    public bool? IsPregnant { get; set; }
}