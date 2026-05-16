using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;


public class ReportDetailAdminDto
{
    public required Guid Id { get; set; }
    public required string NotificationNumber { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime ReportDate { get; set; }
    public required ReportStatus Status { get; set; }
    public required SeverityLevel GlobalSeverityLevel { get; set; }
    public required ReporterAdminDto Reporter { get; set; }
    public required VaccinatedSubjectAdminDto VaccinatedSubject { get; set; }
    public required IEnumerable<VaccinationDetailsDto> Vaccinations { get; set; }
    public required IEnumerable<AdverseEventAdminDto> AdverseEvents { get; set; }
    public required IEnumerable<AssignmentResponse> MedicalReviewAssignments { get; set; }
    public MedicalReviewResponseDto? MedicalReview { get; set; }

}

public class ReportSummaryAdminDto
{
    public required Guid Id { get; set; }
    public required string NotificationNumber { get; set; }
    public required DateTime ReportDate { get; set; }
    public required ReportStatus Status { get; set; }
    public required SeverityLevel GlobalSeverityLevel { get; set; }
    public required VaccinatedSubjectAdminDto VaccinatedSubject { get; set; }
    public required IEnumerable<string> VaccinesName { get; set; }
    public required IEnumerable<string> AdverseEventsName { get; set; }
    public string? MedicalReviewerName { get; set; }
}
