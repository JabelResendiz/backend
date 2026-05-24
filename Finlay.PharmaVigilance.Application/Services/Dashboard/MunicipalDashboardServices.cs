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
                    Pending = g.Count(x => x.Status == ReportStatus.Submitted),
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
            PendingReports = data?.Pending ?? 0,
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






    // public async Task<MunicipalDashboardPerformanceDto> GetPerformanceAsync()
    // {
    //     var user = _userContextService.GetUserId();

    //     var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
    //         .FirstOrDefaultAsync(sr => sr.UserId == user)
    //         ?? throw new Exception("Section Responsible not found for the current user.");

    //     var doctorPerformance = await _assignmentRepository.GetDoctorPerformanceAsync(sectionResponsible.MunicipalityId);

    //     var totalCompletedReportsByHours = await _assignmentRepository.GetTimeHoursAsync(sectionResponsible.MunicipalityId);

    //     var municipalMetrics = await _assignmentRepository.GetMetrics(sectionResponsible.Id);

    //     return new MunicipalDashboardPerformanceDto
    //     {
    //         DoctorPerformances = doctorPerformance,
    //         TimeHours = totalCompletedReportsByHours,
    //         AverageAssignmentByReport = municipalMetrics.AverageAssignmentByReport,
    //         AverageReviewTimeHours = municipalMetrics.AverageReviewTimeHours,
    //         AverageAssignmentTimeHours = municipalMetrics.AverageAssignmentTimeHours

    //     };
    // }

    public async Task<MunicipalDashboardPerformanceDto> GetPerformanceAsync()
    {
        return new MunicipalDashboardPerformanceDto
        {
            AverageAssignmentByReport = 1,
            AverageReviewTimeHours = 10.7,
            AverageAssignmentTimeHours = 4.6,
            TimeHours = new List<TimeHourDto>
        {
            new TimeHourDto
            {
                Hour= "0-5",
                TotalReport = 20
            },
            new TimeHourDto
            {
                Hour = "5-10",
                TotalReport = 30
            },
            new TimeHourDto
            {
                Hour = "10-20",
                TotalReport = 15,
            },
             new TimeHourDto
             {
                 Hour = "20-24",
                 TotalReport = 20
             },
             new TimeHourDto
             {
                 Hour = "24+",
                 TotalReport = 4
             }
        }
        ,
            DoctorPerformances = new List<DoctorPerformanceDto>()
        {
            new DoctorPerformanceDto
            {
               // DoctorId = Guid.NewGuid(),
                DoctorName = "Dr. Martha Silva",
                AssignedReports = 10,
                CompletedReports = 8,
                PendingReports = 1,
                ExpiredReports = 0,
                CancelledReports = 1,
                // AverageReviewTimeHours = 24.5,
                // CompletionRate = 80
            },
            new DoctorPerformanceDto
            {
                // DoctorId = Guid.NewGuid(),
                DoctorName = "Dr. Carlos Mendes",
                AssignedReports = 15,
                CompletedReports = 12,
                PendingReports = 2,
                ExpiredReports = 1,
                CancelledReports = 0,
                // AverageReviewTimeHours = 30.2,
                // CompletionRate = 80,
                // NumeroDeCasosGravesCompletados = 3
            },
            new DoctorPerformanceDto
            {
                // DoctorId = Guid.NewGuid(),
                DoctorName = "Dr. Ana Pereira",
                AssignedReports = 20,
                CompletedReports = 18,
                PendingReports = 1,
                ExpiredReports = 0,
                CancelledReports = 1,
                // AverageReviewTimeHours = 22.8,
                // CompletionRate = 90,
                // NumeroDeCasosGravesCompletados = 5
            },
            new DoctorPerformanceDto
            {
                // DoctorId = Guid.NewGuid(),
                DoctorName = "Dr. João Costa",
                AssignedReports = 12,
                CompletedReports = 7,
                PendingReports = 4,
                ExpiredReports = 0,
                CancelledReports = 1,
                // AverageReviewTimeHours = 28.4,
                // CompletionRate = 83.3,
                // NumeroDeCasosGravesCompletados = 2
            },
            new DoctorPerformanceDto
            {
                // DoctorId = Guid.NewGuid(),
                DoctorName = "Dr. Sofia Almeida",
                AssignedReports = 18,
                CompletedReports = 6,
                PendingReports = 11,
                ExpiredReports = 0,
                CancelledReports = 1,
                // AverageReviewTimeHours = 26.7,
                // CompletionRate = 88.9,
                // NumeroDeCasosGravesCompletados = 4
            },
            new DoctorPerformanceDto
            {
                // DoctorId = Guid.NewGuid(),
                DoctorName = "Dr. Pedro Fernandes",
                AssignedReports = 14,
                CompletedReports = 1,
                PendingReports = 12,
                ExpiredReports = 0,
                CancelledReports = 1,
                // AverageReviewTimeHours = 29.3,
                // CompletionRate = 78.6,
                // NumeroDeCasosGravesCompletados = 1
            },
            new DoctorPerformanceDto
            {
                //DoctorId = Guid.NewGuid(),
                DoctorName = "Dr. Maria Oliveira",
                AssignedReports = 21,
                CompletedReports = 14,
                PendingReports = 6,
                ExpiredReports = 0,
                CancelledReports = 1,
                // AverageReviewTimeHours = 25.6,
                // CompletionRate = 87.5,
                // NumeroDeCasosGravesCompletados = 3
            },
            new DoctorPerformanceDto
            {
                //DoctorId = Guid.NewGuid(),
                DoctorName = "Dr. Luís Santos",
                AssignedReports = 13,
                CompletedReports = 9,
                PendingReports = 3,
                ExpiredReports = 0,
                CancelledReports = 1,
                // AverageReviewTimeHours = 27.8,
                // CompletionRate = 81.8,
                // NumeroDeCasosGravesCompletados = 2
            },
        }
        };


    }





    // public async Task<SectionResponsibleMunicipalDashboardDto> GetDashboardAsync(DashboardFilterDto filter)
    // {
    //     var user = _userContextService.GetUserId();

    //     var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
    //                             .FirstOrDefaultAsync(sr => sr.UserId == user)
    //                             ?? throw new Exception("Section Responsible not found for the current user.");

    //     var municipalityId = sectionResponsible.MunicipalityId;

    //     var vaccineData = _vaccinationRepository.GetVaccineByFilter(municipalityId);

    //     var symptomData = _adverseEventRepository.GetSymptomFilter(municipalityId);

    //     var ditributionData = _adverseEventRepository.GetSeverityDistribution(municipalityId);

    // }

    public async Task<SectionResponsibleMunicipalDashboardDto> GetDashboardAsync(DashboardFilterDto filter)
    {
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
            TopVaccines = new List<VaccineStatsDto>
        {
            new()
            {
                VaccineName = "Abdala",
                TotalReports = 45
            },
            new()
            {
                VaccineName = "Soberana 02",
                TotalReports = 33
            },
            new()
            {
                VaccineName = "Pfizer-BioNTech",
                TotalReports = 25
            },
            new()
            {
                VaccineName = "Moderna",
                TotalReports = 18
            },
        },

            TopSymptoms = new List<SymptomStatsDto>
        {
            new()
            {
                SymptomName = "Fiebre",
                TotalReports = 52
            },
            new()
            {
                SymptomName = "Dolor de cabeza",
                TotalReports = 44
            },
            new()
            {
                SymptomName = "Fatiga",
                TotalReports = 31
            },
            new()
            {
                SymptomName = "Mareos",
                TotalReports = 16
            },
        },

            SeverityDistribution = new List<SeverityDistributionDto>
        {
            new()
            {
                Severity = "Grave",
                TotalReports = 18
            },
            new()
            {
                Severity = "Leve",
                TotalReports = 102
            },
        },


            ReportsTimeline = timeline,
            TotalDeaths = 5,
            TotalEmergencyRoom = 12,
            TotalLifeThreatening = 3,
            TotalPermanentDisability = 2,
            TotalVisitedDoctor = 25
        };
    }
}