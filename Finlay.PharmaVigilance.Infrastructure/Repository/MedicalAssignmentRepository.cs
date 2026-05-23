using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;


public class MedicalAssignmentRepository : GenericRepository<MedicalReviewAssignment>,
                                            IMedicalAssignmentRepository
{

    public MedicalAssignmentRepository(FinlayDbContext context) : base(context) { }

    public async Task<ICollection<DoctorPerformanceDto>> GetDoctorPerformanceAsync(int municipalityId)
    {
        var rawData = await _entity
            .Where(r =>
                r.MedicalReviewer.MunicipalityId == municipalityId)
            .Select(r => new
            {
                r.MedicalReviewerId,
                DoctorName = r.MedicalReviewer.User.UserName,
                r.Status,
                r.AssignedAt,
                ReviewedAt = r.MedicalReview != null
                    ? r.MedicalReview.ReviewedAt
                    : (DateTime?)null
            })
            .ToListAsync();

        var result = rawData
            .GroupBy(x => new { x.MedicalReviewerId, x.DoctorName })
            .Select(g => new DoctorPerformanceDto
            {
                DoctorName = g.Key.DoctorName!,

                AssignedReports = g.Count(),

                CompletedReports = g.Count(x => x.Status == ReviewAssignmentStatus.Completed),

                PendingReports = g.Count(x => x.Status == ReviewAssignmentStatus.Pending),

                ExpiredReports = g.Count(x => x.Status == ReviewAssignmentStatus.Expired),

                CancelledReports = g.Count(x => x.Status == ReviewAssignmentStatus.Cancelled),
            })
            .ToList();

        return result;
    }

    public async Task<ICollection<TimeHourDto>> GetTimeHoursAsync(int municipalityId)
    {
        var reports = await _entity
            .Where(r =>
                r.Status == ReviewAssignmentStatus.Completed &&
                r.MedicalReview != null &&
                r.MedicalReviewer.MunicipalityId == municipalityId)
            .Select(r => new
            {
                AssignedAt = r.AssignedAt,
                ReviewedAt = r.MedicalReview!.ReviewedAt
            })
            .ToListAsync();

        var reportsWithHours = reports
            .Select(r => new
            {
                Hours = (r.ReviewedAt - r.AssignedAt).TotalHours
            })
            .ToList();

        var result = new List<TimeHourDto>
        {
            new()
            {
                Hour = "0 - 5 horas",
                TotalReport = reportsWithHours.Count(r =>
                    r.Hours >= 0 && r.Hours < 5)
            },

            new()
            {
                Hour = "5 - 10 horas",
                TotalReport = reportsWithHours.Count(r =>
                    r.Hours >= 5 && r.Hours < 10)
            },

            new()
            {
                Hour = "10 - 24 horas",
                TotalReport = reportsWithHours.Count(r =>
                    r.Hours >= 10 && r.Hours < 24)
            },

            new()
            {
                Hour = "+24 horas",
                TotalReport = reportsWithHours.Count(r =>
                    r.Hours >= 24)
            }
        };

        return result;
    }

    public async Task<MunicipalMetricsDto> GetMetrics(Guid sectionResponsibleId)
    {
        var data = await _entity
                    .Where(a =>
                        a.SectionResponsibleId == sectionResponsibleId)
                    .Select(a => new
                    {
                        ReportId = a.AefiReportId,

                        AssignedAt = a.AssignedAt,

                        ReportCreatedAt = a.AefiReport.ReportDate,

                        ReviewedAt = a.MedicalReview != null
                            ? a.MedicalReview.ReviewedAt
                            : (DateTime?)null
                    })
                    .ToListAsync();

        double averageAssignmentByReport = 0;

        var totalReports = data
            .Select(x => x.ReportId)
            .Distinct()
            .Count();

        if (totalReports > 0)
        {
            averageAssignmentByReport = Math.Round(
                (double)data.Count / totalReports,
                2
            );
        }







        double averageReviewTimeHours = 0;

        var completed = data
            .Where(x => x.ReviewedAt != null)
            .ToList();

        if (completed.Any())
        {
            averageReviewTimeHours = Math.Round(
                completed.Average(x =>
                    (x.ReviewedAt!.Value - x.AssignedAt).TotalHours),
                2
            );
        }



        double averageAssignmentTimeHours = 0;

        if (data.Any())
        {
            averageAssignmentTimeHours = Math.Round(
                data.Average(x =>
                    (x.AssignedAt - x.ReportCreatedAt).TotalHours),
                2
            );
        }


        return new MunicipalMetricsDto
        {
            AverageAssignmentByReport = averageAssignmentByReport,
            AverageReviewTimeHours = averageReviewTimeHours,
            AverageAssignmentTimeHours = averageAssignmentTimeHours
        };

    }
}