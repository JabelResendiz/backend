

using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class MedicalReviewRepository : GenericRepository<MedicalReview>, IMedicalReviewRepository
{
    public MedicalReviewRepository(FinlayDbContext context) : base(context) { }
}