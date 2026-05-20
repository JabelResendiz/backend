namespace Finlay.PharmaVigilance.Application.DTO;


public class DashboardFilterDto
{
    public string? Period { get; set; } // 7d, 1m, 3m, 6m, 1y, all

    public DateTime? From { get; set; }

    public DateTime? To { get; set; }
}