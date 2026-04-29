
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;


public class AdverseEventDetailDto
{
    public required DateTime StartDate { get; set; }
    public required bool VisitedDoctor { get; set; }
    public required bool WentToEmergencyRoom { get; set; }
    public required bool PermanentDisability { get; set; }
    public required bool IsLifeThreatening { get; set; }
    public required bool ResultedInDeath { get; set; }
    public DateTime? DeathDate { get; set; }
    public required PatientStatus CurrentStatus { get; set; }
    public required IEnumerable<GetSymptomDto> Symptoms { get; set; }
}