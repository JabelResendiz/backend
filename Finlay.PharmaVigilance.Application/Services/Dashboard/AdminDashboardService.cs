using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Microsoft.Extensions.Logging;

namespace Finlay.PharmaVigilance.Application.Services;


public class AdminDashboardService : IAdminDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AdminDashboardService> _logger;
    private readonly IReportRepository _report;

    public AdminDashboardService(
        IUnitOfWork unitOfWork,
        ILogger<AdminDashboardService> logger,
        IReportRepository report
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _report = report;
    }


    public async Task<AdminReportDashboardDto> GetReportAsync()
    {

        var reportStatusDto = await _report.GetReportStatus();

        return new AdminReportDashboardDto
        {
            TotalReports = reportStatusDto?.TotalReports ?? 0,
            Submitted = reportStatusDto?.SubmittedReports ?? 0,
            UnderReview = reportStatusDto?.UnderReviewReports ?? 0,
            Approved = reportStatusDto?.ApprovedReports ?? 0,
            Rejected = reportStatusDto?.RejectedReports ?? 0,
            Reopened = reportStatusDto?.ReopenedReports ?? 0,
            Closed = reportStatusDto?.ClosedReports ?? 0,
            Provinces = { },
            SeverityDistribution = { },
            CausalityDistribution = { },
            SignificanceDistribution = { },
            MonthlyTrends = { }
        };

    }


    public async Task<AdminPerformanceDashboardDto> GetPerformanceAsync()
    {
        return new AdminPerformanceDashboardDto
        {

        };
    }


    public async Task<AdminVaccineDashboardDto> GetVaccinesAsync()
    {
        return new AdminVaccineDashboardDto
        {

        };
    }

}