

using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class VaccinatedSubjectRepository : GenericRepository<VaccinatedSubject>, IVaccinatedSubjectRepository
{
    public VaccinatedSubjectRepository(FinlayDbContext context) : base(context) { }
}