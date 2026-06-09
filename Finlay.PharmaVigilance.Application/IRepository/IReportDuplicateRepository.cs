using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IRepository;

public interface IReportDuplicateRepository : IGenericRepository<ReportDuplicate>
{
    Task<PagedResultDto<ReportDuplicateDto>> GetPendingAsync(
            Guid userId,
            PagedRequestDto paged);

    Task<ReportDuplicateDetailDto> GetDetailByIdAsync(Guid id);

    Task<AefiReport?> ValidateDuplicate(AefiReport report);
}