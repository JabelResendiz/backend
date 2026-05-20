using Finlay.PharmaVigilance.Application.DTO;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IAdminDashboardService
{
    Task<AdminReportDashboardDto> GetReportAsync();

    Task<AdminPerformanceDashboardDto> GetPerformanceAsync();

    Task<AdminVaccineDashboardDto> GetVaccinesAsync();
}