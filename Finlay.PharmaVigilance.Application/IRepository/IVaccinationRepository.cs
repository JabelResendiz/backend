using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IRepository;

public interface IVaccinationRepository : IGenericRepository<Vaccination>
{
    Task<IEnumerable<VaccineStatsDto>> GetVaccineByFilter(int municipalityId);

    Task<IEnumerable<VaccineStatusDto>> GetVaccineDistributionAsync();


}