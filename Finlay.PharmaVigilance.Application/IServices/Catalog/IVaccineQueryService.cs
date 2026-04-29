using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IVaccineQueryService : IGenericQueryService<Vaccine, GetVaccineDto>
{
    Task<PagedResultDto<GetVaccineDto>> GetActivesVaccine(PagedRequestDto paged);

    Task<IEnumerable<GetVaccineDto>> GetActiveVaccinesLookup();

    Task<PagedResultDto<GetVaccineDto>> GetByFilters(PagedRequestDto paged, string? search, bool? status);

}