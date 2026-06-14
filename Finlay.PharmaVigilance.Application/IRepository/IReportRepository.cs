using System.Linq.Expressions;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IRepository;

public interface IReportRepository : IGenericRepository<AefiReport>
{
    IQueryable<AefiReport> GetByFilter(
        string? vaccineName,
        string? provinceName,
        string? severity,
        string? reportStatus);


    IQueryable<AefiReport> GetSectionResponsibleByFilter(
        IQueryable<AefiReport> query,
        ReportSectionResponsibleFilter filter);


    IQueryable<AefiReport> GetMedicalReviewerByFilter(
        IQueryable<AefiReport> query,
        ReportMedicalReviewerFilter filter);

    Task<ReportStatusDto?> GetReportStatus(params Expression<Func<AefiReport, bool>>[] expressions);

    Task<IEnumerable<ProvinceReportStatusDto>> GetReportStatusByProvinces();

    Task<IEnumerable<CausalityDistributionDto>> GetCausalityDistributionAsync();

    Task<IEnumerable<SignificanceDistributionDto>> GetSignificanceDistributionAsync();

    Task<IEnumerable<SeverityLevelDistributionDto>> GetSeverityLevelDistributionAsync();

    Task<IEnumerable<MonthlyReportTrendDto>> GetMonthlyReportTrendAsync();

    Task<PerformanceDto> GetPerformanceMetrics();

    Task<IEnumerable<ProvinceMedicalActivityDto>> GetProvinceMedicalActivityAsync();
}