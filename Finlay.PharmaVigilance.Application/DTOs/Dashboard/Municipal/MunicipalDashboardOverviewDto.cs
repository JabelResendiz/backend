namespace Finlay.PharmaVigilance.Application.DTO;


public class MunicipalDashboardOverviewDto
{
    public int TotalReports { get; set; }

    public int PendingReports { get; set; }
    public int UnderReviewReports { get; set; }

    public int CompletedReports { get; set; }
    public int RejectedReports { get; set; }
    public int ReopenedReports { get; set; }


    public double AverageReviewTimeHours { get; set; }

    public double CompletionRate { get; set; }
}