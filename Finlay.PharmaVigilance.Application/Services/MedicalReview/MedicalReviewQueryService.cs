using AutoMapper;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.Services;


public class MedicalReviewQueryService : GenericQueryService<MedicalReview, MedicalReviewDto>,
                                         IMedicalReviewQueryService
{
    public MedicalReviewQueryService(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {

    }
}