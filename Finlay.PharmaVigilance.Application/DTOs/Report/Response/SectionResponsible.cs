
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class ReportSectionResponsibleDto
{
    public required Guid Id { get; set; }
    public required string NotificationNumber { get; set; }
    public required DateTime ReportDate { get; set; }
    public required ReportStatus Status { get; set; }
    public required SeverityLevel GlobalSeverityLevel { get; set; }
    public required VaccinatedSubjectSummaryDto VaccinatedSubject { get; set; }
    public required IEnumerable<VaccinationSummaryDto> Vaccinations { get; set; }
    public required IEnumerable<AdverseEventSummaryDto> AdverseEvents { get; set; }
    public string? LastDoctorName { get; set; }
    public string? RejectionReason { get; set; }
}
