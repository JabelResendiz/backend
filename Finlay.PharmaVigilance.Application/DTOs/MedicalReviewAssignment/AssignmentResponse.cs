using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class AssignmentResponse
{
    public required Guid Id { get; set; }
    public required string MedicalReviewerName { get; set; }
    public required DateTime AssignedAt { get; set; }
    public required string SectionResponsibleName { get; set; }
    public required ReviewAssignmentStatus Status { get; set; }
    public string? RejectionReason { get; set; }

}