namespace Finlay.PharmaVigilance.Application.DTO;

public class ReportStatusDto
{
    public int TotalReports { get; set; }
    public int SubmittedReports { get; set; }
    public int UnderReviewReports { get; set; }
    public int ApprovedReports { get; set; }
    public int RejectedReports { get; set; }
    public int ReopenedReports { get; set; }
    public int ClosedReports { get; set; }
}