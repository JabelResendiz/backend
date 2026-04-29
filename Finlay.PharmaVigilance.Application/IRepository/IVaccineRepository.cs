using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IRepository;

public interface IVaccineRepository : IGenericRepository<Vaccine>
{
    IQueryable<Vaccine> GetByFilter(string? search, bool? status);
}