namespace Finlay.PharmaVigilance.Application.DTO;

public class DoctorPerformanceDto
{
    public Guid DoctorId { get; set; }

    public string DoctorName { get; set; } = null!;

    public int AssignedReports { get; set; }

    public int CompletedReports { get; set; }

    public int PendingReports { get; set; }
    public int ExpiredReports { get; set; }
    public int CancelledReports { get; set; }
    public double AverageReviewTimeHours { get; set; }

    public double CompletionRate { get; set; }
}