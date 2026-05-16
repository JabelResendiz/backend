
using Finlay.PharmaVigilance.Application.DTO;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface ICatalogCommandService
{
    Task<string> CreateVaccineAsync(VaccineDto vaccineDto);
    Task<string> CreateSymptomAsync(SymptomDto symptomDto);
    Task<string> UpdateVaccineStatus(Guid vaccineId, bool isActive);
    Task<string> UpdateSymptomStatus(Guid symptomId, bool isActive);
    Task DeleteVaccine(Guid vaccineId);
}