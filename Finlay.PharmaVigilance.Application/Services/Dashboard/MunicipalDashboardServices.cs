using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Common;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finlay.PharmaVigilance.Application.Services;


public class MunicipalDashboardService : IMunicipalDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MunicipalDashboardService> _logger;
    private readonly IUserContextService _userContextService;
    private readonly IMedicalAssignmentRepository _assignmentRepository;
    private readonly IVaccinationRepository _vaccinationRepository;
    private readonly IAdverseEventRepository _adverseEventRepository;

    public MunicipalDashboardService(
        IUnitOfWork unitOfWork,
        ILogger<MunicipalDashboardService> logger,
        IUserContextService userContextService,
        IMedicalAssignmentRepository assignmentRepository,
        IVaccinationRepository vaccinationRepository,
        IAdverseEventRepository adverseEventRepository
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _userContextService = userContextService;
        _assignmentRepository = assignmentRepository;
        _vaccinationRepository = vaccinationRepository;
        _adverseEventRepository = adverseEventRepository;
    }
    public async Task<MunicipalDashboardOverviewDto> GetOverviewAsync()
    {

        var user = _userContextService.GetUserId();

        var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
                                .FirstOrDefaultAsync(sr => sr.UserId == user)
                                ?? throw new Exception("Section Responsible not found for the current user.");

        var data = await _unitOfWork.GetRepository<AefiReport>()
                .GetAllByItems(r =>
                    r.VaccinatedSubject.MunicipalityId == sectionResponsible.MunicipalityId)
                .GroupBy(r => 1)
                .Select(g => new
                {
                    Total = g.Count(),
                    Submitted = g.Count(x => x.Status == ReportStatus.Submitted),
                    UnderReview = g.Count(x => x.Status == ReportStatus.UnderReview),
                    Completed = g.Count(x => x.Status == ReportStatus.Approved),
                    Rejected = g.Count(x => x.Status == ReportStatus.Rejected),
                    Reopened = g.Count(x => x.Status == ReportStatus.Reopened)
                })
                .FirstOrDefaultAsync();

        var query = _unitOfWork.GetRepository<MedicalReviewAssignment>()
    .GetAllByItems(a => a.MedicalReview != null);

        var queryData = await query
        .Select(a => new
        {
            a.AssignedAt,
            a.MedicalReview!.ReviewedAt,
            MunicipalityId = a.AefiReport.VaccinatedSubject.MunicipalityId
        })
        .Where(x => x.MunicipalityId == sectionResponsible.MunicipalityId)
        .ToListAsync();


        var avg = queryData.Count() > 0 ? queryData.Average(x => (x.ReviewedAt - x.AssignedAt).TotalHours) : 0;

        return new MunicipalDashboardOverviewDto
        {
            TotalReports = data?.Total ?? 0,
            PendingReports = data?.Submitted ?? 0,
            UnderReviewReports = data?.UnderReview ?? 0,
            CompletedReports = data?.Completed ?? 0,
            RejectedReports = data?.Rejected ?? 0,
            ReopenedReports = data?.Reopened ?? 0,
            AverageReviewTimeHours = avg,

            CompletionRate = (data == null || data.Total == 0)
                ? 0
                : (double)data.Completed * 100 / data.Total
        };
    }






    public async Task<MunicipalDashboardPerformanceDto> GetPerformanceAsync()
    {
        var user = _userContextService.GetUserId();

        var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
            .FirstOrDefaultAsync(sr => sr.UserId == user)
            ?? throw new Exception("Section Responsible not found for the current user.");

        var doctorPerformance = await _assignmentRepository.GetDoctorPerformanceAsync(sectionResponsible.MunicipalityId);

        var totalCompletedReportsByHours = await _assignmentRepository.GetTimeHoursAsync(sectionResponsible.MunicipalityId);

        var municipalMetrics = await _assignmentRepository.GetMetrics(sectionResponsible.Id);

        return new MunicipalDashboardPerformanceDto
        {
            DoctorPerformances = doctorPerformance,
            TimeHours = totalCompletedReportsByHours,
            AverageAssignmentByReport = municipalMetrics.AverageAssignmentByReport,
            AverageReviewTimeHours = municipalMetrics.AverageReviewTimeHours,
            AverageAssignmentTimeHours = municipalMetrics.AverageAssignmentTimeHours

        };
    }

    public async Task<SectionResponsibleMunicipalDashboardDto> GetDashboardAsync(DashboardFilterDto filter)
    {
        var user = _userContextService.GetUserId();

        var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
                                .FirstOrDefaultAsync(sr => sr.UserId == user)
                                ?? throw new Exception("Section Responsible not found for the current user.");

        var municipalityId = sectionResponsible.MunicipalityId;

        var vaccineData = await _vaccinationRepository.GetVaccineByFilter(municipalityId);

        var symptomData = await _adverseEventRepository.GetSymptomFilter(municipalityId);

        var distributionData = await _adverseEventRepository.GetSeverityDistribution(municipalityId);

        var seriousData = await _adverseEventRepository.GetSeriousDataAsync(municipalityId);



        var timeline = filter.Period switch
        {
            "7d" => new List<ReportsTimelineDto>
        {
            new() { Label = "Lun", TotalReports = 4 },
            new() { Label = "Mar", TotalReports = 6 },
            new() { Label = "Mié", TotalReports = 5 },
            new() { Label = "Jue", TotalReports = 8 },
            new() { Label = "Vie", TotalReports = 3 },
            new() { Label = "Sáb", TotalReports = 2 },
            new() { Label = "Dom", TotalReports = 1 },
        },

            "1m" => new List<ReportsTimelineDto>
            {
                new() { Label = "Semana 1", TotalReports = 15 },
                new() { Label = "Semana 2", TotalReports = 21 },
                new() { Label = "Semana 3", TotalReports = 18 },
                new() { Label = "Semana 4", TotalReports = 25 },
            },

            _ => new List<ReportsTimelineDto>
            {
                new() { Label = "Ene", TotalReports = 40 },
                new() { Label = "Feb", TotalReports = 55 },
                new() { Label = "Mar", TotalReports = 61 },
                new() { Label = "Abr", TotalReports = 47 },
                new() { Label = "May", TotalReports = 70 },
            }
        };


        return new SectionResponsibleMunicipalDashboardDto
        {
            TopVaccines = vaccineData,
            TopSymptoms = symptomData,
            SeverityDistribution = distributionData,
            ReportsTimeline = timeline,
            TotalDeaths = seriousData.ResultedInDeath,
            TotalEmergencyRoom = seriousData.WentToEmergencyRoom,
            TotalPermanentDisability = seriousData.PermanentDisability,
            TotalWasHospitalized = seriousData.WasHospitalized,
            TotalVisitedDoctor = seriousData.VisitedDoctor,
            TotalAnomaly = seriousData.Anomaly,
            TotalNoComplications = seriousData.NoComplications
        };

    }

}