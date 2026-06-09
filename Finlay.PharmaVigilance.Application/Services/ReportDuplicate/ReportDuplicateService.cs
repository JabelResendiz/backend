using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.Helpers;
using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Common;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;


namespace Finlay.PharmaVigilance.Application.Services;

public class ReportDuplicateService : IReportDuplicateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReportDuplicateRepository _reportDuplicate;
    private readonly IUserContextService _userContextService;

    public ReportDuplicateService(
        IUnitOfWork unitOfWork,
        IUserContextService userContextService,
        IReportDuplicateRepository reportDuplicateRepository
    )
    {
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
        _reportDuplicate = reportDuplicateRepository;
    }

    public async Task<ReportDuplicate?> ValidateAndRegisterAsync(AefiReport report)
    {
        try
        {
            var original = await _reportDuplicate.ValidateDuplicate(report);

            if (original == null)
                return null;

            var reportDuplicate = new ReportDuplicate
            {
                EnumReportDuplicate = EnumReportDuplicate.IsPossibleDuplicate,
                AefiReportOriginalId = original.Id,
                AefiReportCopyId = report.Id,
                AefiReportOriginal = original,
                AefiReportCopy = report
            };

            report.Status = ReportStatus.Draft;

            await _unitOfWork.GetRepository<ReportDuplicate>().CreateAsync(reportDuplicate);

            return reportDuplicate;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"{ex}");
        }
    }


    public async Task ResolveAsync(Guid duplicateId, ResolveDuplicateDto dto)
    {

        var userId = _userContextService.GetUserId();

        var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
            .FirstOrDefaultAsync(sr => sr.UserId == userId)
            ?? throw new UnauthorizedAccessException("User is not a section responsible.");

        var duplicate = await _unitOfWork.GetRepository<ReportDuplicate>()
            .GetByIdAsync(duplicateId)
            ?? throw new KeyNotFoundException("Duplicate not found.");

        if (duplicate.EnumReportDuplicate != EnumReportDuplicate.IsPossibleDuplicate)
        {
            throw new InvalidOperationException("This duplicate has already been resolved");
        }

        var originalReport = await _unitOfWork.GetRepository<AefiReport>()
                .GetByIdAsync(duplicate.AefiReportOriginalId, includes: a => a.VaccinatedSubject)
                ?? throw new KeyNotFoundException("Original report not found.");

        var copyReport = await _unitOfWork.GetRepository<AefiReport>()
        .GetByIdAsync(duplicate.AefiReportCopyId)
        ?? throw new KeyNotFoundException("Copy report not found.");

        if (originalReport.VaccinatedSubject.MunicipalityId != sectionResponsible.MunicipalityId)
        {
            throw new UnauthorizedAccessException("You can only resolve duplicates for reports in your municipality.");
        }


        switch (dto.Verdict)
        {
            case EnumReportDuplicate.ConfirmedDuplicate:
                copyReport.Status = ReportStatus.Rejected;
                duplicate.EnumReportDuplicate = EnumReportDuplicate.ConfirmedDuplicate;
                break;

            case EnumReportDuplicate.SeparateAsNew:
                copyReport.Status = ReportStatus.Submitted;
                duplicate.EnumReportDuplicate = EnumReportDuplicate.SeparateAsNew;
                break;
        }

        duplicate.ResolvedByUserId = userId;
        duplicate.ResolvedAt = TimeZoneHelper.GetEasternNow();

        await _unitOfWork.CompleteAsync();

    }


    public async Task<PagedResultDto<ReportDuplicateDto>> GetPendingAsync(PagedRequestDto paged)
    {
        var userId = _userContextService.GetUserId();

        return await _reportDuplicate.GetPendingAsync(userId, paged);
    }


    public async Task<ReportDuplicateDetailDto> GetByIdAsync(Guid id)
    {
        return await _reportDuplicate.GetDetailByIdAsync(id);
    }

}