using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;


public class MedicalReviewResponseDto
{
    public ClinicalSignificance ClinicalSignificance { get; set; }
    public CausalityLevel Causality { get; set; }
    public required Guid MedicalReviewAssignmentId { get; set; }
    public DateTime ReviewedAt { get; set; }
}

