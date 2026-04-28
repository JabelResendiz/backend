namespace Finlay.PharmaVigilance.Domain.Events;


public class EmailToReporterEvent
{
    public string ReportNumber { get; set; } = null!;
    public string ReporterEmail { get; set; } = null!;
}


public class EmailToSectionResponsibleEvent
{
    public string ReportNumber { get; set; } = null!;
    public string SectionResponsibleEmail { get; set; } = null!;
}