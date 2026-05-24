
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;


public class AdverseEventDetailDto
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
    public DateTime? DeathDate { get; set; }
    public required PatientStatus CurrentStatus { get; set; }
    public required Intensity Intensity { get; set; }
    public required GetSymptomDto Symptom { get; set; }
}

public class AdverseEventDetailMedicalReviewerDto : AdverseEventDetailDto
{
    public required Guid Id { get; set; }
}

public class AdverseEventAdminDto
{
    public required DateTime StartDate { get; set; }
    public required bool VisitedDoctor { get; set; }
    public required bool WentToEmergencyRoom { get; set; }
    public required bool PermanentDisability { get; set; }
    public required bool IsLifeThreatening { get; set; }
    public required bool ResultedInDeath { get; set; }
    public DateTime? DeathDate { get; set; }
    public required PatientStatus CurrentStatus { get; set; }
    public required Intensity Intensity { get; set; }
    public required SeverityLevel SeverityLevel { get; set; }
    public required string Symptom { get; set; }
    public string? Description { get; set; }

    public string? MedDRACode { get; set; }
    public string? RetClassification { get; set; }

}
