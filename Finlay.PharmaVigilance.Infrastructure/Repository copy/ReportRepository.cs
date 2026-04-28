

using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class ReportRepository : GenericRepository<AefiReport>, IReportRepository
{
    public ReportRepository(FinlayDbContext context) : base(context) { }
}