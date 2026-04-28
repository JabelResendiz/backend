

using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class ReporterRepository : GenericRepository<Reporter>, IReporterRepository
{
    public ReporterRepository(FinlayDbContext context) : base(context) { }
}