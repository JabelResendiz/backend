
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class ReportSectionResponsibleDto
{
    public required Guid Id { get; set; }
    public required DateTime ReportDate { get; set; }
    public required ReportStatus Status { get; set; }
    public required VaccinatedSubjectSummaryDto VaccinatedSubject { get; set; }
    public required IEnumerable<VaccinationSummaryDto> Vaccinations { get; set; }
    public required IEnumerable<AdverseEventSummaryDto> AdverseEvents { get; set; }
}
