
using Finlay.PharmaVigilance.Application.DTO;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface ICatalogCommandService
{
    Task<string> CreateVaccineAsync(VaccineDto vaccineDto);
    Task<string> CreateSymptomAsync(SymptomDto symptomDto);
    Task<string> DeactivateVaccine(int vaccineId);
    Task<string> DeactivateSymptom(int symptomId);
    Task<string> ActivateVaccine(int vaccineId);
    Task<string> ActivateSymptom(int symptomId);
}