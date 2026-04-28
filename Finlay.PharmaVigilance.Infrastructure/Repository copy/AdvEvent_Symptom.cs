

using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class AdverseEventSymptomRepository : GenericRepository<AdverseEventSymptom>, IAdverseEventSymptomRepository
{
    public AdverseEventSymptomRepository(FinlayDbContext context) : base(context) { }
}