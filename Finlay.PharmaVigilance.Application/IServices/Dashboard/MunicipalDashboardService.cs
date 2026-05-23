using Finlay.PharmaVigilance.Application.DTO;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IMunicipalDashboardService
{
    Task<MunicipalDashboardOverviewDto> GetOverviewAsync();

    Task<MunicipalDashboardPerformanceDto> GetPerformanceAsync();

    Task<SectionResponsibleMunicipalDashboardDto> GetDashboardAsync(DashboardFilterDto filter);
}