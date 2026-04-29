using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IReportQueryService : IGenericQueryService<AefiReport, PublicAefiReportDto>
{
    Task<ReportSummaryDto> GetReportByNotificationNumber(string notificationNumber);

    Task<PagedResultDto<ReportDetailDto>> GetReportAssigment(PagedRequestDto paged);

    Task<PagedResultDto<ReportSummaryDto>> GetReportsBySectionResponsible(PagedRequestDto pagedRequestDto);



}