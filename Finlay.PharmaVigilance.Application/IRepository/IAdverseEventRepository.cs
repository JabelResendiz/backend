using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IRepository;

public interface IAdverseEventRepository : IGenericRepository<AdverseEvent>
{
    Task<IEnumerable<SymptomStatsDto>> GetSymptomFilter(int municipalityId);

    Task<IEnumerable<SeverityDistributionDto>> GetSeverityDistribution(int municipalityId);

    Task<SeriousDataDto> GetSeriousDataAsync(int municipalityId);
}