using Finlay.PharmaVigilance.Application.DTO;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IManufacturerQueryService
{
    Task<IEnumerable<ManufacturerResponseDto>> GetManufacturers();
}