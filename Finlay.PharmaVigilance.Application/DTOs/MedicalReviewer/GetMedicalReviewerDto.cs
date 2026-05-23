
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.DTO;

public class GetMedicalReviewerDto
{
    public required Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Institution { get; set; }

}

public class GetMedicalReviewerListDto
{
    public MedicalReviewer MedicalReviewer { get; set; } = null!;
    public double AverageTimeReview { get; set; }
}

public class GetMedicalReviewerDetailDto
{
    public string FullName { get; set; } = null!;
    public string Institution { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public int TotalAssignments { get; set; }
    public int PendingAssignments { get; set; }
    public int CompletedAssignments { get; set; }
    public int ExpiredAssignments { get; set; }
    public double AverageTimeReview { get; set; }
    public DateTime CreatedAt { get; set; }

}