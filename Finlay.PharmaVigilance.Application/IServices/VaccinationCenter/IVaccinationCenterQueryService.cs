using Finlay.PharmaVigilance.Application.DTO;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IVaccinationCenterQueryService
{
    Task<IEnumerable<VaccinationCenterResponseDto>> GetByMunicipality(int municipalityId, int provinceId);
    Task<IEnumerable<VaccinationCenterResponseDto>> GetBySectionResponsible();
}