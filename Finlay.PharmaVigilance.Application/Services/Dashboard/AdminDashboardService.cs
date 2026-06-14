using System.Transactions;
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
    private readonly IVaccinationRepository _vaccination;
    private readonly IAdverseEventRepository _adverseEvent;

    public AdminDashboardService(
        IUnitOfWork unitOfWork,
        ILogger<AdminDashboardService> logger,
        IReportRepository report,
        IVaccinationRepository vaccination,
        IAdverseEventRepository adverseEvent
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _report = report;
        _vaccination = vaccination;
        _adverseEvent = adverseEvent;
    }


    public async Task<AdminReportDashboardDto> GetReportAsync()
    {
        // var transactionOptions = new TransactionOptions
        // {
        //     IsolationLevel = IsolationLevel.RepeatableRead // Nivel por defecto de MySQL
        // };

        // using (var scope = new TransactionScope(
        //         TransactionScopeOption.Required,
        //         transactionOptions,
        //         TransactionScopeAsyncFlowOption.Enabled))
        // {
        //     // Tu código original exactamente igual:
        //     var reportStatusDto = await _report.GetReportStatus();
        //     var provinceStatusDto = await _report.GetReportStatusByProvinces();
        //     var causality = await _report.GetCausalityDistributionAsync();
        //     var significance = await _report.GetSignificanceDistributionAsync();
        //     var severity = await _report.GetSeverityLevelDistributionAsync();
        //     var monthly = await _report.GetMonthlyReportTrendAsync();

        //     scope.Complete(); // Avisas que terminaste de leer de forma segura

        //     return new AdminReportDashboardDto
        //     {
        //         TotalReports = reportStatusDto?.TotalReports ?? 0,
        //         Submitted = reportStatusDto?.SubmittedReports ?? 0,
        //         UnderReview = reportStatusDto?.UnderReviewReports ?? 0,
        //         Approved = reportStatusDto?.ApprovedReports ?? 0,
        //         Rejected = reportStatusDto?.RejectedReports ?? 0,
        //         Reopened = reportStatusDto?.ReopenedReports ?? 0,
        //         Closed = reportStatusDto?.ClosedReports ?? 0,
        //         Provinces = provinceStatusDto,
        //         SeverityDistribution = severity,
        //         CausalityDistribution = causality,
        //         SignificanceDistribution = significance,
        //         MonthlyTrends = monthly
        //     };
        // }


        var reportStatusDto = await _report.GetReportStatus();

        var provinceStatusDto = await _report.GetReportStatusByProvinces();

        var causality = await _report.GetCausalityDistributionAsync();

        var significance = await _report.GetSignificanceDistributionAsync();

        var severity = await _report.GetSeverityLevelDistributionAsync();

        var monthly = await _report.GetMonthlyReportTrendAsync();

        return new AdminReportDashboardDto
        {
            TotalReports = reportStatusDto?.TotalReports ?? 0,
            Submitted = reportStatusDto?.SubmittedReports ?? 0,
            UnderReview = reportStatusDto?.UnderReviewReports ?? 0,
            Approved = reportStatusDto?.ApprovedReports ?? 0,
            Rejected = reportStatusDto?.RejectedReports ?? 0,
            Reopened = reportStatusDto?.ReopenedReports ?? 0,
            Closed = reportStatusDto?.ClosedReports ?? 0,
            Provinces = provinceStatusDto,
            SeverityDistribution = severity,
            CausalityDistribution = causality,
            SignificanceDistribution = significance,
            MonthlyTrends = monthly
        };

    }


    public async Task<AdminPerformanceDashboardDto> GetPerformanceAsync()
    {

        var performance = await _report.GetPerformanceMetrics();
        var provincePerformance = await _report.GetProvinceMedicalActivityAsync();

        return new AdminPerformanceDashboardDto
        {
            ActiveDoctors = performance?.ActiveDoctors ?? 0,
            AvgReportsPerDoctor = performance?.AvgReportsPerDoctor ?? 0,
            AvgReviewTimeHours = performance?.AvgReviewTimeHours ?? 0,
            AvgAssignmentHours = performance?.AvgAssignmentHours ?? 0,
            ActiveMedicalReviewers = provincePerformance
        };
    }


    public async Task<AdminVaccineDashboardDto> GetVaccinesAsync()
    {

        var vaccineDistribution = await _vaccination.GetVaccineDistributionAsync();
        var symptomDistribution = await _adverseEvent.GetSymptomDistributionAsync();

        return new AdminVaccineDashboardDto
        {
            VaccinesDistribution = vaccineDistribution,
            SymptomDistribution = symptomDistribution
        };
    }





}