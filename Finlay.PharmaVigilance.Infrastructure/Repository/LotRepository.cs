

using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class LotRepository : GenericRepository<Lot>, ILotRepository
{
    public LotRepository(FinlayDbContext context) : base(context) { }
}