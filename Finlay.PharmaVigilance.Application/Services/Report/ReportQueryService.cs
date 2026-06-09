
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Common;
using Finlay.PharmaVigilance.Application.IServices.Pdf;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Application.Enum;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Application.Services;


public class ReportQueryService : GenericQueryService<AefiReport, PublicAefiReportDto>,
                                  IReportQueryService
{
    private static readonly Expression<Func<AefiReport, object>>[] includes =
                        { e => e.VaccinatedSubject,
                        e=> e.Reporter,
                         e=> e.Vaccinations,
                         e=> e.AdverseEvents
                        };

    private readonly IUserContextService _userContextService;
    private readonly IPdfService _pdfService;
    private readonly IReportRepository _reportRepository;

    public ReportQueryService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserContextService userContextService,
        IReportRepository reportRepository,
        IPdfService pdfService)
        : base(unitOfWork, mapper)
    {
        _userContextService = userContextService;
        _reportRepository = reportRepository;
        _pdfService = pdfService;
    }

    public override Expression<Func<AefiReport, object>>[] GetIncludes() => includes;



    public async Task<ReportUserDto> GetReportByNotificationNumber(string notificationNumber)
    {
        var report = await _unitOfWork.GetRepository<AefiReport>()
            .GetAllByItems(ar => ar.NotificationNumber == notificationNumber)
            .AsNoTracking()
            .ProjectTo<ReportUserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync()
            ?? throw new ArgumentNullException("Report not found");

        var assignment = await _unitOfWork.GetRepository<MedicalReviewAssignment>()
            .GetAllByItems(x => x.AefiReport.NotificationNumber == notificationNumber)
            .OrderByDescending(x => x.AssignedAt)
            .FirstOrDefaultAsync();

        if (assignment != null)
        {
            report.AssignedAt = assignment.AssignedAt;

            var review = await _unitOfWork.GetRepository<MedicalReview>()
                .FirstOrDefaultAsync(x => x.MedicalReviewAssignmentId == assignment.Id);

            if (review != null)
                report.ReviewedAt = review.ReviewedAt;
        }

        return report;
    }

    public async Task<byte[]> GetReportPdfByNotificationNumber(string notificationNumber, ReportPdfTemplateType templateType)
    {
        if (string.IsNullOrWhiteSpace(notificationNumber))
            throw new ArgumentNullException(nameof(notificationNumber), "Notification number is required.");

        var report = await _unitOfWork.GetRepository<AefiReport>()
                        .GetAllByItems(ar => ar.NotificationNumber == notificationNumber)
                        .AsNoTracking()
                        .ProjectTo<ReportPdfDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync() ?? throw new ArgumentNullException("Report not found");

        return _pdfService.GenerateReportPdf(report, templateType);
    }


    public async Task<PagedResultDto<ReportMedicalReviewerDto>> GetReportAssigment(
        PagedRequestDto paged,
        ReportMedicalReviewerFilter filter)
    {
        var userId = _userContextService.GetUserId();

        var medicalReviewer = await _unitOfWork.GetRepository<MedicalReviewer>()
                                        .FirstOrDefaultAsync(sr => sr.UserId == userId);

        if (medicalReviewer == null)
            throw new UnauthorizedAccessException("User is not a medical reviewer");

        var reportId = _unitOfWork.GetRepository<MedicalReviewAssignment>()
                                .GetAllByItems(
                                mra => mra.MedicalReviewerId == medicalReviewer.Id &&
                                mra.Status == ReviewAssignmentStatus.Pending)
                                .Select(a => a.AefiReportId)
                                .Distinct();

        var query = _unitOfWork.GetRepository<AefiReport>()
                               .GetAllByItems(r => reportId.Contains(r.Id));


        query = _reportRepository
                        .GetMedicalReviewerByFilter(
                            query,
                            filter);

        var totalItems = await query.CountAsync();

        var items = await _unitOfWork.GetRepository<AefiReport>()
                        .GetPaged(query, (paged.PageNumber - 1) * paged.PageSize, paged.PageSize)
                        .AsNoTracking()
                        .ProjectTo<ReportMedicalReviewerDto>(_mapper.ConfigurationProvider)
                        .ToListAsync();


        return new PagedResultDto<ReportMedicalReviewerDto>
        {
            Items = items,
            TotalCount = totalItems,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            NextPageUrl = paged.PageNumber * paged.PageSize < totalItems
                       ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber + 1}&pageSize={paged.PageSize}"
                       : null,
            PreviousPageUrl = paged.PageNumber > 1
                       ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber - 1}&pageSize={paged.PageSize}"
                       : null

        };


    }

    public async Task<PagedResultDto<ReportSectionResponsibleDto>> GetReportsBySectionResponsible(
        PagedRequestDto paged,
        ReportSectionResponsibleFilter filter)
    {
        var userId = _userContextService.GetUserId();

        var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
                                        .FirstOrDefaultAsync(sr => sr.UserId == userId);

        if (sectionResponsible == null)
            throw new UnauthorizedAccessException("User is not a section responsible");

        if (filter.VaccinationCenterId != null)
        {
            var vaccinationCenter = await _unitOfWork.GetRepository<VaccinationCenter>()
                    .GetByIdAsync(filter.VaccinationCenterId)
                    ?? throw new ArgumentException("The specified vaccination center does not exist.");

            if (vaccinationCenter.MunicipalityId != sectionResponsible.MunicipalityId)
            {
                throw new ArgumentException("The vaccination center does not belong to the section responsible's municipality.");
            }
        }


        var reportIds = _unitOfWork.GetRepository<Alert>()
                            .GetAllByItems(a => a.SectionResponsibleId == sectionResponsible.Id)
                            .Select(a => a.AefiReportId)
                            .Distinct();


        IQueryable<AefiReport> reportsQuery = _unitOfWork.GetRepository<AefiReport>()
                                .GetAllByItems(r => reportIds.Contains(r.Id));

        reportsQuery = _reportRepository
                        .GetSectionResponsibleByFilter(
                            reportsQuery,
                            filter);

        var totalItems = await reportsQuery.CountAsync();

        var items = await _unitOfWork.GetRepository<AefiReport>()
                        .GetPaged(reportsQuery, (paged.PageNumber - 1) * paged.PageSize, paged.PageSize)
                        .AsNoTracking()
                        .ProjectTo<ReportSectionResponsibleDto>(_mapper.ConfigurationProvider)
                        .ToListAsync();

        return new PagedResultDto<ReportSectionResponsibleDto>
        {
            Items = items,
            TotalCount = totalItems,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            NextPageUrl = paged.PageNumber * paged.PageSize < totalItems
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber + 1}&pageSize={paged.PageSize}"
                        : null,
            PreviousPageUrl = paged.PageNumber > 1
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber - 1}&pageSize={paged.PageSize}"
                        : null

        };

    }

    public async Task<PagedResultDto<ReportSummaryAdminDto>> GetFilter(
        PagedRequestDto paged,
        string? vaccineName,
        string? provinceName,
        string? severity,
        string? reportStatus
    )
    {

        var query = _reportRepository.GetByFilter(
            vaccineName, provinceName, severity, reportStatus);


        var totalItems = await query.CountAsync();

        var items = await _reportRepository
                    .GetPaged(query, (paged.PageNumber - 1) * paged.PageSize, paged.PageSize)
                    .AsNoTracking()
                    .ProjectTo<ReportSummaryAdminDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

        var reportIds = items.Select(i => i.Id).ToList();

        var reviewerData = await _unitOfWork.GetRepository<MedicalReviewer>()
                            .GetAll()
                            .SelectMany(mr => mr.MedicalReviews, (mr, mra) => new
                            {
                                mra.AefiReportId,
                                mr.User.UserName
                            })
                            .Where(x => reportIds.Contains(x.AefiReportId))
                            .ToListAsync();

        // Diccionario para lookup rápido
        var reviewerMap = reviewerData
                        .GroupBy(x => x.AefiReportId)
                        .ToDictionary(x => x.Key, x => x.First().UserName);

        // Map en memoria
        foreach (var item in items)
        {
            if (reviewerMap.TryGetValue(item.Id, out var userName))
            {
                item.MedicalReviewerName = userName;
            }
        }


        return new PagedResultDto<ReportSummaryAdminDto>
        {
            Items = items,
            TotalCount = totalItems,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            NextPageUrl = paged.PageNumber * paged.PageSize < totalItems
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber + 1}&pageSize={paged.PageSize}"
                        : null,
            PreviousPageUrl = paged.PageNumber > 1
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber - 1}&pageSize={paged.PageSize}"
                        : null

        };
    }


    public async Task<ReportDashboardDto> GetReportDashboard()
    {

        return new ReportDashboardDto
        {
            TotalReport = await _unitOfWork.GetRepository<AefiReport>()
                            .GetAll()
                            .CountAsync(),
            CompletedReport = await _unitOfWork.GetRepository<AefiReport>()
                                .GetAllByItems(ar => ar.Status == ReportStatus.Approved)
                                .CountAsync(),
            InRevisionReport = await _unitOfWork.GetRepository<AefiReport>()
                                .GetAllByItems(ar => ar.Status == ReportStatus.UnderReview)
                                .CountAsync(),
            TodayReport = await _unitOfWork.GetRepository<AefiReport>()
                                .GetAllByItems(ar => ar.ReportDate.Date == DateTime.Today.Date)
                                .CountAsync(),
        };
    }


    public async Task<ReportDetailAdminDto> GetReportDetailAdmin(Guid reportId)
    {
        var reportDetail = await _reportRepository
                        .GetAllByItems(r => r.Id == reportId)
                        .AsNoTracking()
                        .ProjectTo<ReportDetailAdminDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync() ?? throw new ArgumentNullException("Report not found");


        return reportDetail;

    }


}