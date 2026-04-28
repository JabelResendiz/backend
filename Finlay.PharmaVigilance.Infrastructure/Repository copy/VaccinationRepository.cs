

using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class VaccinationRepository : GenericRepository<Vaccination>, IVaccinationRepository
{
    public VaccinationRepository(FinlayDbContext context) : base(context) { }
}