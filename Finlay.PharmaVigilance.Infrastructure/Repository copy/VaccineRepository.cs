

using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class VaccineRepository : GenericRepository<Vaccine>, IVaccineRepository
{
    public VaccineRepository(FinlayDbContext context) : base(context) { }

    public IQueryable<Vaccine> GetByFilter(string? search, bool? status)
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