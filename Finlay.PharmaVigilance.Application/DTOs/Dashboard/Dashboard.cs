namespace Finlay.PharmaVigilance.Application.DTO;


public class DashboardAdminDto
{
    public required ReportDashboardDto ReportDashboard { get; set; }
    public required ICollection<VaccineDashboardDto> VaccineDashboard { get; set; }

}

public class ReportDashboardDto
{
    public required int TotalReport { get; set; }
    public required int CompletedReport { get; set; }
    public required int InRevisionReport { get; set; }
    public required int TodayReport { get; set; }
}

public class VaccineDashboardDto
{
    public required string Name { get; set; }
    public required int TotalReport
    {
        get; set;
    }
}