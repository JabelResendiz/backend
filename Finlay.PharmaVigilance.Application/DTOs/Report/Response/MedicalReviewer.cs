
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class ReportMedicalReviewerDto
{

    public required Guid Id { get; set; }

    public required DateTime ReportDate { get; set; }
    public DateTime? AssignedDate { get; set; }
    public required ReportStatus Status { get; set; }
    public required VaccinatedSubjectSummaryDto VaccinatedSubject { get; set; }
    public required ReporterDetailsDto Reporter { get; set; }
    public required IEnumerable<VaccinationDetailsDto> Vaccinations { get; set; }
    public required IEnumerable<AdverseEventDetailMedicalReviewerDto> AdverseEvents { get; set; }
}
