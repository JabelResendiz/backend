

using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class SymptomRepository : GenericRepository<Symptom>, ISymptomRepository
{
    public SymptomRepository(FinlayDbContext context) : base(context) { }


    public IQueryable<Symptom> GetByFilter(string? search, bool? status)
    {
        var query = _entity.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(v => v.NormalizedName!.Contains(search.ToUpper()));
        }

        if (status.HasValue)
        {
            query = query.Where(v => v.IsActive == status.Value);
        }

        return query;

    }
}