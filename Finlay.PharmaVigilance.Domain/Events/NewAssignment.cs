namespace Finlay.PharmaVigilance.Domain.Events;


public class NewAssignmentEvent
{
    public string MedicalReviewerName { get; set; } = null!;
    public string MedicalReviewerEmail { get; set; } = null!;
    public string ReportNumber { get; set; } = null!;
}


