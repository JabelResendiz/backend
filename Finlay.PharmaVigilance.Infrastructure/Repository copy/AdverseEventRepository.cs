

using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class AdverseEventRepository : GenericRepository<AdverseEvent>, IAdverseEventRepository
{
    public AdverseEventRepository(FinlayDbContext context) : base(context) { }
}