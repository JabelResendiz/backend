using Finlay.PharmaVigilance.Application.DTO;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface ILotQueryService
{
    Task<IEnumerable<LotResponseDto>> GetByVaccine(Guid vaccineId);
}