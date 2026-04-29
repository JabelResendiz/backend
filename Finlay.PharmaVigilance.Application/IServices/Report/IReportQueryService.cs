using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IReportQueryService : IGenericQueryService<AefiReport, PublicAefiReportDto>
{
    Task<ReportUserDto> GetReportByNotificationNumber(string notificationNumber);

    Task<PagedResultDto<ReportMedicalReviewerDto>> GetReportAssigment(PagedRequestDto paged);

    Task<PagedResultDto<ReportSectionResponsibleDto>> GetReportsBySectionResponsible(PagedRequestDto pagedRequestDto);



}