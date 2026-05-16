

using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class VaccinationCenterRepository : GenericRepository<VaccinationCenter>, IVaccinationCenterRepository
{
    public VaccinationCenterRepository(FinlayDbContext context) : base(context) { }
}