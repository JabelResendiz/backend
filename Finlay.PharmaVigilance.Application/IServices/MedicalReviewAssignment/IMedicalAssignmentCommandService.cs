using Finlay.PharmaVigilance.Application.DTO;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IMedicalReviewAssignmentCommandService : IGenericCommandService<MedicalReviewAssignmentDTO, MedicalReviewAssignmentDTO>
{
    Task ReassignedAsync(MedicalReviewAssignmentDTO reportDto);
}