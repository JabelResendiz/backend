using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IVaccinationCenterCommandService
{
    Task CreateVaccinationCenter(VaccinationCenterDto vaccinationCenterDto);
}