using Finlay.PharmaVigilance.Application.DTO;
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

    public MunicipalDashboardService(
        IUnitOfWork unitOfWork,
        ILogger<MunicipalDashboardService> logger,
        IUserContextService userContextService
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _userContextService = userContextService;
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
                    Rejected = g.Count(x => x.Status == ReportStatus.Rejected)
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
            AverageReviewTimeHours = avg,

            CompletionRate = (data == null || data.Total == 0)
                ? 0
                : (double)data.Completed * 100 / data.Total
        };
    }




    // public async Task<IEnumerable<DoctorPerformanceDto>> GetDoctorPerformanceAsync()
    // {
    //     var user = _userContextService.GetUserId();

    //     var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
    //         .FirstOrDefaultAsync(sr => sr.UserId == user)
    //         ?? throw new Exception("Section Responsible not found for the current user.");

    //     var rawData = await _unitOfWork.GetRepository<MedicalReviewAssignment>()
    //         .GetAllByItems(r =>
    //             r.MedicalReviewer.MunicipalityId == sectionResponsible.MunicipalityId)
    //         .Select(r => new
    //         {
    //             r.MedicalReviewerId,
    //             DoctorName = r.MedicalReviewer.User.UserName,
    //             r.Status,
    //             r.AssignedAt,
    //             ReviewedAt = r.MedicalReview != null
    //                 ? r.MedicalReview.ReviewedAt
    //                 : (DateTime?)null
    //         })
    //         .ToListAsync();

    //     var result = rawData
    //         .GroupBy(x => new { x.MedicalReviewerId, x.DoctorName })
    //         .Select(g => new DoctorPerformanceDto
    //         {
    //             DoctorId = g.Key.MedicalReviewerId,
    //             DoctorName = g.Key.DoctorName!,

    //             AssignedReports = g.Count(),

    //             CompletedReports = g.Count(x => x.Status == ReviewAssignmentStatus.Completed),

    //             PendingReports = g.Count(x => x.Status == ReviewAssignmentStatus.Pending),

    //             ExpiredReports = g.Count(x => x.Status == ReviewAssignmentStatus.Expired),

    //             CancelledReports = g.Count(x => x.Status == ReviewAssignmentStatus.Cancelled),

    //             AverageReviewTimeHours = g
    //                 .Where(x => x.ReviewedAt != null)
    //                 .Select(x => (x.ReviewedAt!.Value - x.AssignedAt).TotalHours)
    //                 .DefaultIfEmpty(0)
    //                 .Average(),

    //             CompletionRate = g.Count() == 0
    //                 ? 0
    //                 : (double)g.Count(x => x.Status == ReviewAssignmentStatus.Completed) * 100 / g.Count(),


    //         })
    //         .ToList();

    //     return result;
    // }

    public async Task<IEnumerable<DoctorPerformanceDto>> GetDoctorPerformanceAsync()
    {
        return new List<DoctorPerformanceDto>()
        {
            new DoctorPerformanceDto
            {
                DoctorId = Guid.NewGuid(),
                DoctorName = "Dr. Martha Silva",
                AssignedReports = 10,
                CompletedReports = 8,
                PendingReports = 1,
                ExpiredReports = 0,
                CancelledReports = 1,
                AverageReviewTimeHours = 24.5,
                CompletionRate = 80
            },
            new DoctorPerformanceDto
            {
                DoctorId = Guid.NewGuid(),
                DoctorName = "Dr. Carlos Mendes",
                AssignedReports = 15,
                CompletedReports = 12,
                PendingReports = 2,
                ExpiredReports = 1,
                CancelledReports = 0,
                AverageReviewTimeHours = 30.2,
                CompletionRate = 80,
                NumeroDeCasosGravesCompletados = 3
            },
            new DoctorPerformanceDto
            {
                DoctorId = Guid.NewGuid(),
                DoctorName = "Dr. Ana Pereira",
                AssignedReports = 20,
                CompletedReports = 18,
                PendingReports = 1,
                ExpiredReports = 0,
                CancelledReports = 1,
                AverageReviewTimeHours = 22.8,
                CompletionRate = 90,
                NumeroDeCasosGravesCompletados = 5
            },
            new DoctorPerformanceDto
            {
                DoctorId = Guid.NewGuid(),
                DoctorName = "Dr. João Costa",
                AssignedReports = 12,
                CompletedReports = 7,
                PendingReports = 4,
                ExpiredReports = 0,
                CancelledReports = 1,
                AverageReviewTimeHours = 28.4,
                CompletionRate = 83.3,
                NumeroDeCasosGravesCompletados = 2
            },
            new DoctorPerformanceDto
            {
                DoctorId = Guid.NewGuid(),
                DoctorName = "Dr. Sofia Almeida",
                AssignedReports = 18,
                CompletedReports = 6,
                PendingReports = 11,
                ExpiredReports = 0,
                CancelledReports = 1,
                AverageReviewTimeHours = 26.7,
                CompletionRate = 88.9,
                NumeroDeCasosGravesCompletados = 4
            },
            new DoctorPerformanceDto
            {
                DoctorId = Guid.NewGuid(),
                DoctorName = "Dr. Pedro Fernandes",
                AssignedReports = 14,
                CompletedReports = 1,
                PendingReports = 12,
                ExpiredReports = 0,
                CancelledReports = 1,
                AverageReviewTimeHours = 29.3,
                CompletionRate = 78.6,
                NumeroDeCasosGravesCompletados = 1
            },
            new DoctorPerformanceDto
            {
                DoctorId = Guid.NewGuid(),
                DoctorName = "Dr. Maria Oliveira",
                AssignedReports = 21,
                CompletedReports = 14,
                PendingReports = 6,
                ExpiredReports = 0,
                CancelledReports = 1,
                AverageReviewTimeHours = 25.6,
                CompletionRate = 87.5,
                NumeroDeCasosGravesCompletados = 3
            },
            new DoctorPerformanceDto
            {
                DoctorId = Guid.NewGuid(),
                DoctorName = "Dr. Luís Santos",
                AssignedReports = 13,
                CompletedReports = 9,
                PendingReports = 3,
                ExpiredReports = 0,
                CancelledReports = 1,
                AverageReviewTimeHours = 27.8,
                CompletionRate = 81.8,
                NumeroDeCasosGravesCompletados = 2
            },
        };
    }

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