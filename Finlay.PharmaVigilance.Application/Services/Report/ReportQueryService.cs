
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Common;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
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

    public ReportQueryService(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
        : base(unitOfWork, mapper)
    {
        _userContextService = userContextService;
    }

    public override Expression<Func<AefiReport, object>>[] GetIncludes() => includes;



    public async Task<ReportSummaryDto> GetReportByNotificationNumber(string notificationNumber)
    {

        return await _unitOfWork.GetRepository<AefiReport>()
                        .GetAllByItems(ar => ar.NotificationNumber == notificationNumber)
                        .ProjectTo<ReportSummaryDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync() ?? throw new ArgumentNullException("Report not found");
    }


    public async Task<PagedResultDto<ReportDetailDto>> GetReportAssigment(PagedRequestDto paged)
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


        var totalItems = await query.CountAsync();

        var items = await _unitOfWork.GetRepository<AefiReport>()
                        .GetPaged(query, (paged.PageNumber - 1) * paged.PageSize, paged.PageSize)
                        .ProjectTo<ReportDetailDto>(_mapper.ConfigurationProvider)
                        .ToListAsync();


        return new PagedResultDto<ReportDetailDto>
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

    public async Task<PagedResultDto<ReportSummaryDto>> GetReportsBySectionResponsible(PagedRequestDto paged)
    {
        var userId = _userContextService.GetUserId();

        var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
                                        .FirstOrDefaultAsync(sr => sr.UserId == userId);

        if (sectionResponsible == null)
            throw new UnauthorizedAccessException("User is not a section responsible");

        var reportIds = _unitOfWork.GetRepository<Alert>()
                            .GetAllByItems(a => a.SectionResponsibleId == sectionResponsible.Id &&
                                a.AefiReport.Status == ReportStatus.Submitted)
                            .Select(a => a.AefiReportId)
                            .Distinct();

        var reportsQuery = _unitOfWork.GetRepository<AefiReport>()
                                .GetAllByItems(r => reportIds.Contains(r.Id))
                                .OrderByDescending(r => r.ReportDate);


        var totalItems = await reportsQuery.CountAsync();

        var items = await _unitOfWork.GetRepository<AefiReport>()
                        .GetPaged(reportsQuery, (paged.PageNumber - 1) * paged.PageSize, paged.PageSize)
                        .ProjectTo<ReportSummaryDto>(_mapper.ConfigurationProvider)
                        .ToListAsync();

        return new PagedResultDto<ReportSummaryDto>
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


}