using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IServices;


public interface IReportDuplicateService
{
    Task<ReportDuplicate?> ValidateAndRegisterAsync(AefiReport report);

    Task ResolveAsync(Guid duplicateId, ResolveDuplicateDto dto);
    Task<ReportDuplicateDetailDto> GetByIdAsync(Guid id);
    Task<PagedResultDto<ReportDuplicateDto>> GetPendingAsync(PagedRequestDto request);
}