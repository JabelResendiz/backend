using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;

public class MedicalReview : GuidEntity
{
    public Guid MedicalReviewAssignmentId { get; set; }
    public ClinicalSignificance ClinicalSignificance { get; set; }
    public CausalityLevel Causality { get; set; }
    public DateTime ReviewedAt { get; set; }

    public MedicalReviewAssignment MedicalReviewAssignment { get; set; } = null!;
}