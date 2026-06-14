namespace Finlay.PharmaVigilance.Domain.Events;


public class SectionReportAlertEvent : BasicEvent
{
    public string ReportNumber { get; set; } = null!;
    public string EmailSectionResponsible { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}