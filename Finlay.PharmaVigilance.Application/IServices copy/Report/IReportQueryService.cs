using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IReportQueryService : IGenericQueryService<AefiReport, PublicAefiReportDto>
{
    Task<ReportDetailDto> GetReportByNotificationNumber(string notificationNumber);
    Task<IEnumerable<ReportDetailDto>> GetReportAssigment();

    Task<PagedResultDto<ReportSummaryDto>> GetReportsBySectionResponsible(PagedRequestDto pagedRequestDto);



}