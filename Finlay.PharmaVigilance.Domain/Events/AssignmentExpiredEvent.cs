namespace Finlay.PharmaVigilance.Domain.Events;

public class AssignmentExpiredEvent : BasicEvent
{
    public Guid AssignmentId { get; set; }
    public Guid ReportId { get; set; }
    public string SectionResponsibleEmail { get; set; } = null!;
}