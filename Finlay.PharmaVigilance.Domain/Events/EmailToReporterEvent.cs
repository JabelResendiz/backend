namespace Finlay.PharmaVigilance.Domain.Events;


public class EmailToReporterEvent
{
    public string ReportNumber { get; set; } = null!;
    public string ReporterEmail { get; set; } = null!;
}


