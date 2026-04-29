using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IRepository;

public interface ISymptomRepository : IGenericRepository<Symptom>
{
    IQueryable<Symptom> GetByFilter(string? search, bool? status);
}