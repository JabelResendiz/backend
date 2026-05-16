using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;

public class MedicalReviewAssignment : GuidEntity
{
    public Guid SectionResponsibleId { get; set; }
    public Guid MedicalReviewerId { get; set; }
    public Guid AefiReportId { get; set; }
    public DateTime AssignedAt { get; set; }
    public ReviewAssignmentStatus Status { get; set; }
    public string? RejectionReason { get; set; }


    public SectionResponsible SectionResponsible { get; set; } = null!;
    public MedicalReviewer MedicalReviewer { get; set; } = null!;
    public AefiReport AefiReport { get; set; } = null!;


    public MedicalReview? MedicalReview { get; set; }

}