

using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class SectionResponsibleRepository : GenericRepository<SectionResponsible>, ISectionResponsibleRepository
{
    public SectionResponsibleRepository(FinlayDbContext context) : base(context) { }
}