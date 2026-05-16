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
                    Completed = g.Count(x => x.Status == ReportStatus.Approved)
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

        var avg = queryData
    .Average(x => (x.ReviewedAt - x.AssignedAt).TotalHours);


        return new MunicipalDashboardOverviewDto
        {
            TotalReports = data?.Total ?? 0,
            PendingReports = data?.Pending ?? 0,
            UnderReviewReports = data?.UnderReview ?? 0,
            CompletedReports = data?.Completed ?? 0,

            AverageReviewTimeHours = avg,

            CompletionRate = (data == null || data.Total == 0)
                ? 0
                : (double)data.Completed * 100 / data.Total
        };
    }




    public async Task<IEnumerable<DoctorPerformanceDto>> GetDoctorPerformanceAsync()
    {
        var user = _userContextService.GetUserId();

        var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
            .FirstOrDefaultAsync(sr => sr.UserId == user)
            ?? throw new Exception("Section Responsible not found for the current user.");

        var rawData = await _unitOfWork.GetRepository<MedicalReviewAssignment>()
            .GetAllByItems(r =>
                r.MedicalReviewer.MunicipalityId == sectionResponsible.MunicipalityId)
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
                DoctorId = g.Key.MedicalReviewerId,
                DoctorName = g.Key.DoctorName!,

                AssignedReports = g.Count(),

                CompletedReports = g.Count(x => x.Status == ReviewAssignmentStatus.Completed),

                PendingReports = g.Count(x => x.Status == ReviewAssignmentStatus.Pending),

                ExpiredReports = g.Count(x => x.Status == ReviewAssignmentStatus.Expired),

                CancelledReports = g.Count(x => x.Status == ReviewAssignmentStatus.Cancelled),

                AverageReviewTimeHours = g
                    .Where(x => x.ReviewedAt != null)
                    .Select(x => (x.ReviewedAt!.Value - x.AssignedAt).TotalHours)
                    .DefaultIfEmpty(0)
                    .Average(),

                CompletionRate = g.Count() == 0
                    ? 0
                    : (double)g.Count(x => x.Status == ReviewAssignmentStatus.Completed) * 100 / g.Count(),


            })
            .ToList();

        return result;
    }


}