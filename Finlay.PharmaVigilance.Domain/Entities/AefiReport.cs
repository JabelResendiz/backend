using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;


public class AefiReport : GuidEntity
{
    public DateTime ReportDate { get; set; }
    public ReportStatus Status { get; set; }
    public string NotificationNumber { get; set; } = null!;
    public bool isMedicalReport { get; set; }


    public Guid ReporterId { get; set; }
    public Guid VaccinatedSubjectId { get; set; }

    public Reporter Reporter { get; set; } = null!;

    public VaccinatedSubject VaccinatedSubject { get; set; } = null!;


    public ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();
    public ICollection<AdverseEvent> AdverseEvents { get; set; } = new List<AdverseEvent>();
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
    public ICollection<MedicalReviewAssignment> MedicalReviewAssignments { get; set; } = new List<MedicalReviewAssignment>();


    public ReportPriority Priority =>
        AdverseEvents.Any()
            ? AdverseEvents.Max(ae => ae.GetPriority())
            : ReportPriority.Low;
}