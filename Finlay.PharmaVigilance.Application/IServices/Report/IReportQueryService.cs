using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.Enum;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IReportQueryService : IGenericQueryService<AefiReport, PublicAefiReportDto>
{
    Task<ReportUserDto> GetReportByNotificationNumber(string notificationNumber);

    Task<PagedResultDto<ReportMedicalReviewerDto>> GetReportAssigment(
        PagedRequestDto paged,
        ReportMedicalReviewerFilter filter);

    Task<PagedResultDto<ReportSectionResponsibleDto>> GetReportsBySectionResponsible(
        PagedRequestDto pagedRequestDto,
        ReportSectionResponsibleFilter filter);

    // Task<byte[]> GetReportPdfByNotificationNumber(string notificationNumber, ReportPdfTemplateType templateType);

    // Task<byte[]> GetReportDetailsPdfAsync(string notificationNumber);

    Task<PagedResultDto<ReportSummaryAdminDto>> GetFilter(
        PagedRequestDto paged,
        string? vaccineName,
        string? provinceName,
        string? severity,
        string? reportStatus
    );

    Task<ReportDashboardDto> GetReportDashboard();

    Task<ReportDetailAdminDto> GetReportDetailAdmin(Guid reportId);

}