using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;

public class AdverseEvent : GuidEntity
{
    public DateTime StartDate { get; set; }
    public DateTime? FinishDate { get; set; }

    public string? Description { get; set; }

    public bool VisitedDoctor { get; set; } = false;
    public bool WentToEmergencyRoom { get; set; } = false;
    public bool PermanentDisability { get; set; } = false;
    public bool Anomaly { get; set; } = false;
    public bool WasHospitalized { get; set; } = false;
    public bool ResultedInDeath { get; set; } = false;
    public bool NoComplications { get; set; } = true;
    public DateTime? DeathDate { get; set; }
    public PatientStatus CurrentStatus { get; set; }
    public Intensity Intensity { get; set; }
    public SeverityLevel SeverityLevel { get; set; }


    public string? LaboratoryResults { get; set; }

    public string? MedDRACode { get; set; } = null!;
    public string? RetClassification { get; set; } = null!;



    public Guid AefiReportId { get; set; }
    public AefiReport AefiReport { get; set; } = null!;
    // public ICollection<AdverseEventSymptom> AdverseEventSymptoms { get; set; } = new List<AdverseEventSymptom>();

    public Guid SymptomId { get; set; }
    public Symptom Symptom { get; set; } = null!;
}