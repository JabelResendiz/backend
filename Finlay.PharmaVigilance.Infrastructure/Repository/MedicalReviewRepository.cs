

using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class MedicalReviewRepository : GenericRepository<MedicalReview>, IMedicalReviewRepository
{
    public MedicalReviewRepository(FinlayDbContext context) : base(context) { }
}