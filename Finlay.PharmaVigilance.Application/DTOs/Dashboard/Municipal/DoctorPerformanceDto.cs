namespace Finlay.PharmaVigilance.Application.DTO;



public class MunicipalDashboardPerformanceDto
{
    public double AverageReviewTimeHours { get; set; }
    public double AverageAssignmentTimeHours { get; set; }
    public double AverageAssignmentByReport { get; set; }
    public ICollection<TimeHourDto> TimeHours { get; set; } = new List<TimeHourDto>();
    public ICollection<DoctorPerformanceDto> DoctorPerformances { get; set; } = new List<DoctorPerformanceDto>();

}

public class TimeHourDto
{
    public string Hour { get; set; } = string.Empty;
    public int TotalReport { get; set; }
}

public class MunicipalMetricsDto
{
    public double AverageAssignmentByReport { get; set; }

    public double AverageReviewTimeHours { get; set; }

    public double AverageAssignmentTimeHours { get; set; }
}





public class DoctorPerformanceDto
{
    public string DoctorName { get; set; } = null!;

    public int AssignedReports { get; set; }

    public int CompletedReports { get; set; }

    public int PendingReports { get; set; }
    public int ExpiredReports { get; set; }
    public int CancelledReports { get; set; }


}