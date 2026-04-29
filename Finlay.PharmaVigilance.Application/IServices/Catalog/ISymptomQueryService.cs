using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface ISymptomQueryService : IGenericQueryService<Symptom, GetSymptomDto>
{
    Task<PagedResultDto<GetSymptomDto>> GetActivesSymptoms(PagedRequestDto paged);

    Task<IEnumerable<GetSymptomDto>> GetActiveSymptomsLookup();
    Task<PagedResultDto<GetPrivateSymptomsDto>> GetByFilters(PagedRequestDto paged, string? search, bool? status);

}