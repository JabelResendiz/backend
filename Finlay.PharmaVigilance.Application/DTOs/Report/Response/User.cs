using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class ReportUserDto
{
    public required DateTime ReportDate { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? AssignedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public required ReportStatus Status { get; set; }
    public required VaccinatedSubjectSummaryDto VaccinatedSubject { get; set; }
    public required ReporterDetailsDto Reporter { get; set; }
    public required IEnumerable<VaccinationDetailsDto> Vaccinations { get; set; }
    public required IEnumerable<AdverseEventDetailDto> AdverseEvents { get; set; }
}

