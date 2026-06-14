namespace Finlay.PharmaVigilance.Domain.Events;


public class MedicalReviewerAssignmentEvent : BasicEvent
{
    public string MedicalReviewerName { get; set; } = null!;
    public string MedicalReviewerEmail { get; set; } = null!;
    public string ReportNumber { get; set; } = null!;
}


