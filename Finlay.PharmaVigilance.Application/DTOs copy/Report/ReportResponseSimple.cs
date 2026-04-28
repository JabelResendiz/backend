using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class ReportSummaryDto
{
    public required DateTime ReportDate { get; set; }
    public required VaccinatedSubjectResponseDto VaccinatedSubject { get; set; }
    public required IEnumerable<VaccinationSummaryDto> Vaccinations { get; set; }
    public required IEnumerable<AdverseEventSummaryDto> AdverseEvents { get; set; }
}

public class VaccinationSummaryDto
{
    public required string VaccineName { get; set; }
    public string? VaccinationCenter { get; set; }
}

public class AdverseEventSummaryDto
{
    public required DateTime StartDate { get; set; }
    public required bool VisitedDoctor { get; set; }
    public required bool WentToEmergencyRoom { get; set; }
    public required bool PermanentDisability { get; set; }
    public required bool IsLifeThreatening { get; set; }
    public required bool ResultedInDeath { get; set; }
    public required PatientStatus CurrentStatus { get; set; }
}
