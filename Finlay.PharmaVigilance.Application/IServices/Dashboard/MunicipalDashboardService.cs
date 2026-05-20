using Finlay.PharmaVigilance.Application.DTO;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IMunicipalDashboardService
{
    Task<MunicipalDashboardOverviewDto> GetOverviewAsync();

    Task<IEnumerable<DoctorPerformanceDto>> GetDoctorPerformanceAsync();

    Task<SectionResponsibleMunicipalDashboardDto> GetDashboardAsync(DashboardFilterDto filter);
}