

using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;


public class AdverseEventSummaryDto
{
    public required DateTime StartDate { get; set; }
    public DateTime? FinishDate { get; set; }
    public required bool VisitedDoctor { get; set; }
    public required bool WentToEmergencyRoom { get; set; }
    public required bool PermanentDisability { get; set; }
    public required bool WasHospitalized { get; set; }
    public required bool NoComplications { get; set; }
    public required bool Anomaly { get; set; }
    public required bool ResultedInDeath { get; set; }
    public required PatientStatus CurrentStatus { get; set; }
    public required SeverityLevel SeverityLevel { get; set; }
    public required Intensity Intensity { get; set; }

}